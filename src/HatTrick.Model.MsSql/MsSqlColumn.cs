using HatTrick.Model.Sql;
using System;
using System.Collections.Generic;
using System.Data;

namespace HatTrick.Model.MsSql
{
    public abstract class MsSqlColumn : ISqlColumn
    {
        #region internals
        private INamedMeta _parent;
		#endregion

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

        public Dictionary<string, MsSqlExtendedProperty> ExtendedProperties { get; set; } = new();

        public string Meta { get; set; }
        #endregion

        #region set parent
        protected void SetParent(INamedMeta parent)
        {
            _parent = parent;
        }
        #endregion

        #region get parent
        public INamedMeta GetParent()
        {
            return _parent;
        }
        #endregion

        #region apply
        public void Apply(Action<MsSqlColumn> action)
        {
            action(this);
        }
        #endregion
    }
}