using HatTrick.Model.Sql;
using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace HatTrick.Model.MySql
{
    public class MySqlViewColumn : MySqlColumn, ISqlViewColumn, IChildOf<MySqlView>
    {
        #region internals
        private MySqlView? _parent;
        #endregion

        #region interface
        public string? ParentIdentifier => _parent?.Identifier;
        #endregion

        #region apply
        public void Apply(Action<MySqlViewColumn> action)
        {
            action(this);
        }
        #endregion

        #region get parent
        public MySqlView? GetParent()
            => _parent;
        #endregion

        #region set parent
        public void SetParent(MySqlView parent)
            => _parent = parent;
        #endregion
    }
}
