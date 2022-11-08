using HatTrick.Model.Sql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Microsoft.Data.SqlClient;

namespace HatTrick.Model.MsSql
{
    public class MsSqlModelBuilder : SqlModelBuilder<MsSqlModel>
    {
        #region internals
        private string _sqlConnectionString;
        #endregion

        #region constructors
        public MsSqlModelBuilder(string sqlConnectionString) : base("HatTrick.Model.MsSql.Scripts")
        {
            _sqlConnectionString = sqlConnectionString;
        }
        #endregion

        #region get connection
        protected override DbConnection GetConnection()
        {
            return new SqlConnection(_sqlConnectionString);
        }
        #endregion

        #region build
        protected override void BuildModel(MsSqlModel model)
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
            this.ResolveTableExtendedProperties(model);
            this.ResolveTableColumnExtendedProperties(model);
            this.ResolveViewExtendedProperties(model);
            this.ResolveViewColumnExtendedProperties(model);
        }
        #endregion

        #region resolve name
        public void ResolveName(MsSqlModel model)
        {
            model.Name = new SqlConnectionStringBuilder(_sqlConnectionString).InitialCatalog;
        }
        #endregion

        #region resolve schemas
        public void ResolveSchemas(MsSqlModel model)
        {
            DatabaseObjectList<MsSqlSchema> schemas = new();

            string sql = GetResource("Schema");

            void hydrate(IDataReader dr)
            {
                while (dr.Read())
                {
                    MsSqlSchema s = new MsSqlSchema()
                    {
                        Identifier = dr["schema_id"].ToString(),
                        Name = (string)dr["name"]
                    };
                    schemas.Add(s);
                }
            };

            this.ExecuteSql(sql, hydrate);

            model.Schemas = schemas;
        }
        #endregion

        #region resolve tables
        public void ResolveTables(MsSqlModel model)
        {
            List<(string schema, MsSqlTable table)> tables = new List<(string schema, MsSqlTable table)>();

            string sql = GetResource("Table");

            void hydrate(IDataReader dr)
            {
                while (dr.Read())
                {
                    MsSqlTable table = new MsSqlTable()
                    {
                        Identifier = dr["object_id"].ToString(),
                        Name = (string)dr["table_name"]
                    };
                    tables.Add((dr["schema_id"].ToString(), table));
                }
            };

            this.ExecuteSql(sql, hydrate);

            foreach (MsSqlSchema schema in model.Schemas)
            {
                schema.Tables = tables.Where(t => t.schema == schema.Identifier).Select(t => t.table).ToDatabaseObjectList();

                foreach (var table in schema.Tables)
                    table.SetParent(schema);
            }
        }
        #endregion

        #region resolve table columns
        public void ResolveTableColumns(MsSqlModel model)
        {
            List<(string table, MsSqlTableColumn column)> columns = new List<(string table, MsSqlTableColumn column)>();

            string sql = GetResource("Table_Column");

            void hydrate(IDataReader dr)
            {
                while (dr.Read())
                {
                    string typeName = (string)dr["data_type_name"];

                    MsSqlTypeDescriptor? resolved = MsSqlDataTypes.GetTypeDescriptor(typeName) ?? throw new DataException($"{typeName} is not a recognized Sql type.");

                    var column = new MsSqlTableColumn();
                    column.SqlTypeName = resolved.DbTypeName;
                    column.SqlType = resolved.DbType;
                    column.Precision = dr["precision"] == DBNull.Value ? null : (byte?)dr["precision"];
                    column.Scale = dr["scale"] == DBNull.Value ? null : (byte?)dr["scale"];
                    column.MaxLength = dr["max_length"] == DBNull.Value ? null : Convert.ToInt64((short?)dr["max_length"]);
                    column.Identifier = dr["column_id"].ToString();
                    column.Name = (string)dr["column_name"];
                    column.IsIdentity = (bool)dr["is_identity"];
                    column.OrdinalPosition = (int)dr["column_id"];
                    column.IsNullable = (bool)dr["is_nullable"];
                    column.IsComputed = (bool)dr["is_computed"];
                    column.DefaultDefinition = dr["definition"] == DBNull.Value ? null : (string)dr["definition"];
                    columns.Add((dr["table_id"].ToString(), column));
                }
            };

            this.ExecuteSql(sql, hydrate);

            foreach (MsSqlTable table in model.Schemas.SelectMany(s => s.Tables))
            {
                table.Columns = columns.Where(c => c.table == table.Identifier).Select(c => c.column).ToDatabaseObjectList();

                foreach (var column in table.Columns)
                    column.SetParent(table);
            }
        }
        #endregion

        #region resolve table indexes
        public void ResolveTableIndexes(MsSqlModel model)
        {
            List<(string table, MsSqlIndex index, List<MsSqlIndexedColumn> columns)> indexedColumns = new List<(string table, MsSqlIndex index, List<MsSqlIndexedColumn> columns)>();

            string sql = GetResource("Table_Index");

            void hydrate(IDataReader dr)
            {
                while (dr.Read())
                {
                    var indexName = (string)dr["index_name"];
                    //MsSql reports a different table id for indexes that are primary keys vs other indexes.  Must use a munge using table_name instead of table_id.
                    var indexIdentifier = model.ComputeIdentifier(dr["schema_id"].ToString(), dr["table_name"].ToString(), dr["index_id"].ToString());

                    var existing = indexedColumns.SingleOrDefault(i => i.index.Identifier == indexIdentifier);
                    if (existing == default)
                    {
                        var idx = new MsSqlIndex()
                        {
                            Name = indexName,
                            Identifier = indexIdentifier,
                            IsPrimaryKey = (bool)dr["is_primary_key"],
                            IndexType = (IndexType)(byte)dr["index_type_code"],
                            IsUnique = (bool)dr["is_unique"]
                        };
                        existing = (dr["table_id"].ToString(), idx, new List<MsSqlIndexedColumn>());
                        indexedColumns.Add(existing);
                    }

                    var idxCol = new MsSqlIndexedColumn
                    {
                        Identifier = model.ComputeIdentifier(dr["schema_id"].ToString(), dr["table_name"].ToString(), dr["index_id"].ToString(), dr["column_id"].ToString()),
                        Name = (string)dr["column_name"],
                        OrdinalPosition = (byte)dr["key_ordinal"],
                        IsDescending = (bool)dr["is_descending_key"],
                        IsIncludedColumn = (bool)dr["is_included_column"]
                    };
                    existing.columns.Add(idxCol);
                }
            };

            this.ExecuteSql(sql, hydrate);

            foreach (MsSqlTable table in model.Schemas.SelectMany(s => s.Tables))
            {
                table.Indexes = indexedColumns.Where(c => c.table == table.Identifier).Select(c => c.index).ToDatabaseObjectList();
                foreach (var idx in table.Indexes)
                    idx.SetParent(table);

                foreach (MsSqlIndex index in table.Indexes)
                {
                    index.IndexedColumns = indexedColumns.Where(c => c.index.Identifier == index.Identifier).SelectMany(c => c.columns).ToDatabaseObjectList();
                    foreach (var idx in index.IndexedColumns)
                        idx.SetParent(index);
                }
            }
        }
        #endregion

        #region resolve views
        public void ResolveViews(MsSqlModel model)
        {
            List<(string schema, MsSqlView view)> views = new List<(string schema, MsSqlView view)>();

            string sql = GetResource("View");

            void hydrate(IDataReader dr)
            {
                while (dr.Read())
                {
                    var view = new MsSqlView()
                    {
                        Identifier = dr["object_id"].ToString(),
                        Name = (string)dr["view_name"]
                    };
                    views.Add((dr["schema_id"].ToString(), view));
                }
            };

            this.ExecuteSql(sql, hydrate);

            foreach (MsSqlSchema schema in model.Schemas)
            {
                schema.Views = views.Where(v => v.schema == schema.Identifier).Select(v => v.view).ToDatabaseObjectList();

                foreach (var view in schema.Views)
                    view.SetParent(schema);
            }
        }
        #endregion

        #region resolve view columns
        public void ResolveViewColumns(MsSqlModel model)
        {
            List<(string view, MsSqlViewColumn column)> columns = new List<(string view, MsSqlViewColumn column)>();

            string sql = GetResource("View_Column");

            void hydrate(IDataReader dr)
            {
                while (dr.Read())
                {
                    string typeName = (string)dr["data_type_name"];

                    MsSqlTypeDescriptor? resolved = MsSqlDataTypes.GetTypeDescriptor(typeName) ?? throw new DataException($"{typeName} is not a recognized Sql type.");

                    var column = new MsSqlViewColumn();
                    column.SqlTypeName = resolved.DbTypeName;
                    column.SqlType = resolved.DbType;
                    column.Precision = dr["precision"] == DBNull.Value ? null : (byte?)dr["precision"];
                    column.Scale = dr["scale"] == DBNull.Value ? null : (byte?)dr["scale"];
                    column.MaxLength = dr["max_length"] == DBNull.Value ? null : Convert.ToInt64((short?)dr["max_length"]);

                    column.Identifier = dr["column_id"].ToString();
                    column.Name = (string)dr["column_name"];
                    column.IsIdentity = (bool)dr["is_identity"];
                    column.OrdinalPosition = (int)dr["column_id"];
                    column.IsNullable = (bool)dr["is_nullable"];
                    column.IsComputed = (bool)dr["is_computed"];

                    columns.Add((dr["view_id"].ToString(), column));
                }
            };

            this.ExecuteSql(sql, hydrate);

            foreach (MsSqlView view in model.Schemas.SelectMany(s => s.Views))
            {
                view.Columns = columns.Where(c => c.view == view.Identifier).Select(c => c.column).ToDatabaseObjectList();

                foreach (var column in view.Columns)
                    column.SetParent(view);
            }
        }
        #endregion

        #region resolve sprocs
        public void ResolveProcedures(MsSqlModel model)
        {
            List<(string schema, MsSqlProcedure procedure)> sprocs = new List<(string schema, MsSqlProcedure procedure)>();

            string sql = GetResource("Procedure");

            void hydrate(IDataReader dr)
            {
                while (dr.Read())
                {
                    var procedure = new MsSqlProcedure()
                    {
                        Identifier = dr["object_id"].ToString(),
                        Name = (string)dr["sproc_name"],
                        IsStartupProcedure = (bool)dr["is_startup_sproc"],
                    };
                    sprocs.Add((dr["schema_id"].ToString(), procedure));
                }
            };

            this.ExecuteSql(sql, hydrate);

            foreach (MsSqlSchema schema in model.Schemas)
            {
                schema.Procedures = sprocs.Where(s => s.schema == schema.Identifier).Select(s => s.procedure).ToDatabaseObjectList();

                foreach (var procedure in schema.Procedures)
                    procedure.SetParent(schema);
            }
        }
        #endregion

        #region resolve sproc parameters
        public void ResolveProcedureParameters(MsSqlModel model)
        {
            List<(string procedure, MsSqlParameter parameter)> parameters = new List<(string procedure, MsSqlParameter parameter)>();

            string sql = GetResource("Procedure_Parameter");

            void hydrate(IDataReader dr)
            {
                while (dr.Read())
                {
                    string typeName = (string)dr["data_type_name"];

                    MsSqlTypeDescriptor? resolved = MsSqlDataTypes.GetTypeDescriptor(typeName) ?? throw new DataException($"{typeName} is not a recognized Sql type.");

                    var parameter = new MsSqlParameter();
                    parameter.SqlTypeName = resolved.DbTypeName;
                    parameter.SqlType = resolved.DbType;
                    parameter.Precision = dr["precision"] == DBNull.Value ? null : (byte?)dr["precision"];
                    parameter.Scale = dr["scale"] == DBNull.Value ? null : (byte?)dr["scale"];
                    parameter.MaxLength = dr["max_length"] == DBNull.Value ? null : Convert.ToInt64((short?)dr["max_length"]);

                    parameter.Identifier = dr["parameter_id"].ToString();
                    parameter.Name = (string)dr["parameter_name"];
                    parameter.IsOutput = (bool)dr["is_output"];
                    parameter.IsReadOnly = (bool)dr["is_readonly"];
                    parameter.HasDefaultValue = (bool)dr["has_default_value"];
                    parameter.IsNullable = (bool)dr["is_nullable"];
                    parameter.DefaultValue = dr["default_value"] == DBNull.Value ? null : (object)dr["default_value"];//only valid on clr procedures
                    parameters.Add((dr["sproc_id"].ToString(), parameter));
                }
            };

            this.ExecuteSql(sql, hydrate);

            foreach (MsSqlProcedure sproc in model.Schemas.SelectMany(s => s.Procedures))
            {
                sproc.Parameters = parameters.Where(p => p.procedure == sproc.Identifier).Select(s => s.parameter).ToDatabaseObjectList();

                foreach (MsSqlParameter parameter in sproc.Parameters)
                    parameter.SetParent(sproc);
            }
        }
        #endregion

        #region resolve relationships
        public void ResolveRelationships(MsSqlModel model)
        {
            List<(string table, MsSqlRelationship relationship)> relationships = new List<(string table, MsSqlRelationship relationship)>();

            string sql = GetResource("Relationships");

            void addOrMerge(string table, MsSqlRelationship relationship)
            {
                int idx = relationships.FindIndex(x => x.relationship.Identifier == relationship.Identifier);
                if (idx > -1) //multi column FK..just add the colmn info to the existing
                {
                    MsSqlRelationship tmp = relationships[idx].relationship;
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
                    var table = dr["base_table_id"].ToString();
                    var relationship = new MsSqlRelationship
                    {
                        Name = (string)dr["relationship_name"],
                        Identifier = (string)dr["relationship_name"],
                        BaseColumnIdentifiers = new List<string> { dr["base_column_id"].ToString() },
                        ReferenceTableIdentifier = dr["referenced_table_id"].ToString(),
                        ReferenceColumnIdentifiers = new List<string> { dr["referenced_column_id"].ToString() }
                    };
                    addOrMerge(table, relationship);
                }
            };

            this.ExecuteSql(sql, hydrate);

            foreach (MsSqlTable table in model.Schemas.SelectMany(s => s.Tables))
            {
                table.Relationships = relationships.Where(r => r.table == table.Identifier).Select(r => r.relationship).ToDatabaseObjectList();

                foreach (var relationship in table.Relationships)
                    relationship.SetParent(table);
            }
        }
        #endregion

        #region resolve triggers
        public void ResolveTriggers(MsSqlModel model)
        {
            List<(string table, MsSqlTrigger trigger)> triggers = new List<(string table, MsSqlTrigger trigger)>();

            string sql = GetResource("Trigger");

            void hydrate(IDataReader dr)
            {
                while (dr.Read())
                {
                    var table = dr["table_object_id"].ToString();
                    var trigger = new MsSqlTrigger
                    {
                        Identifier = dr["object_id"].ToString(),
                        Name = (string)dr["trigger_name"],
                        EventType = (TriggerEventType)Enum.Parse(typeof(TriggerEventType), (string)dr["type_desc"], true),
                        IsDisabled = (bool)dr["is_disabled"],
                        IsInsteadOfTrigger = (bool)dr["is_instead_of_trigger"]
                    };
                    triggers.Add((table, trigger));
                }
            };

            this.ExecuteSql(sql, hydrate);

            foreach (MsSqlTable table in model.Schemas.SelectMany(s => s.Tables))
            {
                table.Triggers = triggers.Where(t => t.table == table.Identifier).Select(t => t.trigger).ToDatabaseObjectList();

                foreach (var trigger in table.Triggers)
                    trigger.SetParent(table);
            }
        }
		#endregion

		#region resolve table ext props
		public void ResolveTableExtendedProperties(MsSqlModel model)
        {
            List<(string table, MsSqlExtendedProperty extProp)> extProps = new List<(string table, MsSqlExtendedProperty extProp)>();

            string sql = GetResource("Table_Ext_Props");

            void hydrate(IDataReader dr)
            {
                while (dr.Read())
                {
                    var table = dr["table_id"].ToString();
                    var p = new MsSqlExtendedProperty
                    {
                        MinorIdentifier = null,
                        Name = dr["name"].ToString(),
                        Value = dr["value"].ToString()
                    };
                    extProps.Add((table, p));
                }
            };

            this.ExecuteSql(sql, hydrate);

            foreach (MsSqlTable table in model.Schemas.SelectMany(s => s.Tables))
            {
                table.ExtendedProperties = extProps.Where(e => e.table == table.Identifier).Select(e => e.extProp).ToDatabaseObjectList();

                foreach (MsSqlExtendedProperty prop in table.ExtendedProperties)
                    prop.SetParent(table);
            }
        }
        #endregion

        #region resolve table column ext props
        public void ResolveTableColumnExtendedProperties(MsSqlModel model)
        {
            List<(string table, string column, MsSqlExtendedProperty extProp)> extProps = new List<(string table, string column, MsSqlExtendedProperty extProp)>();

            string sql = GetResource("Table_Column_Ext_Props");

            void hydrate(IDataReader dr)
            {
                while (dr.Read())
                {
                    var table = dr["table_id"].ToString();
                    var column = dr["column_id"].ToString();
                    var p = new MsSqlExtendedProperty
                    {
                        MinorIdentifier = dr["column_id"].ToString(),
                        Name = dr["name"].ToString(),
                        Value = dr["value"].ToString()
                    };
                    extProps.Add((table, column, p));
                }
            };

            this.ExecuteSql(sql, hydrate);

            foreach (MsSqlTableColumn column in model.Schemas.SelectMany(s => s.Tables).SelectMany(t => t.Columns))
            {
                column.ExtendedProperties = extProps.Where(e => e.table == column.ParentIdentifier && e.column == column.Identifier).Select(e => e.extProp).ToDatabaseObjectList();

                foreach (MsSqlExtendedProperty prop in column.ExtendedProperties)
                    prop.SetParent(column);
            }
        }
        #endregion

        #region resolve view ext props
        public void ResolveViewExtendedProperties(MsSqlModel model)
        {
            List<(string view, MsSqlExtendedProperty extProp)> extProps = new List<(string view, MsSqlExtendedProperty extProp)>();

            string sql = GetResource("View_Ext_Props");

            void hydrate(IDataReader dr)
            {
                while (dr.Read())
                {
                    var view = dr["view_id"].ToString();
                    var p = new MsSqlExtendedProperty
                    {
                        MinorIdentifier = null,
                        Name = dr["name"].ToString(),
                        Value = dr["value"].ToString()
                    };
                    extProps.Add((view, p));
                }
            };

            this.ExecuteSql(sql, hydrate);

            foreach (MsSqlView view in model.Schemas.SelectMany(s => s.Views))
            {
                view.ExtendedProperties = extProps.Where(e => e.view == view.Identifier).Select(e => e.extProp).ToDatabaseObjectList();

                foreach (MsSqlExtendedProperty prop in view.ExtendedProperties)
                    prop.SetParent(view);
            }
        }
        #endregion

        #region resolve view column ext props
        public void ResolveViewColumnExtendedProperties(MsSqlModel model)
        {
            List<(string view, string column, MsSqlExtendedProperty extProp)> extProps = new List<(string view, string column, MsSqlExtendedProperty extProp)>();

            string sql = GetResource("View_Column_Ext_Props");

            void hydrate(IDataReader dr)
            {
                while (dr.Read())
                {
                    var view = dr["view_id"].ToString();
                    var column = dr["column_id"].ToString();
                    var p = new MsSqlExtendedProperty
                    {
                        MinorIdentifier = dr["column_id"].ToString(),
                        Name = dr["name"].ToString(),
                        Value = dr["value"].ToString()
                    };
                    extProps.Add((view, column, p));
                }
            };

            this.ExecuteSql(sql, hydrate);

            foreach (MsSqlViewColumn column in model.Schemas.SelectMany(s => s.Views).SelectMany(t => t.Columns))
            {
                column.ExtendedProperties = extProps.Where(e => e.view == column.ParentIdentifier && e.column == column.Identifier).Select(e => e.extProp).ToDatabaseObjectList();

                foreach (MsSqlExtendedProperty prop in column.ExtendedProperties)
                    prop.SetParent(column);
            }
        }
        #endregion
    }
}
