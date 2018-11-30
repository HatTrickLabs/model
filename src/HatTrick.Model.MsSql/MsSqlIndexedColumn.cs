using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace HatTrick.Model.MsSql
{
    public class MsSqlIndexedColumn
    {
        #region interface
        public int ParentObjectId { get; set; }

        public int IndexId { get; set; }

        public int ColumnId { get; set; }

        public string ColumnName { get; set; }

        public byte KeyOrdinal { get; set; }

        public bool IsDescendingKey { get; set; }

        public bool IsIncludedColumn { get; set; }
        #endregion

        #region apply
        public void Apply(Action<MsSqlIndexedColumn> action)
        {
            action(this);
        }
        #endregion
    }
}