using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace HatTrick.Model.MsSql
{
    public class MsSqlColumn : INamedMeta
    {
        #region interface
        public int ParentObjectId { get; set; }

        public int ColumnId { get; set; } //also ordinal

        public string Name { get; set; }

        public bool IsIdentity { get; set; }

        public string SqlTypeName { get; set; }

        public SqlDbType SqlType { get; set; }

        public bool IsNullable { get; set; }

        public byte Scale { get; set; }

        public byte Precision { get; set; }

        public short MaxLength { get; set; }

        public bool IsComputed { get; set; }

        public string DefaultDefinition { get; set; }

        public EnumerableNamedMetaSet<MsSqlExtendedProperty> ExtendedProperties { get; set; }

        public string Meta { get; set; }
        #endregion

        #region apply
        public void Apply(Action<MsSqlColumn> action)
        {
            action(this);
        }
        #endregion
    }
}