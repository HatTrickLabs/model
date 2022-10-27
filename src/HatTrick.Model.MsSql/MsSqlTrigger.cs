using HatTrick.Model.Sql;
using System;

namespace HatTrick.Model.MsSql
{
    public class MsSqlTrigger : ISqlTrigger
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
