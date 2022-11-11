using System;
using System.Collections.Generic;
using System.Text;

namespace HatTrick.Model.Sql
{
    public static class EnumerableExtensions
    {
        public static DatabaseObjectList<T> ToDatabaseObjectList<T>(this IEnumerable<T> collection)
            where T : IDatabaseObject
            => new DatabaseObjectList<T>(collection);
    }
}
