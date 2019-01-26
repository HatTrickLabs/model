using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace HatTrick.Model.MsSql
{
    public class MsSqlView : IName, IMeta
    {
        #region interface
        public int ObjectId { get; set; }

        public string Name { get; set; }

        public EnumerableNamedSet<MsSqlColumn> Columns { get; set; }

        public EnumerableNamedSet<MsSqlExtendedProperty> ExtendedProperties { get; set; }

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