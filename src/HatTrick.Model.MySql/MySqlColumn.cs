using HatTrick.Model.Sql;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace HatTrick.Model.MySql
{
    public abstract class MySqlColumn : IDatabaseObjectModifier<MySqlColumn>, ISqlColumn
    {
        #region interface
        public string Name { get; set; } = string.Empty;

        public string Meta { get; set; } = string.Empty;

        public string Identifier { get; set; } = string.Empty;

        public string SqlTypeName { get; set; } = string.Empty;

        public MySqlDbType SqlType { get; set; }

        public bool AutoIncrement { get; set; }

        public bool IsNullable { get; set; }

        public int OrdinalPosition { get; set; }

        public byte? Scale { get; set; }

        public byte? Precision { get; set; }

        public long? MaxLength { get; set; }

        public string? GenerationExpression { get; set; }

        public string? DefaultDefinition { get; set; }

        public string? ColumnType { get; set; }

        public long? CharacterOctetLength { get; set; }
        #endregion

        #region apply
        public void Apply(Action<MySqlColumn> action)
        {
            action(this);
        }
        #endregion
    }
}