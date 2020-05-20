using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace HatTrick.Model.MsSql
{
    public class MsSqlView : INamedMeta
    {
        #region interface
        public INamedMeta Parent { get; set; }
        public int ObjectId { get; set; }

        public string Name { get; set; }

        public EnumerableNamedMetaSet<MsSqlColumn> Columns { get; set; }

        public EnumerableNamedMetaSet<MsSqlExtendedProperty> ExtendedProperties { get; set; }

        public string Meta { get; set; }
        #endregion

        #region apply
        public void Apply(Action<MsSqlView> action)
        {
            action(this);
        }
        #endregion
    }
}