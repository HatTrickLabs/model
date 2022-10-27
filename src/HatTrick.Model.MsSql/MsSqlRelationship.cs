using HatTrick.Model.Sql;
using System;
using System.Collections.Generic;

namespace HatTrick.Model.MsSql
{
    public class MsSqlRelationship : ISqlRelationship
    {
        #region internals
        private INamedMeta _parent;
		#endregion

		#region interface
		public string Name { get; set; }

        public int BaseTableId { get; set; }

        public string BaseTableName { get; set; }//Primary Key Table

        public IList<int> BaseColumnIds { get; set; }

        public IList<string> BaseColumnNames { get; set; }

        public int ReferenceTableId { get; set; }//Foreign Key Table

        public string ReferenceTableName { get; set; }

        public IList<int> ReferenceColumnIds { get; set; }

        public IList<string> ReferenceColumnNames { get; set; }

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