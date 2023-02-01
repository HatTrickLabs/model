using HatTrick.Model.Sql;
using System;
using System.Collections.Generic;
using System.Data;

namespace HatTrick.Model.MsSql
{
    public class MsSqlParameter : IDatabaseObjectModifier<MsSqlParameter>, ISqlParameter, IChildOf<MsSqlProcedure>
    {
        #region internals
        private MsSqlProcedure? _parent;
        #endregion

        #region interface
        public string Name { get; set; } = string.Empty;

        public IDictionary<string, object> Meta { get; set; } = new Dictionary<string, object>();

        public string Identifier { get; set; } = string.Empty;

        public string ParentIdentifier => _parent?.Identifier ?? string.Empty;

        public string SqlTypeName { get; set; } = string.Empty;

        public SqlDbType SqlType { get; set; }

        public bool HasDefaultValue { get; set; } //will always be false for tsql procedures (CLR procs only)

        public object? DefaultValue { get; set; } //will always be null for tsql procedures (CLR procs only)

        public byte? Scale { get; set; }

        public byte? Precision { get; set; }

        public long? MaxLength { get; set; }

        public bool IsOutput { get; set; }

        public bool IsReadOnly { get; set; }

        public bool IsNullable { get; set; }
        #endregion

        #region methods
        public void Apply(Action<MsSqlParameter> action)
        {
            action(this);
        }

        public void SetParent(MsSqlProcedure schema)
        {
            _parent = schema;
        }

        public MsSqlProcedure? GetParent()
        {
            return _parent;
        }

        public override string ToString()
            => $"{Identifier}:{Name} {SqlTypeName}";
        #endregion
    }
}