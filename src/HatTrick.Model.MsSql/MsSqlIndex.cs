using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace HatTrick.Model.MsSql
{
    public class MsSqlIndex
    {
        #region interface
        public int ParentObjectId { get; set; }

        public string Name { get; set; }

        public int IndexId { get; set; }

        public bool IsPrimaryKey { get; set; }

        public bool IsUnique { get; set; }

        public MsSqlIndexedColumn[] IndexedColumns { get; set; }
        #endregion

        #region apply
        public void Apply(Action<MsSqlIndex> action)
        {
            action(this);
        }
        #endregion
    }
}