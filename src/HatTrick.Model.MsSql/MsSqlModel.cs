using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace HatTrick.Model.MsSql
{
    public class MsSqlModel
    {
        #region interface
        public string MsSqlDbName { get; set; }

        public EnumerableNamedSet<MsSqlSchema> Schemas { get; set; }
        #endregion

        #region constructors
        public MsSqlModel()
        {
        }
        #endregion
    }
}