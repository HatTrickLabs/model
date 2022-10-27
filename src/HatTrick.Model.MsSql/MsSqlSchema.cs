using HatTrick.Model.Sql;
using System;
using System.Collections.Generic;

namespace HatTrick.Model.MsSql
{
    public class MsSqlSchema : ISqlSchema
    {
        #region interface
        public int SchemaId { get; set; }

        public string Name { get; set; }

        public Dictionary<string, MsSqlTable> Tables { get; set; } = new();

        public Dictionary<string, MsSqlView> Views { get; set; } = new();

        public Dictionary<string, MsSqlRelationship> Relationships { get; set; } = new();

        public Dictionary<string, MsSqlProcedure> Procedures { get; set; } = new();

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