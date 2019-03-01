using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace HatTrick.Model.MsSql
{
    public class MsSqlSchema : INamedMeta
    {
        #region interface
        public int SchemaId { get; set; }

        public string Name { get; set; }

        public EnumerableNamedMetaSet<MsSqlTable> Tables { get; set; }

        public EnumerableNamedMetaSet<MsSqlView> Views { get; set; }

        public EnumerableNamedMetaSet<MsSqlRelationship> Relationships { get; set; }

        public EnumerableNamedMetaSet<MsSqlProcedure> Procedures { get; set; }

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