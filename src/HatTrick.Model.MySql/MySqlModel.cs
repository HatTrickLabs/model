using HatTrick.Model.Sql;
using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HatTrick.Model.MySql
{
    public class MySqlModel : ISqlModel, IDatabaseObjectModifier<MySqlModel>
    {
        #region interface
        public string Name { get; set; } = string.Empty;

        public string Meta { get; set; } = string.Empty;

        public string Identifier { get; set; } = string.Empty;

        public DatabaseObjectList<MySqlSchema> Schemas { get; set; } = new DatabaseObjectList<MySqlSchema>();
        #endregion

        #region apply
        public void Apply(Action<MySqlModel> action)
        {
            action(this);
        }
        #endregion        
    }
}