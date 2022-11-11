using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HatTrick.Model.Sql
{
    public static class DatabaseObjectListExtensions
    {
        public static List<T> GetMatchList<T>(this DatabaseObjectList<T> list, string name, Func<string, string, bool> isNameMatch) 
            where T : IDatabaseObject
        {
            List<T> matches = new List<T>();
            foreach (var item in list)
            {
                if (isNameMatch(item.Name, name))
                {
                    matches.Add(item);
                }
            }
            return matches;
        }

        public static bool Contains<T>(this DatabaseObjectList<T> list, string name)
            where T : IDatabaseObject
        {
            return list.SingleOrDefault(l => string.Equals(l.Name, name, StringComparison.OrdinalIgnoreCase)) is not null;
        }
    }
}
