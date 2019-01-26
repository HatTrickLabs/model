using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace HatTrick.Model.MsSql
{
    public class MsSqlSchema : IName, IMeta
    {
        #region interface
        public int SchemaId { get; set; }

        public string Name { get; set; }

        public EnumerableNamedSet<MsSqlTable> Tables { get; set; }

        public EnumerableNamedSet<MsSqlView> Views { get; set; }

        public EnumerableNamedSet<MsSqlRelationship> Relationships { get; set; }

        public EnumerableNamedSet<MsSqlProcedure> Procedures { get; set; }

        public string Meta { get; set; }
        #endregion

        #region apply
        public void Apply(Action<MsSqlSchema> action)
        {
            action(this);
        }
        #endregion
    }
}