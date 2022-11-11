using HatTrick.Model.Sql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using static Google.Protobuf.Reflection.UninterpretedOption.Types;
using Google.Protobuf;
using System.Security.Cryptography;
using System.Text;
using MySqlX.XDevAPI.Relational;
using MySqlX.XDevAPI;

namespace HatTrick.Model.MySql
{
    public class MySqlModelBuilder : SqlModelBuilder<MySqlModel>
    {
        #region internals
        private string _sqlConnectionString;
        private List<string> _databases;
        #endregion

        #region constructors
        public MySqlModelBuilder(string sqlConnectionString, string database, params string[] databases) : base("HatTrick.Model.MySql.Scripts")
        {
            _sqlConnectionString = sqlConnectionString;
            _databases = new List<string>() { $"'{database}'" }.Concat(databases.Select(d => $"'{d}'")).ToList();
        }
        #endregion

        #region get connection
        protected override DbConnection GetConnection()
        {
            return new MySqlConnection(_sqlConnectionString);
        }
        #endregion

        #region build
        protected override void BuildModel(MySqlModel model)
        {
            this.ResolveName(model);
            this.ResolveSchemas(model);
            this.ResolveTables(model);
            this.ResolveTableColumns(model);
            this.ResolveTableIndexes(model);
            this.ResolveViews(model);
            this.ResolveViewColumns(model);
            this.ResolveProcedures(model);
            this.ResolveProcedureParameters(model);
            this.ResolveRelationships(model);
            this.ResolveTriggers(model);
        }
        #endregion

        #region resolve name
        public void ResolveName(MySqlModel model)
        {
            model.Name = new SqlConnectionStringBuilder(_sqlConnectionString).DataSource;
        }
        #endregion

        #region resolve schemas
        public void ResolveSchemas(MySqlModel model)
        {
            DatabaseObjectList<MySqlSchema> schemas = new DatabaseObjectList<MySqlSchema>();

            string sql = GetResourceScopedToSchemas("Schema");

            void hydrate(IDataReader dr)
            {
                while (dr.Read())
                {
                    MySqlSchema schema = new MySqlSchema()
                    {
                        Identifier = (string)dr["SCHEMA_NAME"],
                        Name = (string)dr["SCHEMA_NAME"]
                    };
                    schemas.Add(schema);
                }
            };

            this.ExecuteSql(sql, hydrate);

            model.Schemas = schemas;
        }
        #endregion

        #region resolve tables
        public void ResolveTables(MySqlModel model)
        {
            List<(string schema, MySqlTable table)> tables = new List<(string, MySqlTable)>();

            string sql = GetResourceScopedToSchemas("Table");

            void hydrate(IDataReader dr)
            {
                while (dr.Read())
                {
                    var table = new MySqlTable();
                    table.Identifier = model.ComputeIdentifier((string)dr["TABLE_SCHEMA"], (string)dr["TABLE_NAME"]);
                    table.Name = (string)dr["TABLE_NAME"];
                    tables.Add(((string)dr["TABLE_SCHEMA"], table));
                }
            };

            this.ExecuteSql(sql, hydrate);

            foreach (MySqlSchema schema in model.Schemas)
            {
                schema.Tables = tables.Where(t => t.schema == schema.Identifier).Select(t => t.table).ToDatabaseObjectList();

                foreach (MySqlTable table in schema.Tables)
                    table.SetParent(schema);
            }
        }
        #endregion

        #region resolve table columns
        public void ResolveTableColumns(MySqlModel model)
        {
            List<(string table, MySqlTableColumn column)> columns = new List<(string, MySqlTableColumn)>();

            string sql = GetResourceScopedToSchemas("Table_Column");

            void hydrate(IDataReader dr)
            {
                while (dr.Read())
                {
                    string typeName = (string)dr["DATA_TYPE"];
                    byte? precision = dr["NUMERIC_PRECISION"] == DBNull.Value ? null : Convert.ToByte((ulong?)dr["NUMERIC_PRECISION"]);
                    byte? scale = dr["NUMERIC_SCALE"] == DBNull.Value ? null : Convert.ToByte((ulong?)dr["NUMERIC_SCALE"]);
                    long? maxLength = dr["CHARACTER_MAXIMUM_LENGTH"] == DBNull.Value ? null : (long?)dr["CHARACTER_MAXIMUM_LENGTH"];
                    string columnType = dr["COLUMN_TYPE"] == DBNull.Value ? typeName : (string)dr["COLUMN_TYPE"];
                    long? characterOctetLength = dr["CHARACTER_OCTET_LENGTH"] == DBNull.Value ? null : (long?)dr["CHARACTER_OCTET_LENGTH"];

                    MySqlTypeDescriptor? resolved = MySqlDataTypes.GetTypeDescriptor(typeName, columnType, precision, scale, maxLength, characterOctetLength)
                        ?? throw new DataException($"{typeName} is not a recognized Sql type.");

                    var column = new MySqlTableColumn();
                    column.SqlTypeName = resolved.DbTypeName;
                    column.SqlType = resolved.DbType;
                    column.ColumnType = resolved.ColumnType;
                    column.Precision = resolved.Precision;
                    column.Scale = resolved.Scale;
                    column.MaxLength = resolved.MaxLength;
                    column.CharacterOctetLength = resolved.CharacterOctetLength;

                    column.OrdinalPosition = Convert.ToInt32((uint)dr["ORDINAL_POSITION"]);
                    column.Identifier = model.ComputeIdentifier((string)dr["TABLE_SCHEMA"], (string)dr["TABLE_NAME"], (string)dr["COLUMN_NAME"]);
                    column.Name = (string)dr["COLUMN_NAME"];
                    column.AutoIncrement = (int)dr["AUTO_INCREMENT"] == 1;
                    column.IsNullable = (int)dr["IS_NULLABLE"] == 1;
                    column.GenerationExpression = dr["GENERATION_EXPRESSION"] == DBNull.Value ? null : (string)dr["GENERATION_EXPRESSION"];
                    column.DefaultDefinition = dr["COLUMN_DEFAULT"] == DBNull.Value ? null : (string)dr["COLUMN_DEFAULT"];
 
                    columns.Add((model.ComputeIdentifier((string)dr["TABLE_SCHEMA"], (string)dr["TABLE_NAME"]), column));
                }
            };

            this.ExecuteSql(sql, hydrate);

            foreach (MySqlTable table in model.Schemas.SelectMany(s => s.Tables))
            {
                table.Columns = columns.Where(c => c.table == table.Identifier).Select(c => c.column).ToDatabaseObjectList();

                foreach (MySqlTableColumn column in table.Columns)
                    column.SetParent(table);
            }
        }
        #endregion

        #region resolve table indexes
        public void ResolveTableIndexes(MySqlModel model)
        {
            List<(string table, MySqlIndex index, List<MySqlIndexedColumn> columns)> indexedColumns = new List<(string table, MySqlIndex index, List<MySqlIndexedColumn> columns)>();

            string sql = GetResourceScopedToSchemas("Table_Index");

            void hydrate(IDataReader dr)
            {
                while (dr.Read())
                {
                    var indexName = (string)dr["INDEX_NAME"];
                    var tableIdentifier = model.ComputeIdentifier((string)dr["INDEX_SCHEMA"], (string)dr["TABLE_NAME"]);
                    var indexIdentifier = model.ComputeIdentifier((string)dr["INDEX_SCHEMA"], (string)dr["TABLE_NAME"], (string)dr["INDEX_NAME"]);

                    var existing = indexedColumns.SingleOrDefault(i => i.table == tableIdentifier && i.index.Identifier == indexIdentifier);
                    if (existing == default)
                    {
                        var idx = new MySqlIndex();
                        idx.Name = indexName;
                        idx.Identifier = indexIdentifier;
                        idx.IsPrimaryKey = indexName == "PRIMARY";
                        idx.IndexType = (IndexType)Enum.Parse(typeof(IndexType), (string)dr["INDEX_TYPE"], true);
                        idx.IsUnique = (long)dr["NON_UNIQUE"] == 0;
                        existing = (tableIdentifier, idx, new List<MySqlIndexedColumn>());
                        indexedColumns.Add(existing);
                    }

                    var idxCol = new MySqlIndexedColumn();
                    idxCol.Identifier = model.ComputeIdentifier((string)dr["INDEX_SCHEMA"], (string)dr["TABLE_NAME"], (string)dr["COLUMN_NAME"]);
                    idxCol.Name = (string)dr["COLUMN_NAME"];
                    idxCol.OrdinalPosition = Convert.ToInt16((UInt32)dr["SEQ_IN_INDEX"]);
                    idxCol.IsDescending = (string)dr["COLLATION"] == "D";

                    existing.columns.Add(idxCol);
                }
            };

            this.ExecuteSql(sql, hydrate);

            foreach (MySqlTable table in model.Schemas.SelectMany(s => s.Tables))
            {
                table.Indexes = indexedColumns.Where(c => c.table == table.Identifier).Select(c => c.index).ToDatabaseObjectList();
                foreach (var idx in table.Indexes)
                    idx.SetParent(table);

                foreach (MySqlIndex index in table.Indexes)
                {
                    index.IndexedColumns = indexedColumns.Where(c => c.index.Identifier == index.Identifier).SelectMany(c => c.columns).ToDatabaseObjectList();
                    foreach (var idx in index.IndexedColumns)
                        idx.SetParent(index);
                }
            }
        }
        #endregion

        #region resolve views
        public void ResolveViews(MySqlModel model)
        {
            List<(string schema, MySqlView view)> views = new List<(string, MySqlView)>();

            string sql = GetResourceScopedToSchemas("View");

            void hydrate(IDataReader dr)
            {
                while (dr.Read())
                {
                    var schema = (string)dr["TABLE_SCHEMA"];
                    var view = new MySqlView();
                    view.Identifier = model.ComputeIdentifier(schema, (string)dr["TABLE_NAME"]);
                    view.Name = (string)dr["TABLE_NAME"];
                    views.Add((schema, view));
                }
            };

            this.ExecuteSql(sql, hydrate);

            foreach (MySqlSchema schema in model.Schemas)
            {
                schema.Views = views.Where(v => v.schema == schema.Identifier).Select(t => t.view).ToDatabaseObjectList();

                foreach (MySqlView view in schema.Views)
                    view.SetParent(schema);
            }
        }
        #endregion

        #region resolve view columns
        public void ResolveViewColumns(MySqlModel model)
        {
            List<(string view, MySqlViewColumn column)> columns = new List<(string view, MySqlViewColumn column)>();

            string sql = GetResourceScopedToSchemas("View_Column");

            void hydrate(IDataReader dr)
            {
                while (dr.Read())
                {
                    string typeName = (string)dr["DATA_TYPE"];
                    byte? precision = dr["NUMERIC_PRECISION"] == DBNull.Value ? null : Convert.ToByte((ulong?)dr["NUMERIC_PRECISION"]);
                    byte? scale = dr["NUMERIC_SCALE"] == DBNull.Value ? null : Convert.ToByte((ulong?)dr["NUMERIC_SCALE"]);
                    long? maxLength = dr["CHARACTER_MAXIMUM_LENGTH"] == DBNull.Value ? null : (long?)dr["CHARACTER_MAXIMUM_LENGTH"];
                    string columnType = dr["COLUMN_TYPE"] == DBNull.Value ? typeName : (string)dr["COLUMN_TYPE"];
                    long? characterOctetLength = dr["CHARACTER_OCTET_LENGTH"] == DBNull.Value ? null : (long?)dr["CHARACTER_OCTET_LENGTH"];

                    MySqlTypeDescriptor? resolved = MySqlDataTypes.GetTypeDescriptor(typeName, columnType, precision, scale, maxLength, characterOctetLength)
                        ?? throw new DataException($"{typeName} is not a recognized Sql type.");

                    var column = new MySqlViewColumn();
                    column.SqlTypeName = resolved.DbTypeName;
                    column.SqlType = resolved.DbType;
                    column.ColumnType = resolved.ColumnType;
                    column.Precision = resolved.Precision;
                    column.Scale = resolved.Scale;
                    column.MaxLength = resolved.MaxLength;
                    column.CharacterOctetLength = resolved.CharacterOctetLength;

                    column.OrdinalPosition = Convert.ToInt32((uint)dr["ORDINAL_POSITION"]);
                    column.Identifier = model.ComputeIdentifier((string)dr["TABLE_SCHEMA"], (string)dr["TABLE_NAME"], (string)dr["COLUMN_NAME"]);
                    column.Name = (string)dr["COLUMN_NAME"];
                    column.AutoIncrement = (int)dr["AUTO_INCREMENT"] == 1;
                    column.IsNullable = (int)dr["IS_NULLABLE"] == 1;
                    column.GenerationExpression = dr["GENERATION_EXPRESSION"] == DBNull.Value ? null : (string)dr["GENERATION_EXPRESSION"];
                    column.DefaultDefinition = dr["COLUMN_DEFAULT"] == DBNull.Value ? null : (string)dr["COLUMN_DEFAULT"];

                    columns.Add((model.ComputeIdentifier((string)dr["TABLE_SCHEMA"], (string)dr["TABLE_NAME"]), column));
                }
            };

            this.ExecuteSql(sql, hydrate);

            foreach (MySqlView view in model.Schemas.SelectMany(s => s.Views))
            {
                view.Columns = columns.Where(v => v.view == view.Identifier).Select(t => t.column).ToDatabaseObjectList();

                foreach (MySqlViewColumn column in view.Columns)
                    column.SetParent(view);
            }
        }
        #endregion

        #region resolve sprocs
        public void ResolveProcedures(MySqlModel model)
        {
            List<(string schema, MySqlProcedure procedure)> sprocs = new List<(string schema, MySqlProcedure procedure)>();

            string sql = GetResourceScopedToSchemas("Procedure");

            void hydrate(IDataReader dr)
            {
                while (dr.Read())
                {
                    var procedure = new MySqlProcedure();
                    procedure.Name = (string)dr["SPECIFIC_NAME"];
                    procedure.Identifier = model.ComputeIdentifier((string)dr["ROUTINE_SCHEMA"], procedure.Name);
                    sprocs.Add(((string)dr["ROUTINE_SCHEMA"], procedure));
                }
            };

            this.ExecuteSql(sql, hydrate);

            foreach (MySqlSchema schema in model.Schemas)
            {
                schema.Procedures = sprocs.Where(s => s.schema == schema.Identifier).Select(s => s.procedure).ToDatabaseObjectList();

                foreach (MySqlProcedure procedure in schema.Procedures)
                    procedure.SetParent(schema);
            }
        }
        #endregion

        #region resolve sproc parameters
        public void ResolveProcedureParameters(MySqlModel model)
        {
            List<(string procedure, MySqlParameter parameter)> parameters = new List<(string procedure, MySqlParameter parameter)>();

            string sql = GetResourceScopedToSchemas("Procedure_Parameter");

            void hydrate(IDataReader dr)
            {
                while (dr.Read())
                {
                    string typeName = (string)dr["DATA_TYPE"];
                    //precision and scale are different data type than other queries
                    byte? precision = dr["NUMERIC_PRECISION"] == DBNull.Value ? null : Convert.ToByte((uint)dr["NUMERIC_PRECISION"]);
                    byte? scale = dr["NUMERIC_SCALE"] == DBNull.Value ? null : Convert.ToByte((long)dr["NUMERIC_SCALE"]);
                    long? maxLength = dr["CHARACTER_MAXIMUM_LENGTH"] == DBNull.Value ? null : (long?)dr["CHARACTER_MAXIMUM_LENGTH"];

                    MySqlTypeDescriptor? resolved = MySqlDataTypes.GetTypeDescriptor(typeName, null, precision, scale, maxLength, null)
                        ?? throw new DataException($"{typeName} is not a recognized Sql type.");

                    var parameter = new MySqlParameter();
                    parameter.SqlTypeName = resolved.DbTypeName;
                    parameter.SqlType = resolved.DbType;
                    parameter.Precision = resolved.Precision;
                    parameter.Scale = resolved.Scale;
                    parameter.MaxLength = resolved.MaxLength;

                    parameter.IsOutput = dr["PARAMETER_MODE"] == DBNull.Value && dr["DATA_TYPE"] != DBNull.Value;
 
                    parameters.Add((model.ComputeIdentifier((string)dr["ROUTINE_SCHEMA"], (string)dr["SPECIFIC_NAME"]), parameter));
                }
            };

            this.ExecuteSql(sql, hydrate);

            foreach (MySqlProcedure sproc in model.Schemas.SelectMany(s => s.Procedures))
            {
                sproc.Parameters = parameters.Where(p => p.procedure == sproc.Identifier).Select(p => p.parameter).ToDatabaseObjectList();

                foreach (MySqlParameter parameter in sproc.Parameters)
                    parameter.SetParent(sproc);
            }
        }
        #endregion

        #region resolve relationships
        public void ResolveRelationships(MySqlModel model)
        {
            List<(string table, MySqlRelationship relationship)> relationships = new List<(string table, MySqlRelationship relationship)>();

            string sql = GetResourceScopedToSchemas("Relationship");

            void addOrMerge(string table, MySqlRelationship relationship)
            {
                int idx = relationships.FindIndex(x => x.relationship.Identifier == relationship.Identifier);
                if (idx > -1) //multi column FK..just add the colmn info to the existing
                {
                    MySqlRelationship tmp = relationships[idx].relationship;
                    tmp.ReferenceTableIdentifier = relationship.ReferenceTableIdentifier;
                    tmp.BaseColumnIdentifiers.Add(relationship.BaseColumnIdentifiers[0]);
                    tmp.ReferenceColumnIdentifiers.Add(relationship.ReferenceColumnIdentifiers[0]);
                }
                else
                {
                    relationships.Add((table, relationship));
                }
            };

            void hydrate(IDataReader dr)
            {
                while (dr.Read())
                {
                    var table = model.ComputeIdentifier((string)dr["CONSTRAINT_SCHEMA"], (string)dr["TABLE_NAME"]);
                    var relationship = new MySqlRelationship();
                    relationship.Identifier = model.ComputeIdentifier((string)dr["CONSTRAINT_SCHEMA"], (string)dr["TABLE_NAME"], (string)dr["CONSTRAINT_NAME"]);
                    relationship.Name = (string)dr["CONSTRAINT_NAME"];
                    relationship.BaseColumnIdentifiers = new List<string> { model.ComputeIdentifier((string)dr["CONSTRAINT_SCHEMA"], (string)dr["TABLE_NAME"], (string)dr["COLUMN_NAME"]) };
                    relationship.ReferenceTableIdentifier = model.ComputeIdentifier((string)dr["CONSTRAINT_SCHEMA"], (string)dr["REFERENCED_TABLE_NAME"]);
                    relationship.ReferenceColumnIdentifiers = new List<string> { model.ComputeIdentifier((string)dr["CONSTRAINT_SCHEMA"], (string)dr["REFERENCED_COLUMN_NAME"]) };
                    addOrMerge(table, relationship);
                }
            };

            this.ExecuteSql(sql, hydrate);

            foreach (MySqlTable table in model.Schemas.SelectMany(s => s.Tables))
            {
                table.Relationships = relationships.Where(r => r.table == table.Identifier).Select(r => r.relationship).ToDatabaseObjectList();

                foreach (var relationship in table.Relationships)
                    relationship.SetParent(table);
            }
        }
        #endregion

        #region resolve triggers
        public void ResolveTriggers(MySqlModel model)
        {
            List<(string table, MySqlTrigger trigger)> triggers = new List<(string table, MySqlTrigger trigger)>();

            string sql = GetResourceScopedToSchemas("Trigger");

            void hydrate(IDataReader dr)
            {
                while (dr.Read())
                {
                    var trigger = new MySqlTrigger();
                    trigger.Identifier = model.ComputeIdentifier((string)dr["TRIGGER_SCHEMA"], (string)dr["EVENT_OBJECT_TABLE"], (string)dr["TRIGGER_NAME"]);
                    trigger.Name = (string)dr["TRIGGER_NAME"];
                    trigger.EventTimingType = (TriggerEventTimingType)Enum.Parse(typeof(TriggerEventTimingType), (string)dr["ACTION_TIMING"], true);
                    trigger.EventActionType = (TriggerEventActionType)Enum.Parse(typeof(TriggerEventActionType), (string)dr["EVENT_MANIPULATION"], true);
                    triggers.Add((model.ComputeIdentifier((string)dr["TRIGGER_SCHEMA"], (string)dr["EVENT_OBJECT_TABLE"]), trigger));
                }
            };

            this.ExecuteSql(sql, hydrate);

            foreach (MySqlTable table in model.Schemas.SelectMany(s => s.Tables))
            {
                table.Triggers = triggers.Where(t => t.table == table.Identifier).Select(t => t.trigger).ToDatabaseObjectList();

                foreach (var trigger in table.Triggers)
                    trigger.SetParent(table);
            }
        }

        private string GetResourceScopedToSchemas(string name)
        {
            return GetResource(name, "_sql").Replace("{Schemas}", String.Join(",", _databases));
        }
        #endregion
    }
}
