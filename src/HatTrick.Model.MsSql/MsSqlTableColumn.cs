using HatTrick.Model.Sql;
using System;
using System.Data;

namespace HatTrick.Model.MsSql
{
    public class MsSqlTableColumn : MsSqlColumn, ISqlTableColumn, IChildOf<MsSqlTable>
    {
        #region internals
        private MsSqlTable? _parent;
        #endregion

        #region interface
        public string? ParentIdentifier => _parent?.Identifier;
        #endregion

        #region apply
        public void Apply(Action<MsSqlTableColumn> action)
        {
            action(this);
        }
        #endregion

        #region get parent
        public MsSqlTable? GetParent()
            => _parent;
        #endregion

        #region set parent
        public void SetParent(MsSqlTable parent)
            => _parent = parent;
        #endregion
    }
}
