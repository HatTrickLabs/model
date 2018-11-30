using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HatTrick.Model.MsSql
{
    public class EnumerableNamedSet<T> : IEnumerable<T> where T : IName
    {
        #region internals
        private Dictionary<string, T> _set;
        #endregion

        #region interface
        public T this[string name]
        {
            get { return _set[name]; }
        }
        #endregion

        #region constructors
        public EnumerableNamedSet()
        {
            _set = new Dictionary<string, T>();
        }

        public EnumerableNamedSet(IEnumerable<T> values)
        {
            _set = new Dictionary<string, T>();
            if (values != null && values.Count() > 0)
            {
                foreach (T v in values)
                {
                    _set.Add(v.Name, v);
                }
            }
        }
        #endregion

        #region add
        public void Add(T value)
        {
            _set.Add(value.Name, value);
        }
        #endregion

        #region remove
        public void Remove(string name)
        {
            _set.Remove(name);
        }
        #endregion

        #region get enumerator
        public IEnumerator<T> GetEnumerator()
        {
            return _set.Values.ToList().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        #endregion
    }
}
