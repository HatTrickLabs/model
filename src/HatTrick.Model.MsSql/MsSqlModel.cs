using HatTrick.Model.Sql;
using System.Collections.Generic;

namespace HatTrick.Model.MsSql
{
    public class MsSqlModel : ISqlModel<MsSqlSchema>
    {
        #region interface
		public string Name {  get; set;  }

        public Dictionary<string, MsSqlSchema> Schemas { get; set; } = new();

        public string Meta { get; set; }
        #endregion
    }
}