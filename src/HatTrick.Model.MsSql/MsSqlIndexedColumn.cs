using System;
using HatTrick.Model.Sql;

namespace HatTrick.Model.MsSql
{
    public class MsSqlIndexedColumn : IDatabaseObjectModifier<MsSqlIndexedColumn>, ISqlIndexedColumn, IChildOf<MsSqlIndex>
    {
        #region internals
        private MsSqlIndex? _parent;
        #endregion

        #region interface
        public string Name { get; set; } = string.Empty;

        public string Meta { get; set; } = string.Empty;

        public string Identifier { get; set; } = string.Empty;

        public string ParentIdentifier => _parent?.Identifier ?? string.Empty;

        public bool IsIncludedColumn { get; set; }

        public short OrdinalPosition { get; set; }

        public bool IsDescending { get; set; }
        #endregion

        #region methods
        public void Apply(Action<MsSqlIndexedColumn> action)
        {
            action(this);
        }

        public void SetParent(MsSqlIndex index)
        {
            _parent = index;
        }

        public MsSqlIndex? GetParent()
        {
            return _parent;
        }

        public override string ToString()
            => $"{Identifier}:{Name}, Ordinal: {OrdinalPosition}";
        #endregion
    }
}