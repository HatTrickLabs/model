using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace HatTrick.Model.MsSql
{
    public class MsSqlProcedure : IName
    {
        #region inteface
        public int ObjectId { get; set; }

        public string Name { get; set; }

        public bool IsStartupProcedure { get; set; }

        public EnumerableNamedSet<MsSqlParameter> Parameters { get; set; }
        #endregion

        #region apply
        public void Apply(Action<MsSqlProcedure> action)
        {
            action(this);
        }
        #endregion
    }
}