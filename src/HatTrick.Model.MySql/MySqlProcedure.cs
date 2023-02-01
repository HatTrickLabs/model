using HatTrick.Model.Sql;
using System;
using System.Collections.Generic;

namespace HatTrick.Model.MySql
{
    public class MySqlProcedure : IDatabaseObjectModifier<MySqlProcedure>, ISqlProcedure, IChildOf<MySqlSchema>
    {
        #region internals
        private MySqlSchema? _parent;
        #endregion

        #region interface
        public string Name { get; set; } = string.Empty;

        public IDictionary<string, object> Meta { get; set; } = new Dictionary<string, object>();

        public string Identifier { get; set; } = string.Empty;

        public string ParentIdentifier => _parent?.Identifier ?? string.Empty;

        public DatabaseObjectList<MySqlParameter> Parameters { get; set; } = new();
        #endregion

        #region methods
        public void Apply(Action<MySqlProcedure> action)
        {
            action(this);
        }

        public void SetParent(MySqlSchema schema)
        {
            _parent = schema;
        }

        public MySqlSchema? GetParent()
        {
            return _parent;
        }
        #endregion
    }
}