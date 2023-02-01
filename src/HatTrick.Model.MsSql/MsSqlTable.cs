using HatTrick.Model.Sql;
using System;
using System.Collections.Generic;

namespace HatTrick.Model.MsSql
{
    public class MsSqlTable : IDatabaseObjectModifier<MsSqlTable>, ISqlTable, IChildOf<MsSqlSchema>
    {
        #region internals
        private MsSqlSchema? _parent;
        #endregion

        #region interface
        public string Name { get; set; } = string.Empty;

        public IDictionary<string, object> Meta { get; set; } = new Dictionary<string, object>();

        public string Identifier { get; set; } = string.Empty;

        public string ParentIdentifier => _parent?.Identifier ?? string.Empty;

        public DatabaseObjectList<MsSqlTableColumn> Columns { get; set; } = new();

        public DatabaseObjectList<MsSqlIndex> Indexes { get; set; } = new();

        public DatabaseObjectList<MsSqlRelationship> Relationships { get; set; } = new();

        public DatabaseObjectList<MsSqlTrigger> Triggers { get; set; } = new();

        public DatabaseObjectList<MsSqlExtendedProperty> ExtendedProperties { get; set; } = new();
        #endregion

        #region methods
        public void Apply(Action<MsSqlTable> action)
        {
            action(this);
        }

        public void SetParent(MsSqlSchema schema)
        {
            _parent = schema;
        }

        public MsSqlSchema? GetParent()
        {
            return _parent;
        }

        public override string ToString()
            => $"{Identifier}:{Name}";
        #endregion
    }
}