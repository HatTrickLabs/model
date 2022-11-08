using HatTrick.Model.Sql;
using System;
using System.Collections.Generic;
using System.Data;

namespace HatTrick.Model.MsSql
{
    public class MsSqlProcedure : IDatabaseObjectModifier<MsSqlProcedure>, ISqlProcedure, IChildOf<MsSqlSchema>
    {
        #region internals
        private MsSqlSchema? _parent;
        #endregion

        #region interface
        public string Name { get; set; } = string.Empty;

        public string Meta { get; set; } = string.Empty;

        public string Identifier { get; set; } = string.Empty;

        public string ParentIdentifier => _parent?.Identifier ?? string.Empty;

        public bool IsStartupProcedure { get; set; }

        public DatabaseObjectList<MsSqlParameter> Parameters { get; set; } = new();
        #endregion

        #region methods
        public void Apply(Action<MsSqlProcedure> action)
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