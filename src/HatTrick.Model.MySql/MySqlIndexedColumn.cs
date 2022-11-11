using System;
using HatTrick.Model.Sql;

namespace HatTrick.Model.MySql
{
    public class MySqlIndexedColumn : IDatabaseObjectModifier<MySqlIndexedColumn>, ISqlIndexedColumn, IChildOf<MySqlIndex>
    {
        #region internals
        private MySqlIndex? _parent;
        #endregion

        #region interface
        public string Name { get; set; } = string.Empty;

        public string Meta { get; set; } = string.Empty;

        public string Identifier { get; set; } = string.Empty;

        public string ParentIdentifier => _parent?.Identifier ?? string.Empty;

        public short OrdinalPosition { get; set; }

        public bool IsDescending { get; set; }
        #endregion

        #region methods
        public void Apply(Action<MySqlIndexedColumn> action)
        {
            action(this);
        }

        public void SetParent(MySqlIndex index)
        {
            _parent = index;
        }

        public MySqlIndex? GetParent()
        {
            return _parent;
        }
        #endregion
    }
}