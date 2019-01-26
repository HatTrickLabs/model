using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace HatTrick.Model.MsSql
{
    public class MsSqlParameter : IName, IMeta
    {
        #region interface
        public int ParentObjectId { get; set; }

        public string Name { get; set; }

        public int ParameterId { get; set; } //also ordinal

        public string SqlTypeName { get; set; }

        public SqlDbType SqlType { get; set; }

        public bool HasDefaultValue { get; set; } //will always be false for tsql procedures (CLR procs only)

        public object DefaultValue { get; set; } //will always be null for tsql procedures (CLR procs only)

        public byte Scale { get; set; }

        public byte Precision { get; set; }

        public short MaxLength { get; set; }

        public bool IsOutput { get; set; }

        public bool IsReadOnly { get; set; }

        public string Meta { get; set; }
        #endregion

        #region apply
        public void Apply(Action<MsSqlParameter> action)
        {
            action(this);
        }
        #endregion
    }
}