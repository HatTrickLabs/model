using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HatTrick.Model.MsSql
{
	public static class DictionaryExtensions
	{
		#region contains
		public static bool Contains<T>(this Dictionary<string, T> dictionary, string key) where T : INamedMeta
		{
			return dictionary.ContainsKey(key);
		}
		#endregion

		#region add
		public static void Add<T>(this Dictionary<string, T> dictionary, T value) where T : INamedMeta
		{
			dictionary.Add(value.Name, value);
		}
		#endregion

		#region add range
		public static Dictionary<string, T> AddRange<T>(this Dictionary<string, T> dictionary, List<T> values) where T : INamedMeta
		{
			for (int i = 0; i < values.Count; i++)
			{
				dictionary.Add(values[i].Name, values[i]);
			}
			return dictionary;
		}
		#endregion

		#region has match
		public static bool HasMatch<T>(this Dictionary<string, T> dictionary, string key, Func<string, string, bool> isKeyMatch) where T : INamedMeta
		{
			if (key == null)
				throw new ArgumentNullException(nameof(key));

			if (key == string.Empty)
				throw new ArgumentException("argument must contain a value", nameof(key));

			bool found = false;
			foreach (var item in dictionary)
			{
				if (isKeyMatch(item.Key, key))
				{
					found = true;
					break;
				}
			}
			return found;
		}
		#endregion

		#region get match list
		public static List<T> GetMatchList<T>(this Dictionary<string, T> dictionary, string key, Func<string, string, bool> isKeyMatch) where T : INamedMeta
		{
			List<T> matches = new List<T>();
			foreach (var item in dictionary)
			{
				if (isKeyMatch(item.Key, key))
				{
					matches.Add(item.Value);
				}
			}
			return matches;
		}
		#endregion
	}
}
