using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace HatTrick.Model.MsSql
{
    public class MsSqlProcedure : INamedMeta
    {
        #region internals
        private INamedMeta _parent;
		#endregion

		#region interface
		//public INamedMeta Parent { get; set; }

		public int ObjectId { get; set; }

        public string Name { get; set; }

        public bool IsStartupProcedure { get; set; }

        public Dictionary<string, MsSqlParameter> Parameters { get; set; }

        public string Meta { get; set; }
        #endregion

        #region set parent
        public void SetParent(MsSqlSchema schema)
        {
            _parent = schema;
        }
        #endregion

        #region get parent
        public INamedMeta GetParent()
        {
            return _parent;
        }
        #endregion

        #region apply
        public void Apply(Action<MsSqlProcedure> action)
        {
            action(this);
        }
        #endregion
    }
}