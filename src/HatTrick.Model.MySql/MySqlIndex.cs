using HatTrick.Model.Sql;
using System;
using System.Collections.Generic;

namespace HatTrick.Model.MySql
{
    public class MySqlIndex : IDatabaseObjectModifier<MySqlIndex>, ISqlIndex, IChildOf<MySqlTable>
    {
        #region internals
        private MySqlTable? _parent;
        #endregion

        #region interface
        public string Name { get; set; } = string.Empty;

        public string Meta { get; set; } = string.Empty;

        public string Identifier { get; set; } = string.Empty;

        public string ParentIdentifier => _parent?.Identifier ?? string.Empty;

        public bool IsPrimaryKey { get; set; }

        public bool IsUnique { get; set; }

        public DatabaseObjectList<MySqlIndexedColumn> IndexedColumns { get; set; } = new();

        public IndexType? IndexType { get; set; }
        #endregion

        #region methods
        public void Apply(Action<MySqlIndex> action)
        {
            action(this);
        }

        public void SetParent(MySqlTable table)
        {
            _parent = table;
        }

        public MySqlTable? GetParent()
        {
            return _parent;
        }
        #endregion
    }
}