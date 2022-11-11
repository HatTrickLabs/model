using HatTrick.Model.Sql;
using System;
using System.Collections.Generic;

namespace HatTrick.Model.MsSql
{
    public class MsSqlSchema : IDatabaseObjectModifier<MsSqlSchema>, ISqlSchema, IChildOf<MsSqlModel>
    {
        #region internals
        private MsSqlModel? _parent;
        #endregion

        #region interface
        public string Name { get; set; } = string.Empty;

        public string Meta { get; set; } = string.Empty;

        public string Identifier { get; set; } = string.Empty;

        public string ParentIdentifier => _parent?.Identifier ?? string.Empty;

        public DatabaseObjectList<MsSqlTable> Tables { get; set; } = new();

        public DatabaseObjectList<MsSqlView> Views { get; set; } = new();

        public DatabaseObjectList<MsSqlProcedure> Procedures { get; set; } = new();
        #endregion

        #region methods
        public void Apply(Action<MsSqlSchema> action)
        {
            action(this);
        }

        public void SetParent(MsSqlModel model)
        {
            _parent = model;
        }

        public MsSqlModel? GetParent()
        {
            return _parent;
        }

        public override string ToString()
            => $"{Identifier}:{Name}";
        #endregion
    }
}