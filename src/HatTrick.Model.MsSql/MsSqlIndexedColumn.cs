using System;
using HatTrick.Model.Sql;

namespace HatTrick.Model.MsSql
{
    public class MsSqlIndexedColumn : ISqlIndexedColumn
    {
        #region interface
        public int ParentObjectId { get; set; }

        public int IndexId { get; set; }

        public int ColumnId { get; set; }

        [Obsolete("ColumnName will be removed in a future version.  Please use Name instead.")]
        public string ColumnName => Name;

        public string Name { get; set; }

        public byte KeyOrdinal { get; set; }

        public bool IsDescendingKey { get; set; }

        public bool IsIncludedColumn { get; set; }

        public string Meta { get; set; }
        #endregion

        #region apply
        public void Apply(Action<MsSqlIndexedColumn> action)
        {
            action(this);
        }
        #endregion
    }
}