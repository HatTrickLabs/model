using HatTrick.Model.Sql;
using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace HatTrick.Model.MySql
{
    public class MySqlParameter : IDatabaseObjectModifier<MySqlParameter>, ISqlParameter, IChildOf<MySqlProcedure>
    {
        #region internals
        private MySqlProcedure? _parent;
        #endregion

        #region interface
        public string Name { get; set; } = string.Empty;

        public string Meta { get; set; } = string.Empty;

        public string Identifier { get; set; } = string.Empty;

        public string ParentIdentifier => _parent?.Identifier ?? string.Empty;

        public string SqlTypeName { get; set; } = string.Empty;

        public MySqlDbType SqlType { get; set; }

        public bool IsOutput { get; set; }

        public byte? Scale { get; set; }

        public byte? Precision { get; set; }

        public long? MaxLength { get; set; }
        #endregion

        #region methods
        public void Apply(Action<MySqlParameter> action)
        {
            action(this);
        }

        public void SetParent(MySqlProcedure schema)
        {
            _parent = schema;
        }

        public MySqlProcedure? GetParent()
        {
            return _parent;
        }
        #endregion
    }
}