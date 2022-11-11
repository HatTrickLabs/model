using HatTrick.Model.Sql;
using System;
using System.Collections.Generic;

namespace HatTrick.Model.MySql
{
    public class MySqlTable : IDatabaseObjectModifier<MySqlTable>, ISqlTable, IChildOf<MySqlSchema>
    {
        #region internals
        private MySqlSchema? _parent;
        #endregion

        #region interface
        public string Name { get; set; } = string.Empty;

        public string Meta { get; set; } = string.Empty;

        public string Identifier { get; set; } = string.Empty;

        public string ParentIdentifier => _parent?.Identifier ?? string.Empty;

        public DatabaseObjectList<MySqlTableColumn> Columns { get; set; } = new();

        public DatabaseObjectList<MySqlRelationship> Relationships { get; set; } = new();
        
        public DatabaseObjectList<MySqlIndex> Indexes { get; set; } = new();

        public DatabaseObjectList<MySqlTrigger> Triggers { get; set; } = new();
        #endregion

        #region methods
        public void Apply(Action<MySqlTable> action)
        {
            action(this);
        }

        public void SetParent(MySqlSchema schema)
        {
            _parent = schema;
        }

        public MySqlSchema? GetParent()
        {
            return _parent;
        }
        #endregion
    }
}