using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HatTrick.Model.Sql
{

    public abstract class SqlModelAccessor<T>
		where T : ISqlSchema
	{
		#region internals
		private readonly char _defaultWildcard = '*';

		private char _wildcardOverride;
		protected ISqlModel<T> Model { get; private set; }
		#endregion

		#region interface
		public char Wildcard => (_wildcardOverride == '\0') ? _defaultWildcard : _wildcardOverride;
		#endregion

		#region constructors
		protected SqlModelAccessor(ISqlModel<T> model)
		{
			Model = model;
			_wildcardOverride = '\0';
		}
		#endregion

		#region apply wildcard override
		public void ApplyWildcardOverride(char wildcard)
		{
			_wildcardOverride = wildcard;
		}
		#endregion

		#region resolve item
		public abstract INamedMeta ResolveItem(string path);
		#endregion

		#region resolve item set
		public abstract List<INamedMeta> ResolveItemSet(string path);

		public IList<U> ResolveItemSet<U>(string path) 
			where U : INamedMeta
		{
			var set = this.ResolveItemSet(path);
			List<U> typedSet = new List<U>();
			foreach (var item in set)
			{
				if (item is U itm)
					typedSet.Add(itm);
			}
			return typedSet;
		}

		public IList<U> ResolveItemSet<U>(string path, Predicate<U> predicate) 
			where U : INamedMeta
		{
			var set = this.ResolveItemSet(path);
			List<U> filteredSet = new List<U>();
			foreach (var item in set)
			{
				if (item is U itm && (predicate == null || predicate(itm)))
					filteredSet.Add(itm);
			}
			return filteredSet;
		}
        #endregion

        #region is string match
        public bool IsStringMatch(string left, string right)
		{
			if (string.IsNullOrEmpty(left))
				throw new ArgumentException("argument must contain a value", nameof(left));

			if (string.IsNullOrEmpty(right))
				throw new ArgumentException("argument must contain a value", nameof(right));

			if (right.Length == 1 && right[0] == Wildcard)
				return true;

			bool match = false;
			bool beginWild = (right[0] == Wildcard);
			bool endWild = (right[right.Length - 1] == Wildcard);

			if ((beginWild || endWild))
			{
				string sub = right;
				if (beginWild)
				{
					sub = right.Substring(1, right.Length - 1);
				}
				if (endWild)
				{
					sub = sub.Substring(0, sub.Length - 1);
				}
				if (beginWild && endWild)
				{
					match = left.IndexOf(sub, StringComparison.OrdinalIgnoreCase) > -1;
				}
				else if (beginWild)
				{
					match = left.IndexOf(sub, StringComparison.OrdinalIgnoreCase) == (left.Length - sub.Length);
				}
				else if (endWild)
				{
					match = left.IndexOf(sub, StringComparison.OrdinalIgnoreCase) == 0;
				}
			}
			else
			{
				match = string.Compare(left, right, true) == 0;
			}
			return match;
		}
        #endregion
    }
}
