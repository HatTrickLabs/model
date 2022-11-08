using HatTrick.Model.Sql;
using System;
using System.Collections.Generic;

namespace HatTrick.Model.MySql
{
    public class MySqlSchema : IDatabaseObjectModifier<MySqlSchema>, ISqlSchema, IChildOf<MySqlModel>
    {
        #region internals
        private MySqlModel? _parent;
        #endregion

        #region interface
        public string Name { get; set; } = string.Empty;

        public string Meta { get; set; } = string.Empty;

        public string Identifier { get; set; } = string.Empty;

        public string ParentIdentifier => _parent?.Identifier ?? string.Empty;

        public DatabaseObjectList<MySqlTable> Tables { get; set; } = new();

        public DatabaseObjectList<MySqlView> Views { get; set; } = new();

        public DatabaseObjectList<MySqlProcedure> Procedures { get; set; } = new();
        #endregion

        #region methods
        public void Apply(Action<MySqlSchema> action)
        {
            action(this);
        }

        public void SetParent(MySqlModel model)
        {
            _parent = model;
        }

        public MySqlModel? GetParent()
        {
            return _parent;
        }
        #endregion
    }
}