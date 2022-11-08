using HatTrick.Model.Sql;
using System;
using System.Collections.Generic;

namespace HatTrick.Model.MsSql
{
    public class MsSqlView : IDatabaseObjectModifier<MsSqlView>, ISqlView, IChildOf<MsSqlSchema>
    {
        #region internals
        private MsSqlSchema? _parent;
        #endregion

        #region interface
        public string Name { get; set; } = string.Empty;

        public string Meta { get; set; } = string.Empty;

        public string Identifier { get; set; } = string.Empty;

        public string ParentIdentifier => _parent?.Identifier ?? string.Empty;

        public DatabaseObjectList<MsSqlViewColumn> Columns { get; set; } = new();

        public DatabaseObjectList<MsSqlExtendedProperty> ExtendedProperties { get; set; } = new();
        #endregion

		#region methods
		public void Apply(Action<MsSqlView> action)
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