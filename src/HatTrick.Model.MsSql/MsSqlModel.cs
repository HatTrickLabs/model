using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace HatTrick.Model.MsSql
{
    public class MsSqlModel : INamedMeta
    {
        #region internals
        private NamedMetaAccessor _accessor;
		#endregion

		#region interface
		public string Name {  get; set;  }

        public EnumerableNamedMetaSet<MsSqlSchema> Schemas { get; set; }

        public string Meta { get; set; }
        #endregion

        #region constructors
        public MsSqlModel()
        {
            _accessor = new NamedMetaAccessor(this);
        }
        #endregion

        #region resolve meta
        public INamedMeta ResolveItem(string path)
        {
            return _accessor.ResolveItem(path);
        }
        #endregion

        #region resolve item set
        public IEnumerable<INamedMeta> ResolveItemSet(string path)
        {
            return _accessor.ResolveItemSet(path);
        }

        public IEnumerable<T> ResolveItemSet<T>(string path) where T : INamedMeta
        {
            var set =_accessor.ResolveItemSet(path);
            List<T> typedSet = new List<T>();
            foreach (var item in set)
            {
                if (item is T itm)
                    typedSet.Add(itm);
            }
            return typedSet;
        }

        public IEnumerable<T> ResolveItemSet<T>(string path, Predicate<T> predicate) where T : INamedMeta
        {
            var set = _accessor.ResolveItemSet(path);
            List<T> typedSet = new List<T>();
            foreach (var item in set)
            {
                if (item is T itm)
                    typedSet.Add(itm);
            }
            return typedSet;
        }
        #endregion
    }
}