using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HatTrick.Model.Sql
{
    public class DatabaseObjectList<T> : List<T>
        where T : IDatabaseObject
    {
        /// <summary>
        /// Find a database object in the list by it's name.
        /// </summary>
        /// <param name="name">The name of the database object.</param>
        /// <returns>A database object of type <typeparamref name="T"/> if the object is in the list.</returns>
        /// <exception cref="DatabaseObjectNotFoundException{T}">The exception thrown when the item with <paramref name="name"/> is not in the list.</exception>
        public T this[string name]
        {
            get
            { 
                var item = this.SingleOrDefault(s => string.Equals(s.Name, name, StringComparison.OrdinalIgnoreCase));
                if (item is null)
                    throw new DatabaseObjectNotFoundException<T>(name);
                return item;
            }
        }

        public DatabaseObjectList() : base() { }
        public DatabaseObjectList(int capacity) : base(capacity) { }
        public DatabaseObjectList(IEnumerable<T> collection) : base(collection) { }

        public bool Remove(string name)
        {
            var item = this.SingleOrDefault(s => string.Equals(s.Name, name, StringComparison.OrdinalIgnoreCase));
            if (item is null)
                return false;
            return Remove(item);
        }
    }
}
