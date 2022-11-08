using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace HatTrick.Model.Sql
{
    public static class StringExtensions
    {
		public static string[] SplitPath(this string path, char segmentStart, char segmentEnd)
		{
			//walk the entire string to ensure . within segmentStart and segmentEnd is maintained
			if (path == null)
				return Array.Empty<string>();

			if (path == string.Empty)
				return new string[0];

			char c;
			bool inBracket = false;
			List<string> segments = new List<string>();
			StringBuilder segment = new StringBuilder();
			for (int i = 0; i < path.Length; i++)
			{
				c = path[i];
				if (c == segmentStart || c == segmentEnd)
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

        public static string ComputeIdentifier(this ISqlModel _, params string[] values)
        {
            var value = string.Join(":", values);
            var hash = MD5.Create();
            var hashed = hash.ComputeHash(Encoding.UTF8.GetBytes(value));
            return Convert.ToBase64String(hashed).TrimEnd('=').Replace('+', '-').Replace('/', '_');
        }
    }
}
