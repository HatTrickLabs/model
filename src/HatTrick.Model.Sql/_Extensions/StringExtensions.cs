using System;
using System.Collections.Generic;
using System.Text;

namespace HatTrick.Model.Sql
{
    public static class StringExtensions
    {
		public static string[] SplitPath(this string path)
		{
			//walk the entire string to ensure . within [] is maintained
			if (path == null)
				return null;

			if (path == string.Empty)
				return new string[0];

			char c;
			bool inBracket = false;
			List<string> segments = new List<string>();
			StringBuilder segment = new StringBuilder();
			for (int i = 0; i < path.Length; i++)
			{
				c = path[i];
				if (c == '[' || c == ']')
				{
					inBracket = !inBracket;
					continue;
				}

				if (c == '.' && !inBracket)
				{
					segments.Add(segment.ToString());
					segment.Clear();
					continue;
				}

				segment.Append(c);
			}

			if (segment.Length > 0)
			{
				segments.Add(segment.ToString());
			}
			return segments.ToArray();
		}
	}
}
