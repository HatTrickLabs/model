using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace HatTrick.Model.MsSql
{
	public class MsSqlTrigger : INamedMeta
	{
		#region internals
		private INamedMeta _parent;
		#endregion

		#region interface
		public int ParentObjectId { get; set; }

		public int ObjectId { get; set; }

		public string Name { get; set; }

		public bool IsDisabled { get; set; }

		public bool IsInsteadOfTrigger { get; set; }

		public string EventType { get; set; }

		public string Meta { get; set; }
		#endregion

		#region set parent
		public void SetParent(MsSqlTable table)
		{
			_parent = table;
		}
		#endregion

		#region get parent
		public INamedMeta GetParent()
		{
			return _parent;
		}
		#endregion

		#region apply
		public void Apply(Action<MsSqlTrigger> action)
		{
			action(this);
		}
		#endregion
	}
}
