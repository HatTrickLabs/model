using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HatTrick.Model.MsSql
{
    public class EnumerableNamedMetaSet<T> : IDictionary, IEnumerable<T> where T : INamedMeta
    {
        #region internals
        private Dictionary<string, T> _set;
        #endregion

        #region interface
        public T this[string key]
        {
            get { return _set[key]; }
        }

        public object this[object key]
        {
            get { return this[key.ToString()]; }
            set { _set[key.ToString()] = (T)value; }
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

        public bool IsFixedSize => (_set as IDictionary).IsFixedSize;

        public bool IsReadOnly => (_set as IDictionary).IsReadOnly;
        #endregion

        #region constructors
        public EnumerableNamedMetaSet()
        {
            _set = new Dictionary<string, T>(StringComparer.OrdinalIgnoreCase);
        }

        public EnumerableNamedMetaSet(IEnumerable<T> values)
        {
            _set = new Dictionary<string, T>(StringComparer.OrdinalIgnoreCase);
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
                throw new ArgumentException("key must be a string", nameof(key));

            if (!(value is T val))
                throw new ArgumentException($"value must be a type of {typeof(T)}", nameof(value));

            if (val.Name != name)
                throw new ArgumentException("argument provided for 'key' must be a string and the string value must be equal to ((T)value).Name");

            this.Add(val);
        }
        #endregion

        #region region add range
        public EnumerableNamedMetaSet<T> AddRange(EnumerableNamedMetaSet<T> values)
        {
            if (values == null || values.Count == 0)
                return this;

            foreach (var value in values)
            {
                _set.Add(value.Name, value);
            }

            return this;
        }
        #endregion

        #region remove
        public void Remove(string key)
        {
            _set.Remove(key);
        }

        public void Remove(object key)
        {
            string name = (key as string);
            if (name == null)
                throw new ArgumentException("key must be a string", nameof(key));

            _set.Remove(name);
        }
        #endregion

        #region contains
        public bool Contains(object key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            string name = (key as string);
            if (name == null)
                throw new ArgumentException("key must be a string", nameof(key));

            return _set.Keys.Contains(name);
        }
        #endregion

        #region has match
        public bool HasMatch(string key, Func<string, string, bool> isKeyMatch)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if (key == string.Empty)
                throw new ArgumentException("argument must contain a value", nameof(key));

            bool found = false;
            foreach (var item in _set)
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

        #region get matches
        public EnumerableNamedMetaSet<T> GetMatches(string key, Func<string, string, bool> isKeyMatch)
        {
            EnumerableNamedMetaSet<T> matches = new EnumerableNamedMetaSet<T>();
            foreach (var item in _set)
            {
                if (isKeyMatch(item.Key, key))
                {
                    matches.Add(item.Key, item.Value);
                }
            }

            return matches;
        }
        #endregion

        #region get match list
        public List<T> GetMatchList(string key, Func<string, string, bool> isKeyMatch)
        {
            List<T> matches = new List<T>();
            foreach (var item in _set)
            {
                if (isKeyMatch(item.Key, key))
                {
                    matches.Add(item.Value);
                }
            }

            return matches;
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

        #region implict => EnumerableNamedSet<INamedMeta>
        public static implicit operator EnumerableNamedMetaSet<INamedMeta>(EnumerableNamedMetaSet<T> from)
        {
            EnumerableNamedMetaSet<INamedMeta> to = new EnumerableNamedMetaSet<INamedMeta>();
            foreach (var item in from._set)
            {
                to.Add(item.Value);
            }
            return to;
        }
        #endregion
    }
}
