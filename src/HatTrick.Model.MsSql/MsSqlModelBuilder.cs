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
            var x = new DbConnectionStringBuilder();
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
        protected override void BuildModel(ref MsSqlModel model)
        {
            this.ResolveName(ref model);
            this.ResolveSchemas(ref model);
            this.ResolveTables(ref model);
            this.ResolveTableColumns(ref model);
            this.ResolveTableIndexes(ref model);
            this.ResolveViews(ref model);
            this.ResolveViewColumns(ref model);
            this.ResolveProcedures(ref model);
            this.ResolveProcedureParameters(ref model);
            this.ResolveRelationships(ref model);
            this.ResolveTriggers(ref model);
            this.ResolveTableExtendedProperties(ref model);
            this.ResolveTableColumnExtendedProperties(ref model);
            this.ResolveViewExtendedProperties(ref model);
            this.ResolveViewColumnExtendedProperties(ref model);
        }
        #endregion

        #region resolve name
        public void ResolveName(ref MsSqlModel model)
        {
            model.Name = new SqlConnectionStringBuilder(_sqlConnectionString).InitialCatalog;
        }
        #endregion

        #region resolve schemas
        public void ResolveSchemas(ref MsSqlModel model)
        {
            Dictionary<string, MsSqlSchema> schemas = new Dictionary<string, MsSqlSchema>(StringComparer.OrdinalIgnoreCase);

            string sql = GetResource("Schema");

            Action<IDataReader> action = (dr) =>
            {
                while (dr.Read())
                {
                    MsSqlSchema s = new MsSqlSchema()
                    {
                        SchemaId = (int)dr["schema_id"],
                        Name = (string)dr["name"]
                    };
                    schemas.Add(s);
                }
            };

            this.ExecuteSql(sql, action);

            model.Schemas = schemas;
        }
        #endregion

        #region resolve tables
        public void ResolveTables(ref MsSqlModel model)
        {
            List<(string, MsSqlTable)> tables = new List<(string, MsSqlTable)>();

            string sql = GetResource("Table");

            Action<IDataReader> action = (dr) =>
            {
                string s = null;
                MsSqlTable t = null;
                while (dr.Read())
                {
                    s = (string)dr["schema_name"];
                    t = new MsSqlTable()
                    {
                        ObjectId = (int)dr["object_id"],
                        Name = (string)dr["table_name"]
                    };
                    tables.Add((s, t));
                }
            };

            this.ExecuteSql(sql, action);

            foreach (MsSqlSchema schema in model.Schemas.Values)
            {
                schema.Tables = new Dictionary<string, MsSqlTable>(StringComparer.OrdinalIgnoreCase)
                    .AddRange(tables.FindAll(t => t.Item1 == schema.Name).ConvertAll(t => t.Item2));

                foreach (var table in schema.Tables.Values)
                {
                    table.SetParent(schema);
                }
            }
        }
        #endregion

        #region resolve table columns
        public void ResolveTableColumns(ref MsSqlModel model)
        {
            List<MsSqlTableColumn> columns = new List<MsSqlTableColumn>();

            string sql = GetResource("Table_Column");

            Action<IDataReader> action = (dr) =>
            {
                SqlDbType sqlType;
                MsSqlTableColumn c = null;
                while (dr.Read())
                {
                    string typeName = (string)dr["data_type_name"];

                    //need to swap out numeric for decimal.. SqlDbType enum does NOT have 'numeric'
                    if (string.Compare(typeName, "numeric", true) == 0)
                        typeName = "decimal";

                    bool knownType = Enum.TryParse<SqlDbType>(typeName, true, out sqlType);
                    c = new MsSqlTableColumn
                    {
                        ColumnId = (int)dr["column_id"],
                        ParentObjectId = (int)dr["table_id"],
                        Name = (string)dr["column_name"],
                        IsIdentity = (bool)dr["is_identity"],
                        SqlTypeName = typeName,
                        SqlType = knownType ? sqlType : SqlDbType.Udt,
                        IsNullable = (bool)dr["is_nullable"],
                        MaxLength = (short)dr["max_length"],
                        Precision = (byte)dr["precision"],
                        Scale = (byte)dr["scale"],
                        IsComputed = (bool)dr["is_computed"],
                        DefaultDefinition = dr["definition"] == DBNull.Value ? null : (string)dr["definition"]
                    };
                    columns.Add(c);
                }
            };

            this.ExecuteSql(sql, action);

            foreach (MsSqlSchema schema in model.Schemas.Values)
            {
                foreach (MsSqlTable table in schema.Tables.Values)
                {
                    table.Columns = new Dictionary<string, MsSqlTableColumn>(StringComparer.OrdinalIgnoreCase)
                        .AddRange(columns.FindAll(c => c.ParentObjectId == table.ObjectId));

                    foreach (var column in table.Columns.Values)
                    {
                        column.SetParent(table);
                    }
                }
            }
        }
        #endregion

        #region resolve table indexes
        public void ResolveTableIndexes(ref MsSqlModel model)
        {
            List<MsSqlIndex> indexes = new List<MsSqlIndex>();
            List<MsSqlIndexedColumn> indexedColumns = new List<MsSqlIndexedColumn>();

            string sql = GetResource("Table_Index");

            Action<IDataReader> action = (dr) =>
            {
                string indexName = null;
                MsSqlIndex index = null;
                MsSqlIndexedColumn idxCol = null;
                while (dr.Read())
                {
                    indexName = (string)dr["index_name"];
                    if (!indexes.Exists(i => i.Name == indexName))
                    {
                        index = new MsSqlIndex()
                        {
                            ParentObjectId = (int)dr["table_id"],
                            Name = indexName,
                            IndexId = (int)dr["index_id"],
                            IsPrimaryKey = (bool)dr["is_primary_key"],
                            IndexType = (IndexType)(byte)dr["index_type_code"],
                            IsUnique = (bool)dr["is_unique"]
                        };
                        indexes.Add(index);
                    }

                    idxCol = new MsSqlIndexedColumn
                    {
                        ParentObjectId = (int)dr["table_id"],
                        IndexId = (int)dr["index_id"],
                        ColumnId = (int)dr["column_id"],
                        Name = (string)dr["column_name"],
                        KeyOrdinal = (byte)dr["key_ordinal"],
                        IsDescendingKey = (bool)dr["is_descending_key"],
                        IsIncludedColumn = (bool)dr["is_included_column"]
                    };
                    indexedColumns.Add(idxCol);
                }
            };

            this.ExecuteSql(sql, action);

            foreach (MsSqlSchema schema in model.Schemas.Values)
            {
                foreach (MsSqlTable table in schema.Tables.Values)
                {
                    table.Indexes = new Dictionary<string, MsSqlIndex>(StringComparer.OrdinalIgnoreCase)
                        .AddRange(indexes.FindAll(i => i.ParentObjectId == table.ObjectId));

                    foreach (MsSqlIndex index in table.Indexes.Values)
                    {
                        index.IndexedColumns = indexedColumns.FindAll(ic => ic.ParentObjectId == index.ParentObjectId && ic.IndexId == index.IndexId).ToArray();
                    }
                }
            }
        }
        #endregion

        #region resolve views
        public void ResolveViews(ref MsSqlModel model)
        {
            List<(string, MsSqlView)> views = new List<(string, MsSqlView)>();

            string sql = GetResource("View");

            Action<IDataReader> action = (dr) =>
            {
                string s = null;
                MsSqlView v = null;
                while (dr.Read())
                {
                    s = (string)dr["schema_name"];
                    v = new MsSqlView()
                    {
                        ObjectId = (int)dr["object_id"],
                        Name = (string)dr["view_name"]
                    };
                    views.Add((s, v));
                }
            };

            this.ExecuteSql(sql, action);

            foreach (MsSqlSchema schema in model.Schemas.Values)
            {
                schema.Views = new Dictionary<string, MsSqlView>(StringComparer.OrdinalIgnoreCase)
                    .AddRange(views.FindAll(v => v.Item1 == schema.Name).ConvertAll(v => v.Item2));

                foreach (var view in schema.Views.Values)
                {
                    view.SetParent(schema);
                }
            }
        }
        #endregion

        #region resolve view columns
        public void ResolveViewColumns(ref MsSqlModel model)
        {
            List<MsSqlViewColumn> columns = new List<MsSqlViewColumn>();

            string sql = GetResource("View_Column");

            Action<IDataReader> action = (dr) =>
            {
                SqlDbType sqlType;
                MsSqlViewColumn c = null;
                while (dr.Read())
                {
                    string typeName = (string)dr["data_type_name"];

                    //need to swap out numeric for decimal.. SqlDbType enum does NOT have 'numeric'
                    if (string.Compare(typeName, "numeric", true) == 0)
                        typeName = "decimal";

                    bool knownType = Enum.TryParse<SqlDbType>(typeName, true, out sqlType);
                    c = new MsSqlViewColumn
                    {
                        ColumnId = (int)dr["column_id"],
                        ParentObjectId = (int)dr["view_id"],
                        Name = (string)dr["column_name"],
                        IsIdentity = (bool)dr["is_identity"],
                        SqlTypeName = (string)dr["data_type_name"],
                        SqlType = knownType ? sqlType : SqlDbType.Udt,
                        IsNullable = (bool)dr["is_nullable"],
                        IsComputed = (bool)dr["is_computed"],
                        MaxLength = (short)dr["max_length"],
                        Precision = (byte)dr["precision"],
                        Scale = (byte)dr["scale"]
                    };
                    columns.Add(c);
                }
            };

            this.ExecuteSql(sql, action);

            foreach (MsSqlSchema schema in model.Schemas.Values)
            {
                foreach (MsSqlView view in schema.Views.Values)
                {
                    view.Columns = new Dictionary<string, MsSqlViewColumn>(StringComparer.OrdinalIgnoreCase)
                        .AddRange(columns.FindAll(c => c.ParentObjectId == view.ObjectId));

                    foreach (var column in view.Columns.Values)
                    {
                        column.SetParent(view);
                    }
                }
            }
        }
        #endregion

        #region resolve sprocs
        public void ResolveProcedures(ref MsSqlModel model)
        {
            List<(string, MsSqlProcedure)> sprocs = new List<(string, MsSqlProcedure)>();

            string sql = GetResource("Procedure");

            Action<IDataReader> action = (dr) =>
            {
                string s = null;
                MsSqlProcedure p = null;
                while (dr.Read())
                {
                    s = (string)dr["schema_name"];
                    p = new MsSqlProcedure()
                    {
                        ObjectId = (int)dr["object_id"],
                        Name = (string)dr["sproc_name"],
                        IsStartupProcedure = (bool)dr["is_startup_sproc"],
                    };
                    sprocs.Add((s, p));
                }
            };

            this.ExecuteSql(sql, action);

            foreach (MsSqlSchema schema in model.Schemas.Values)
            {
                schema.Procedures = new Dictionary<string, MsSqlProcedure>(StringComparer.OrdinalIgnoreCase)
                    .AddRange(sprocs.FindAll(p => p.Item1 == schema.Name).ConvertAll(p => p.Item2).ToList());

                foreach (var procedure in schema.Procedures.Values)
                {
                    procedure.SetParent(schema);
                }
            }
        }
        #endregion

        #region resolve sproc parameters
        public void ResolveProcedureParameters(ref MsSqlModel model)
        {
            List<MsSqlParameter> parameters = new List<MsSqlParameter>();

            string sql = GetResource("Procedure_Parameter");

            Action<IDataReader> action = (dr) =>
            {
                SqlDbType sqlType;
                MsSqlParameter p = null;
                while (dr.Read())
                {
                    string typeName = (string)dr["data_type_name"];

                    //need to swap out numeric for decimal.. SqlDbType enum does NOT have 'numeric'
                    if (string.Compare(typeName, "numeric", true) == 0)
                        typeName = "decimal";

                    bool knownType = Enum.TryParse<SqlDbType>(typeName, true, out sqlType);
                    p = new MsSqlParameter
                    {
                        ParentObjectId = (int)dr["sproc_id"],
                        ParameterId = (int)dr["parameter_id"],
                        Name = (string)dr["parameter_name"],
                        SqlTypeName = (string)dr["data_type_name"],
                        SqlType = knownType ? sqlType : SqlDbType.Udt,
                        IsOutput = (bool)dr["is_output"],
                        IsReadOnly = (bool)dr["is_readonly"],
                        HasDefaultValue = (bool)dr["has_default_value"],
                        IsNullable = (bool)dr["is_nullable"],
                        DefaultValue = dr["default_value"] == DBNull.Value ? null : (object)dr["default_value"],//only valid on clr procedures
                        Precision = (byte)dr["precision"],
                        Scale = (byte)dr["scale"],
                        MaxLength = (short)dr["max_length"]
                    };
                    parameters.Add(p);
                }
            };

            this.ExecuteSql(sql, action);

            foreach (MsSqlSchema schema in model.Schemas.Values)
            {
                foreach (MsSqlProcedure sproc in schema.Procedures.Values)
                {
                    sproc.Parameters = new Dictionary<string, MsSqlParameter>(StringComparer.OrdinalIgnoreCase)
                        .AddRange(parameters.FindAll(p => p.ParentObjectId == sproc.ObjectId));
                }
            }
        }
        #endregion

        #region resolve relationships
        public void ResolveRelationships(ref MsSqlModel model)
        {
            List<(string, MsSqlRelationship)> relationships = new List<(string, MsSqlRelationship)>();

            string sql = GetResource("Relationships");

            Action<string, MsSqlRelationship> AddOrMerge = (s, r) =>
            {
                int idx = relationships.FindIndex(x => x.Item2.Name == r.Name);
                if (idx > -1) //multi column FK..just add the colmn info to the existing
                {
                    MsSqlRelationship tmp = relationships[idx].Item2;
                    tmp.BaseColumnIds.Add(r.BaseColumnIds[0]);
                    tmp.BaseColumnNames.Add(r.BaseColumnNames[0]);
                    tmp.ReferenceColumnIds.Add(r.ReferenceColumnIds[0]);
                    tmp.ReferenceColumnNames.Add(r.ReferenceColumnNames[0]);
                }
                else
                {
                    relationships.Add((s, r));
                }
            };

            Action<IDataReader> action = (dr) =>
            {
                string s = null;
                MsSqlRelationship r = null;
                while (dr.Read())
                {
                    s = (string)dr["schema_name"];
                    r = new MsSqlRelationship()
                    {
                        Name = (string)dr["relationship_name"],
                        BaseTableId = (int)dr["base_table_id"],
                        BaseTableName = (string)dr["base_table_name"],
                        BaseColumnIds = new List<int> { (int)dr["base_column_id"] },
                        BaseColumnNames = new List<string> { (string)dr["base_column_name"] },
                        ReferenceTableId = (int)dr["referenced_table_id"],
                        ReferenceTableName = (string)dr["referenced_table_name"],
                        ReferenceColumnIds = new List<int> { (int)dr["referenced_column_id"] },
                        ReferenceColumnNames = new List<string> { (string)dr["referenced_column_name"] }
                    };
                    AddOrMerge(s, r);
                }
            };

            this.ExecuteSql(sql, action);

            foreach (MsSqlSchema schema in model.Schemas.Values)
            {
                schema.Relationships = new Dictionary<string, MsSqlRelationship>(StringComparer.OrdinalIgnoreCase)
                    .AddRange(relationships.FindAll(p => p.Item1 == schema.Name).ConvertAll(p => p.Item2));

                foreach (var relationship in schema.Relationships.Values)
                {
                    relationship.SetParent(schema);
                }
            }
        }
        #endregion

        #region resolve triggers
        public void ResolveTriggers(ref MsSqlModel model)
        {
            List<MsSqlTrigger> triggers = new List<MsSqlTrigger>();

            string sql = GetResource("Trigger");

            Action<IDataReader> action = (dr) =>
            {
                string s = null;
                MsSqlTrigger t = null;
                while (dr.Read())
                {
                    s = (string)dr["schema_name"];
                    t = new MsSqlTrigger()
                    {
                        ParentObjectId = (int)dr["table_object_id"],
                        ObjectId = (int)dr["object_id"],
                        Name = (string)dr["trigger_name"],
                        EventType = (string)dr["type_desc"],
                        IsDisabled = (bool)dr["is_disabled"],
                        IsInsteadOfTrigger = (bool)dr["is_instead_of_trigger"]
                    };
                    triggers.Add(t);
                }
            };

            this.ExecuteSql(sql, action);

            foreach (MsSqlSchema schema in model.Schemas.Values)
            {
                foreach (MsSqlTable table in schema.Tables.Values)
                {
                    table.Triggers = new Dictionary<string, MsSqlTrigger>(StringComparer.OrdinalIgnoreCase)
                        .AddRange(triggers.FindAll(t => t.ParentObjectId == table.ObjectId));

                    foreach (var trigger in table.Triggers.Values)
                    {
                        trigger.SetParent(table);
                    }
                }
            }
        }
		#endregion

		#region resolve table ext props
		public void ResolveTableExtendedProperties(ref MsSqlModel model)
        {
            List<MsSqlExtendedProperty> extProps = new List<MsSqlExtendedProperty>();

            string sql = GetResource("Table_Ext_Props");

            Action<IDataReader> action = (dr) =>
            {
                MsSqlExtendedProperty p = null;
                while (dr.Read())
                {
                    p = new MsSqlExtendedProperty
                    {
                        MajorId = (int)dr["table_id"],
                        MinorId = null,
                        Name = dr["name"].ToString(),
                        Value = dr["value"].ToString()
                    };
                    extProps.Add(p);
                }
            };

            this.ExecuteSql(sql, action);

            foreach (MsSqlSchema schema in model.Schemas.Values)
            {
                foreach (MsSqlTable table in schema.Tables.Values)
                {
                    table.ExtendedProperties = new Dictionary<string, MsSqlExtendedProperty>(StringComparer.OrdinalIgnoreCase)
                        .AddRange(extProps.FindAll(p => p.MajorId == table.ObjectId));
                }
            }
        }
        #endregion

        #region resolve table column ext props
        public void ResolveTableColumnExtendedProperties(ref MsSqlModel model)
        {
            List<MsSqlExtendedProperty> extProps = new List<MsSqlExtendedProperty>();

            string sql = GetResource("Table_Column_Ext_Props");

            Action<IDataReader> action = (dr) =>
            {
                MsSqlExtendedProperty p = null;
                while (dr.Read())
                {
                    p = new MsSqlExtendedProperty
                    {
                        MajorId = (int)dr["table_id"],
                        MinorId = (int)dr["column_id"],
                        Name = dr["name"].ToString(),
                        Value = dr["value"].ToString()
                    };
                    extProps.Add(p);
                }
            };

            this.ExecuteSql(sql, action);

            foreach (MsSqlSchema schema in model.Schemas.Values)
            {
                foreach (MsSqlTable table in schema.Tables.Values)
                {
                    foreach (MsSqlColumn column in table.Columns.Values)
                    {
                        column.ExtendedProperties = new Dictionary<string, MsSqlExtendedProperty>(StringComparer.OrdinalIgnoreCase)
                            .AddRange(extProps.FindAll(p => p.MajorId == table.ObjectId && p.MinorId == column.ColumnId));
                    }
                }
            }
        }
        #endregion

        #region resolve view ext props
        public void ResolveViewExtendedProperties(ref MsSqlModel model)
        {
            List<MsSqlExtendedProperty> extProps = new List<MsSqlExtendedProperty>();

            string sql = GetResource("View_Ext_Props");

            Action<IDataReader> action = (dr) =>
            {
                MsSqlExtendedProperty p = null;
                while (dr.Read())
                {
                    p = new MsSqlExtendedProperty
                    {
                        MajorId = (int)dr["view_id"],
                        MinorId = null,
                        Name = dr["name"].ToString(),
                        Value = dr["value"].ToString()
                    };
                    extProps.Add(p);
                }
            };

            this.ExecuteSql(sql, action);

            foreach (MsSqlSchema schema in model.Schemas.Values)
            {
                foreach (MsSqlView view in schema.Views.Values)
                {
                    view.ExtendedProperties = new Dictionary<string, MsSqlExtendedProperty>(StringComparer.OrdinalIgnoreCase)
                        .AddRange(extProps.FindAll(p => p.MajorId == view.ObjectId));
                }
            }
        }
        #endregion

        #region resolve view column ext props
        public void ResolveViewColumnExtendedProperties(ref MsSqlModel model)
        {
            List<MsSqlExtendedProperty> extProps = new List<MsSqlExtendedProperty>();

            string sql = GetResource("View_Column_Ext_Props");

            Action<IDataReader> action = (dr) =>
            {
                MsSqlExtendedProperty p = null;
                while (dr.Read())
                {
                    p = new MsSqlExtendedProperty
                    {
                        MajorId = (int)dr["view_id"],
                        MinorId = (int)dr["column_id"],
                        Name = dr["name"].ToString(),
                        Value = dr["value"].ToString()
                    };
                    extProps.Add(p);
                }
            };

            this.ExecuteSql(sql, action);

            foreach (MsSqlSchema schema in model.Schemas.Values)
            {
                foreach (MsSqlView view in schema.Views.Values)
                {
                    foreach (MsSqlColumn column in view.Columns.Values)
                    {
                        column.ExtendedProperties = new Dictionary<string, MsSqlExtendedProperty>(StringComparer.OrdinalIgnoreCase)
                            .AddRange(extProps.FindAll(p => p.MajorId == view.ObjectId && p.MinorId == column.ColumnId));
                    }
                }
            }
        }
        #endregion
    }
}
