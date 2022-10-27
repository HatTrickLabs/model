using HatTrick.Model.Sql;
using System;

namespace HatTrick.Model.MsSql
{
    public class MsSqlIndex : ISqlIndex
    {
        #region interface
        public int ParentObjectId { get; set; }

        public string Name { get; set; }

        public int IndexId { get; set; }

        public bool IsPrimaryKey { get; set; }

        public IndexType IndexType { get; set; }

        public bool IsUnique { get; set; }

        public MsSqlIndexedColumn[] IndexedColumns { get; set; } = new MsSqlIndexedColumn[] { };

        public string Meta { get; set; }
        #endregion

        #region apply
        public void Apply(Action<MsSqlIndex> action)
        {
            action(this);
        }
        #endregion
    }
}