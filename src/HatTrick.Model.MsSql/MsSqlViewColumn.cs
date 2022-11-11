using HatTrick.Model.Sql;
using System;
using System.Data;

namespace HatTrick.Model.MsSql
{
    public class MsSqlViewColumn : MsSqlColumn, ISqlViewColumn, IChildOf<MsSqlView>
    {
        #region internals
        private MsSqlView? _parent;
        #endregion

        #region interface
        public string? ParentIdentifier => _parent?.Identifier;
        #endregion

        #region apply
        public void Apply(Action<MsSqlViewColumn> action)
        {
            action(this);
        }
        #endregion

        #region get parent
        public MsSqlView? GetParent()
            => _parent;
        #endregion

        #region set parent
        public void SetParent(MsSqlView parent)
            => _parent = parent;
        #endregion
    }
}
