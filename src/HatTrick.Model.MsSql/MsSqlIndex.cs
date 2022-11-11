using HatTrick.Model.Sql;
using System;
using System.Collections.Generic;

namespace HatTrick.Model.MsSql
{
    public class MsSqlIndex : IDatabaseObjectModifier<MsSqlIndex>, ISqlIndex, IChildOf<MsSqlTable>
    {
        #region internals
        private MsSqlTable? _parent;
        #endregion

        #region interface
        public string Name { get; set; } = string.Empty;

        public string Meta { get; set; } = string.Empty;

        public string Identifier { get; set; } = string.Empty;

        public string ParentIdentifier => _parent?.Identifier ?? string.Empty;

        public bool IsPrimaryKey { get; set; }

        public bool IsUnique { get; set; }

        public DatabaseObjectList<MsSqlIndexedColumn> IndexedColumns { get; set; } = new();

        public IndexType? IndexType { get; set; }
        #endregion

        #region apply
        public void Apply(Action<MsSqlIndex> action)
        {
            action(this);
        }

        public void SetParent(MsSqlTable table)
        {
            _parent = table;
        }

        public MsSqlTable? GetParent()
        {
            return _parent;
        }

        public override string ToString()
            => $"{Identifier}:{Name} IsPrimaryKey: {IsPrimaryKey}, IsUnique: {IsUnique}";
        #endregion
    }
}