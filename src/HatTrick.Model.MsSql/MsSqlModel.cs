using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace HatTrick.Model.MsSql
{
    public class MsSqlModel : INamedMeta
    {
        #region internals
        private SqlModelAccessor _accessor;
		#endregion

		#region interface
		public string Name {  get; set;  }

        public Dictionary<string, MsSqlSchema> Schemas { get; set; }

        public string Meta { get; set; }
        #endregion

        #region constructors
        public MsSqlModel()
        {
            _accessor = new SqlModelAccessor(this);
        }
        #endregion
    }
}