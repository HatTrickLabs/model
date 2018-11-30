using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace HatTrick.Model.MsSql
{
    public class MsSqlView
    {
        #region interface
        public int ObjectId { get; set; }

        public string Name { get; set; }

        public Dictionary<string, MsSqlColumn> Columns { get; set; }
        #endregion

        #region apply
        public void Apply(Action<MsSqlView> action)
        {
            action(this);
        }
        #endregion
    }
}