using HatTrick.Model.Sql;
using MySql.Data.MySqlClient;
using System;

namespace HatTrick.Model.MySql
{
    public class MySqlTableColumn : MySqlColumn, ISqlTableColumn, IChildOf<MySqlTable>
    {
        #region internals
        private MySqlTable? _parent;
        #endregion

        #region interface
        public string? ParentIdentifier => _parent?.Identifier;
        #endregion

        #region apply
        public void Apply(Action<MySqlTableColumn> action)
        {
            action(this);
        }
        #endregion

        #region get parent
        public MySqlTable? GetParent()
            => _parent;
        #endregion

        #region set parent
        public void SetParent(MySqlTable parent)
            => _parent = parent;
        #endregion
    }
}
