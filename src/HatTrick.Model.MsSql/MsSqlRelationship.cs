using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace HatTrick.Model.MsSql
{
    public class MsSqlRelationship : IName
    {
        #region interface
        public string Name { get; set; }

        public int BaseTableId { get; set; }

        public string BaseTableName { get; set; }//Primary Key Table

        public int BaseColumnId { get; set; }

        public string BaseColumnName { get; set; }

        public int ReferenceTableId { get; set; }//Foreign Key Table

        public string ReferenceTableName { get; set; }

        public int ReferenceColumnId { get; set; }

        public string ReferenceColumnName { get; set; }
        #endregion

        #region apply
        public void Apply(Action<MsSqlRelationship> action)
        {
            action(this);
        }
        #endregion
    }
}