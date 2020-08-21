using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace HatTrick.Model.MsSql
{
    public class MsSqlRelationship : INamedMeta
    {
        #region internals
        private INamedMeta _parent;
		#endregion

		#region interface
		//public INamedMeta Parent { get; set; }

		public string Name { get; set; }

        public int BaseTableId { get; set; }

        public string BaseTableName { get; set; }//Primary Key Table

        public int BaseColumnId { get; set; }

        public string BaseColumnName { get; set; }

        public int ReferenceTableId { get; set; }//Foreign Key Table

        public string ReferenceTableName { get; set; }

        public int ReferenceColumnId { get; set; }

        public string ReferenceColumnName { get; set; }

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
        public void Apply(Action<MsSqlRelationship> action)
        {
            action(this);
        }
        #endregion
    }
}