using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace HatTrick.Model.MsSql
{
    public class MsSqlSchema
    {
        #region interface
        public int SchemaId { get; set; }

        public string Name { get; set; }

        public Dictionary<string, MsSqlTable> Tables { get; set; }

        public Dictionary<string, MsSqlView> Views { get; set; }

        public Dictionary<string, MsSqlRelationship> Relationships { get; set; }

        public Dictionary<string, MsSqlProcedure> Procedures { get; set; }
        #endregion

        #region apply
        public void Apply(Action<MsSqlSchema> action)
        {
            action(this);
        }
        #endregion
    }
}