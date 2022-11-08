using HatTrick.Model.Sql;
using System;
using System.Collections.Generic;

namespace HatTrick.Model.MsSql
{
    public class MsSqlModel : ISqlModel, IDatabaseObjectModifier<MsSqlModel>
    {
        #region interface
        public string Name { get; set; } = string.Empty;

        public string Meta { get; set; } = string.Empty;

        public string Identifier { get; set; } = string.Empty;

        public DatabaseObjectList<MsSqlSchema> Schemas { get; set; } = new();
        #endregion

        #region apply
        public void Apply(Action<MsSqlModel> action)
        {
            action(this);
        }
        #endregion
    }
}