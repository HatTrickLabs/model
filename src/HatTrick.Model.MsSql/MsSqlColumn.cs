using HatTrick.Model.Sql;
using System;
using System.Collections.Generic;
using System.Data;

namespace HatTrick.Model.MsSql
{
    public abstract class MsSqlColumn : IDatabaseObjectModifier<MsSqlColumn>, ISqlColumn
    {
        #region interface
        public string Name { get; set; } = string.Empty;

        public string Meta { get; set; } = string.Empty;

        public string Identifier { get; set; } = string.Empty;

        public bool IsIdentity { get; set; }

        public bool IsComputed { get; set; }

        public string SqlTypeName { get; set; } = string.Empty;

        public SqlDbType SqlType { get; set; }

        public bool IsNullable { get; set; }

        public int OrdinalPosition { get; set; }

        public byte? Scale { get; set; }

        public byte? Precision { get; set; }

        public long? MaxLength { get; set; }

        public string? DefaultDefinition { get; set; }

        public DatabaseObjectList<MsSqlExtendedProperty> ExtendedProperties { get; set; } = new();
        #endregion

        #region methods
        public void Apply(Action<MsSqlColumn> action)
        {
            action(this);
        }

        public override string ToString()
            => $"{Identifier}:{Name} {SqlTypeName}";
        #endregion
    }
}