﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace HatTrick.Model.MsSql
{
    public class MsSqlModelBuilder
    {
        #region internals
        private ResourceAccessor _resourceAccessor;
        private SqlConnection _sqlConnection;
        #endregion

        #region interface
        public Action<Exception> OnError { get; set; }
        #endregion

        #region constructors
        public MsSqlModelBuilder(string sqlConnectionString)
        {
            _resourceAccessor = new ResourceAccessor();
            _sqlConnection = new SqlConnection(sqlConnectionString);
        }

        public MsSqlModelBuilder(SqlConnection sqlConnection)
        {
            _resourceAccessor = new ResourceAccessor();
            _sqlConnection = sqlConnection;
        }
        #endregion

        #region ensure connection
        private DbConnection EnsureConnection()
        {
            if (_sqlConnection.State != ConnectionState.Open)
            {
                _sqlConnection.Open();
            }

            return _sqlConnection;
        }
        #endregion

        #region close connection
        private void CloseConnection()
        {
            if (_sqlConnection != null)
            {
                if (_sqlConnection.State != ConnectionState.Closed)
                {
                    _sqlConnection.Close();
                }
            }
        }
        #endregion

        #region build
        public MsSqlModel Build()
        {
            MsSqlModel model = new MsSqlModel();

            try
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
                this.ResolveTableExtendedProperties(ref model);
                this.ResolveTableColumnExtendedProperties(ref model);
                this.ResolveViewExtendedProperties(ref model);
                this.ResolveViewColumnExtendedProperties(ref model);
            }
            catch(Exception ex)
            {
                if (this.OnError != null)
                { this.OnError.Invoke(ex); }
                else
                { throw; }
            }
            finally
            {
                this.CloseConnection();
            }
            return model;
        }
        #endregion

        #region execute sql
        private void ExecuteSql(string sql, Action<DbDataReader> action)
        {
            DbDataReader reader = null;
            try
            {
                var cmd = this.EnsureConnection().CreateCommand();
#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
                cmd.CommandText = sql;
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities
                cmd.CommandType = CommandType.Text;

                reader = cmd.ExecuteReader(CommandBehavior.SingleResult);

                action(reader);
            }
            catch
            {
                throw;
            }
            finally
            {
                if (reader != null && !reader.IsClosed) { reader.Close(); }
            }
        }
        #endregion

        #region resolve name
        public void ResolveName(ref MsSqlModel model)
        {
            var conn = this.EnsureConnection();
            model.Name = conn.Database;
        }
        #endregion

        #region resolve schemas
        public void ResolveSchemas(ref MsSqlModel model)
        {
            EnumerableNamedMetaSet<MsSqlSchema> schemas = new EnumerableNamedMetaSet<MsSqlSchema>();

            string sql = _resourceAccessor.Get("Schema");

            Action<DbDataReader> action = (dr) =>
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

            string sql = _resourceAccessor.Get("Table");

            Action<DbDataReader> action = (dr) =>
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

            foreach (MsSqlSchema schema in model.Schemas)
            {
                schema.Tables = new EnumerableNamedMetaSet<MsSqlTable>(tables.FindAll(t => t.Item1 == schema.Name).ConvertAll(t => t.Item2));
                foreach (var table in schema.Tables)
                {
                    table.Parent = schema;
                }
            }
        }
        #endregion

        #region resolve table columns
        public void ResolveTableColumns(ref MsSqlModel model)
        {
            List<MsSqlTableColumn> columns = new List<MsSqlTableColumn>();

            string sql = _resourceAccessor.Get("Table_Column");

            Action<DbDataReader> action = (dr) =>
            {
                SqlDbType sqlType;
                MsSqlTableColumn c = null;
                while (dr.Read())
                {
                    bool typeParsed = Enum.TryParse<SqlDbType>((string)dr["data_type_name"], true, out sqlType);
                    c = new MsSqlTableColumn
                    {
                        ColumnId = (int)dr["column_id"],
                        ParentObjectId = (int)dr["table_id"],
                        Name = (string)dr["column_name"],
                        IsIdentity = (bool)dr["is_identity"],
                        SqlTypeName = (string)dr["data_type_name"],
                        SqlType = typeParsed ? sqlType : SqlDbType.VarChar,
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

            foreach (MsSqlSchema schema in model.Schemas)
            {
                foreach (MsSqlTable table in schema.Tables)
                {
                    table.Columns = new EnumerableNamedMetaSet<MsSqlTableColumn>(columns.FindAll(c => c.ParentObjectId == table.ObjectId));
                    foreach (var column in table.Columns)
                    {
                        column.Parent = table;
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

            string sql = _resourceAccessor.Get("Table_Index");

            Action<DbDataReader> action = (dr) =>
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
                        ColumnName = (string)dr["column_name"],
                        KeyOrdinal = (byte)dr["key_ordinal"],
                        IsDescendingKey = (bool)dr["is_descending_key"],
                        IsIncludedColumn = (bool)dr["is_included_column"]
                    };
                    indexedColumns.Add(idxCol);
                }
            };

            this.ExecuteSql(sql, action);

            foreach (MsSqlSchema schema in model.Schemas)
            {
                foreach (MsSqlTable table in schema.Tables)
                {
                    table.Indexes = new EnumerableNamedMetaSet<MsSqlIndex>(indexes.FindAll(i => i.ParentObjectId == table.ObjectId));
                    foreach (MsSqlIndex index in table.Indexes)
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

            string sql = _resourceAccessor.Get("View");

            Action<DbDataReader> action = (dr) =>
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

            foreach (MsSqlSchema schema in model.Schemas)
            {
                schema.Views = new EnumerableNamedMetaSet<MsSqlView>(views.FindAll(v => v.Item1 == schema.Name).ConvertAll(v => v.Item2));
                foreach (var view in schema.Views)
                {
                    view.Parent = schema;
                }
            }
        }
        #endregion

        #region resolve view columns
        public void ResolveViewColumns(ref MsSqlModel model)
        {
            List<MsSqlViewColumn> columns = new List<MsSqlViewColumn>();

            string sql = _resourceAccessor.Get("View_Column");

            Action<DbDataReader> action = (dr) =>
            {
                SqlDbType sqlType;
                MsSqlViewColumn c = null;
                while (dr.Read())
                {
                    bool typeParsed = Enum.TryParse<SqlDbType>((string)dr["data_type_name"], true, out sqlType);
                    c = new MsSqlViewColumn
                    {
                        ColumnId = (int)dr["column_id"],
                        ParentObjectId = (int)dr["view_id"],
                        Name = (string)dr["column_name"],
                        IsIdentity = (bool)dr["is_identity"],
                        SqlTypeName = (string)dr["data_type_name"],
                        SqlType = typeParsed ? sqlType : SqlDbType.VarChar,
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

            foreach (MsSqlSchema schema in model.Schemas)
            {
                foreach (MsSqlView view in schema.Views)
                {
                    view.Columns = new EnumerableNamedMetaSet<MsSqlViewColumn>(columns.FindAll(c => c.ParentObjectId == view.ObjectId));
                    foreach (var column in view.Columns)
                    {
                        column.Parent = view;
                    }
                }
            }
        }
        #endregion

        #region resolve sprocs
        public void ResolveProcedures(ref MsSqlModel model)
        {
            List<(string, MsSqlProcedure)> sprocs = new List<(string, MsSqlProcedure)>();

            string sql = _resourceAccessor.Get("Procedure");

            Action<DbDataReader> action = (dr) =>
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

            foreach (MsSqlSchema schema in model.Schemas)
            {
                schema.Procedures = new EnumerableNamedMetaSet<MsSqlProcedure>(sprocs.FindAll(p => p.Item1 == schema.Name).ConvertAll(p => p.Item2).ToList());
                foreach (var procedure in schema.Procedures)
                {
                    procedure.Parent = schema;
                }
            }
        }
        #endregion

        #region resolve sproc parameters
        public void ResolveProcedureParameters(ref MsSqlModel model)
        {
            List<MsSqlParameter> parameters = new List<MsSqlParameter>();

            string sql = _resourceAccessor.Get("Procedure_Parameter");

            Action<DbDataReader> action = (dr) =>
            {
                SqlDbType sqlType;
                MsSqlParameter p = null;
                while (dr.Read())
                {
                    bool typeParsed = Enum.TryParse<SqlDbType>((string)dr["data_type_name"], true, out sqlType);
                    p = new MsSqlParameter
                    {
                        ParentObjectId = (int)dr["sproc_id"],
                        ParameterId = (int)dr["parameter_id"],
                        Name = (string)dr["parameter_name"],
                        SqlTypeName = (string)dr["data_type_name"],
                        SqlType = typeParsed ? sqlType : SqlDbType.VarChar,
                        IsOutput = (bool)dr["is_output"],
                        IsReadOnly = (bool)dr["is_readonly"],
                        HasDefaultValue = (bool)dr["has_default_value"],
                        DefaultValue = dr["default_value"] == DBNull.Value ? null : (object)dr["default_value"],//only valid on clr procedures
                        Precision = (byte)dr["precision"],
                        Scale = (byte)dr["scale"],
                        MaxLength = (short)dr["max_length"]
                    };
                    parameters.Add(p);
                }
            };

            this.ExecuteSql(sql, action);

            foreach (MsSqlSchema schema in model.Schemas)
            {
                foreach (MsSqlProcedure sproc in schema.Procedures)
                {
                    sproc.Parameters = new EnumerableNamedMetaSet<MsSqlParameter>(parameters.FindAll(p => p.ParentObjectId == sproc.ObjectId));
                }
            }
        }
        #endregion

        #region resolve relationships
        public void ResolveRelationships(ref MsSqlModel model)
        {
            List<(string, MsSqlRelationship)> relationships = new List<(string, MsSqlRelationship)>();

            string sql = _resourceAccessor.Get("Relationships");

            Action<DbDataReader> action = (dr) =>
            {
                string s = null;
                MsSqlRelationship p = null;
                while (dr.Read())
                {
                    s = (string)dr["schema_name"];
                    p = new MsSqlRelationship()
                    {
                        Name = (string)dr["relationship_name"],
                        BaseTableId = (int)dr["base_table_id"],
                        BaseTableName = (string)dr["base_table_name"],
                        BaseColumnId = (int)dr["base_column_id"],
                        BaseColumnName = (string)dr["base_column_name"],
                        ReferenceTableId = (int)dr["referenced_table_id"],
                        ReferenceTableName = (string)dr["referenced_table_name"],
                        ReferenceColumnId = (int)dr["referenced_column_id"],
                        ReferenceColumnName = (string)dr["referenced_column_name"]
                    };
                    relationships.Add((s, p));
                }
            };

            this.ExecuteSql(sql, action);

            foreach (MsSqlSchema schema in model.Schemas)
            {
                schema.Relationships = new EnumerableNamedMetaSet<MsSqlRelationship>(relationships.FindAll(p => p.Item1 == schema.Name).ConvertAll(p => p.Item2));
                foreach (var relationship in schema.Relationships)
                {
                    relationship.Parent = schema;
                }
            }
        }
        #endregion

        #region resolve table ext props
        public void ResolveTableExtendedProperties(ref MsSqlModel model)
        {
            List<MsSqlExtendedProperty> extProps = new List<MsSqlExtendedProperty>();

            string sql = _resourceAccessor.Get("Table_Ext_Props");

            Action<DbDataReader> action = (dr) =>
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

            foreach (MsSqlSchema schema in model.Schemas)
            {
                foreach (MsSqlTable table in schema.Tables)
                {
                    table.ExtendedProperties = new EnumerableNamedMetaSet<MsSqlExtendedProperty>(extProps.FindAll(p => p.MajorId == table.ObjectId));
                }
            }
        }
        #endregion

        #region resolve table column ext props
        public void ResolveTableColumnExtendedProperties(ref MsSqlModel model)
        {
            List<MsSqlExtendedProperty> extProps = new List<MsSqlExtendedProperty>();

            string sql = _resourceAccessor.Get("Table_Column_Ext_Props");

            Action<DbDataReader> action = (dr) =>
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

            foreach (MsSqlSchema schema in model.Schemas)
            {
                foreach (MsSqlTable table in schema.Tables)
                {
                    foreach (MsSqlColumn column in table.Columns)
                    {
                        column.ExtendedProperties = new EnumerableNamedMetaSet<MsSqlExtendedProperty>(
                            extProps.FindAll(p => p.MajorId == table.ObjectId && p.MinorId == column.ColumnId)
                        );
                    }
                }
            }
        }
        #endregion

        #region resolve view ext props
        public void ResolveViewExtendedProperties(ref MsSqlModel model)
        {
            List<MsSqlExtendedProperty> extProps = new List<MsSqlExtendedProperty>();

            string sql = _resourceAccessor.Get("View_Ext_Props");

            Action<DbDataReader> action = (dr) =>
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

            foreach (MsSqlSchema schema in model.Schemas)
            {
                foreach (MsSqlView view in schema.Views)
                {
                    view.ExtendedProperties = new EnumerableNamedMetaSet<MsSqlExtendedProperty>(extProps.FindAll(p => p.MajorId == view.ObjectId));
                }
            }
        }
        #endregion

        #region resolve view column ext props
        public void ResolveViewColumnExtendedProperties(ref MsSqlModel model)
        {
            List<MsSqlExtendedProperty> extProps = new List<MsSqlExtendedProperty>();

            string sql = _resourceAccessor.Get("View_Column_Ext_Props");

            Action<DbDataReader> action = (dr) =>
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

            foreach (MsSqlSchema schema in model.Schemas)
            {
                foreach (MsSqlView view in schema.Views)
                {
                    foreach (MsSqlColumn column in view.Columns)
                    {
                        column.ExtendedProperties = new EnumerableNamedMetaSet<MsSqlExtendedProperty>(
                            extProps.FindAll(p => p.MajorId == view.ObjectId && p.MinorId == column.ColumnId)
                        );
                    }
                }
            }
        }
        #endregion
    }
}
