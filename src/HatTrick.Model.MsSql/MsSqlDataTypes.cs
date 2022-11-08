using System;
using System.Collections.Generic;
using System.Data;

namespace HatTrick.Model.MsSql
{
    public static class MsSqlDataTypes
    {
        #region internals
        private static readonly Dictionary<string, SqlDbType> _typeMap = new Dictionary<string, SqlDbType>(StringComparer.OrdinalIgnoreCase)
        {
            { "bit", SqlDbType.Bit },
            { "datetime", SqlDbType.DateTime },
            { "datetime2", SqlDbType.DateTime2 },
            { "smalldatetime", SqlDbType.SmallDateTime },
            { "datetimeoffset", SqlDbType.DateTimeOffset },
            { "date", SqlDbType.Date },
            { "timestamp", SqlDbType.Timestamp },
            { "time", SqlDbType.Time },
            { "varchar", SqlDbType.VarChar },
            { "nvarchar", SqlDbType.NVarChar },
            { "char", SqlDbType.Char },
            { "nchar", SqlDbType.NChar },
            { "tinyint", SqlDbType.TinyInt },
            { "smallint", SqlDbType.SmallInt },
            { "int", SqlDbType.Int },
            { "bigint", SqlDbType.BigInt },
            { "money", SqlDbType.Money },
            { "smallmoney", SqlDbType.SmallMoney },
            { "numeric", SqlDbType.Decimal },
            { "decimal", SqlDbType.Decimal },
            { "float", SqlDbType.Float },
            { "real", SqlDbType.Real },
            { "uniqueidentifier", SqlDbType.UniqueIdentifier },
            { "binary", SqlDbType.Binary },
            { "varbinary", SqlDbType.VarBinary },
            { "text", SqlDbType.Text },
            { "ntext", SqlDbType.NText },
            { "image", SqlDbType.Image },
            { "xml", SqlDbType.Xml },
        };

        private static readonly HashSet<string> _otherTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "hierarchyid",
            "geometry",
            "geography",
        };

        private static readonly MsSqlTypeDescriptor _bit = new MsSqlTypeDescriptor("bit", SqlDbType.Bit);
        private static readonly MsSqlTypeDescriptor _dateTime = new MsSqlTypeDescriptor("datetime", SqlDbType.DateTime);
        private static readonly MsSqlTypeDescriptor _dateTime2 = new MsSqlTypeDescriptor("datetime2", SqlDbType.DateTime2);
        private static readonly MsSqlTypeDescriptor _smalldatetime = new MsSqlTypeDescriptor("smalldatetime", SqlDbType.SmallDateTime);
        private static readonly MsSqlTypeDescriptor _dateTimeOffset = new MsSqlTypeDescriptor("datetimeoffset", SqlDbType.DateTimeOffset);
        private static readonly MsSqlTypeDescriptor _date = new MsSqlTypeDescriptor("date", SqlDbType.Date);
        private static readonly MsSqlTypeDescriptor _timestamp = new MsSqlTypeDescriptor("timestamp", SqlDbType.Timestamp);
        private static readonly MsSqlTypeDescriptor _time = new MsSqlTypeDescriptor("time", SqlDbType.Time);
        private static readonly MsSqlTypeDescriptor _varchar = new MsSqlTypeDescriptor("varchar", SqlDbType.VarChar);
        private static readonly MsSqlTypeDescriptor _char = new MsSqlTypeDescriptor("char", SqlDbType.Char);
        private static readonly MsSqlTypeDescriptor _nvarchar = new MsSqlTypeDescriptor("nvarchar", SqlDbType.NVarChar);
        private static readonly MsSqlTypeDescriptor _nchar = new MsSqlTypeDescriptor("nchar", SqlDbType.NChar);
        private static readonly MsSqlTypeDescriptor _tinyint = new MsSqlTypeDescriptor("tinyint", SqlDbType.TinyInt);
        private static readonly MsSqlTypeDescriptor _smallInt = new MsSqlTypeDescriptor("smallint", SqlDbType.SmallInt);
        private static readonly MsSqlTypeDescriptor _int = new MsSqlTypeDescriptor("int", SqlDbType.Int);
        private static readonly MsSqlTypeDescriptor _bigInt = new MsSqlTypeDescriptor("bigint", SqlDbType.BigInt);
        private static readonly MsSqlTypeDescriptor _money = new MsSqlTypeDescriptor("money", SqlDbType.Money);
        private static readonly MsSqlTypeDescriptor _smallmoney = new MsSqlTypeDescriptor("smallmoney", SqlDbType.SmallMoney);
        private static readonly MsSqlTypeDescriptor _numeric = new MsSqlTypeDescriptor("numeric", SqlDbType.Decimal);
        private static readonly MsSqlTypeDescriptor _decimal = new MsSqlTypeDescriptor("decimal", SqlDbType.Decimal);
        private static readonly MsSqlTypeDescriptor _float = new MsSqlTypeDescriptor("float", SqlDbType.Float);
        private static readonly MsSqlTypeDescriptor _real = new MsSqlTypeDescriptor("real", SqlDbType.Real);
        private static readonly MsSqlTypeDescriptor _uniqueIdentifier = new MsSqlTypeDescriptor("uniqueidentifier", SqlDbType.UniqueIdentifier);
        private static readonly MsSqlTypeDescriptor _binary = new MsSqlTypeDescriptor("binary", SqlDbType.Binary);
        private static readonly MsSqlTypeDescriptor _varbinary = new MsSqlTypeDescriptor("varbinary", SqlDbType.VarBinary);
        private static readonly MsSqlTypeDescriptor _text = new MsSqlTypeDescriptor("text", SqlDbType.Text);
        private static readonly MsSqlTypeDescriptor _ntext = new MsSqlTypeDescriptor("ntext", SqlDbType.NText);
        private static readonly MsSqlTypeDescriptor _image = new MsSqlTypeDescriptor("image", SqlDbType.Image);
        private static readonly MsSqlTypeDescriptor _xml = new MsSqlTypeDescriptor("xml", SqlDbType.Xml);
        private static readonly MsSqlTypeDescriptor _hierarchyId = new MsSqlTypeDescriptor("hierarchyid", SqlDbType.Udt);
        private static readonly MsSqlTypeDescriptor _geometry = new MsSqlTypeDescriptor("geometry", SqlDbType.Udt);
        private static readonly MsSqlTypeDescriptor _geography = new MsSqlTypeDescriptor("geography", SqlDbType.Udt);
        #endregion

        #region interface
        public static MsSqlTypeDescriptor Bit => _bit;
        public static MsSqlTypeDescriptor DateTime => _dateTime;
        public static MsSqlTypeDescriptor DateTime2 => _dateTime2;
        public static MsSqlTypeDescriptor SmallDateTime => _smalldatetime;
        public static MsSqlTypeDescriptor DateTimeOffset => _dateTimeOffset;
        public static MsSqlTypeDescriptor Date => _date;
        public static MsSqlTypeDescriptor Timestamp => _timestamp;
        public static MsSqlTypeDescriptor Time => _time;
        public static MsSqlTypeDescriptor VarChar => _varchar;
        public static MsSqlTypeDescriptor Char => _char;
        public static MsSqlTypeDescriptor NVarChar => _nvarchar;
        public static MsSqlTypeDescriptor NChar => _nchar;
        public static MsSqlTypeDescriptor TinyInt => _tinyint;
        public static MsSqlTypeDescriptor SmallInt => _smallInt;
        public static MsSqlTypeDescriptor Int => _int;
        public static MsSqlTypeDescriptor BigInt => _bigInt;
        public static MsSqlTypeDescriptor Money => _money;
        public static MsSqlTypeDescriptor Numeric => _numeric;
        public static MsSqlTypeDescriptor Decimal => _decimal;
        public static MsSqlTypeDescriptor Float => _float;
        public static MsSqlTypeDescriptor UniqueIdentifier => _uniqueIdentifier;
        public static MsSqlTypeDescriptor Binary => _binary;
        public static MsSqlTypeDescriptor VarBinary => _varbinary;
        public static MsSqlTypeDescriptor Text => _text;
        public static MsSqlTypeDescriptor NText => _ntext;
        public static MsSqlTypeDescriptor Image => _image;
        public static MsSqlTypeDescriptor Xml => _xml;
        public static MsSqlTypeDescriptor HierarchyId => _hierarchyId;
        public static MsSqlTypeDescriptor Geometry => _geometry;
        public static MsSqlTypeDescriptor Geography => _geography;
        #endregion

        #region methods
        public static MsSqlTypeDescriptor? GetTypeDescriptor(string typeName)
        {
            if (_typeMap.ContainsKey(typeName))
                return new MsSqlTypeDescriptor(typeName, _typeMap[typeName]);
            if (_otherTypes.Contains(typeName))
                return new MsSqlTypeDescriptor(typeName, SqlDbType.Udt);
            return null;
        }
        #endregion
    }
}
