using HatTrick.Model.Sql;
using System;
using System.Collections.Generic;

namespace HatTrick.Model.MySql
{
    public class MySqlView : IDatabaseObjectModifier<MySqlView>, ISqlView, IChildOf<MySqlSchema>
    {
        #region internals
        private MySqlSchema? _parent;
        #endregion

        #region interface
        public string Name { get; set; } = string.Empty;

        public string Meta { get; set; } = string.Empty;

        public string Identifier { get; set; } = string.Empty;

        public string ParentIdentifier => _parent?.Identifier ?? string.Empty;

        public DatabaseObjectList<MySqlViewColumn> Columns { get; set; } = new();
        #endregion

        #region methods
        public void Apply(Action<MySqlView> action)
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