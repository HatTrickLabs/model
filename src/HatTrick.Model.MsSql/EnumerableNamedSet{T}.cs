using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HatTrick.Model.MsSql
{
    public class EnumerableNamedSet<T> : IDictionary, IEnumerable<T> where T : IName
    {
        #region internals
        private Dictionary<string, T> _set;
        #endregion

        #region interface
        public T this[string name]
        {
            get { return _set[name]; }
        }

        public object this[object name]
        {
            get { return this[name.ToString()]; }
            set { _set[name.ToString()] = (T)value; }
        }

        public T this[int index]
        {
            get { return _set.ElementAt(index).Value; }
        }

        public ICollection Keys
        {
            get { return _set.Keys; }
        }

        public ICollection Values
        {
            get { return _set.Values; }
        }

        public int Count
        {
            get { return _set.Count; }
        }

        public object SyncRoot
        {
            get { return (_set as IDictionary).SyncRoot; }
        }

        public bool IsSynchronized
        {
            get { return (_set as IDictionary).IsSynchronized; }
        }

        public bool IsFixedSize => false;

        public bool IsReadOnly => false;
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

        public void Add(object key, object value)
        {
            string name = (key as string);
            if (name == null)
            {
                throw new ArgumentException("key must be a string", nameof(key));
            }
            if (!(value is T val))
            {
                throw new ArgumentException($"value must be a type of {typeof(T)}", nameof(value));
            }
            if (val.Name != name)
            {
                throw new ArgumentException("argument provided for 'key' must be a string and the string value must be equal to ((T)value).Name");
            }
            this.Add(val);
        }
        #endregion

        #region remove
        public void Remove(string name)
        {
            _set.Remove(name);
        }

        public void Remove(object key)
        {
            string name = (key as string);
            if (name == null)
            {
                throw new ArgumentException("key must be a string", nameof(key));
            }

            _set.Remove(name);
        }
        #endregion

        #region contains
        public bool Contains(object key)
        {
            string name = (key as string);
            if (name == null)
            {
                throw new ArgumentException("key must be a string", nameof(key));
            }

            return _set.Keys.Contains(name);
        }
        #endregion

        #region clear
        public void Clear()
        {
            _set.Clear();
        }
        #endregion

        #region copy to
        public void CopyTo(Array array, int index)
        {
            (_set as IDictionary).CopyTo(array, index);
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

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return (_set as IDictionary).GetEnumerator();
        }
        #endregion
    }
}
