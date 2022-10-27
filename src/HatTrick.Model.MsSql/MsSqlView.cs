using HatTrick.Model.Sql;
using System;
using System.Collections.Generic;

namespace HatTrick.Model.MsSql
{
    public class MsSqlView : ISqlView
    {
        #region internals
        private INamedMeta _parent;
		#endregion

		#region interface
		public int ObjectId { get; set; }

        public string Name { get; set; }

        public Dictionary<string, MsSqlViewColumn> Columns { get; set; } = new();

        public Dictionary<string, MsSqlExtendedProperty> ExtendedProperties { get; set; } = new();

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
		public void Apply(Action<MsSqlView> action)
        {
            action(this);
        }
        #endregion
    }
}