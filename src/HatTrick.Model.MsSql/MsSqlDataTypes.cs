using System;
using System.Collections.Generic;
using System.Data;

namespace HatTrick.Model.MsSql
{
    public static class MsSqlDataTypes
    {
        #region internals
        private static readonly MsSqlTypeDescriptor _bit = MsSqlTypeDescriptor.Create(SqlDbType.Bit, "bit", 1, 0, 1);
        private static readonly MsSqlTypeDescriptor _dateTime = MsSqlTypeDescriptor.Create(SqlDbType.DateTime, "datetime", 23, 3, 8);
        private static readonly MsSqlTypeDescriptor _dateTime2 = MsSqlTypeDescriptor.Create(SqlDbType.DateTime2, "datetime2", 27, 7, 8);
        private static readonly MsSqlTypeDescriptor _smallDateTime = MsSqlTypeDescriptor.Create(SqlDbType.SmallDateTime, "smalldatetime", 16, 0, 4);
        private static readonly MsSqlTypeDescriptor _dateTimeOffset = MsSqlTypeDescriptor.Create(SqlDbType.DateTimeOffset, "datetimeoffset", 34, 7, 10);
        private static readonly MsSqlTypeDescriptor _date = MsSqlTypeDescriptor.Create(SqlDbType.Date, "date", 10, 0, 3);
        private static readonly MsSqlTypeDescriptor _timestamp = MsSqlTypeDescriptor.Create(SqlDbType.Timestamp, "timestamp", 0, 0, 8);
        private static readonly MsSqlTypeDescriptor _time = MsSqlTypeDescriptor.Create(SqlDbType.Time, "time", 16, 7, 5);
        private static readonly MsSqlTypeDescriptor _tinyInt = MsSqlTypeDescriptor.Create(SqlDbType.TinyInt, "tinyint", 3, 0, 1);
        private static readonly MsSqlTypeDescriptor _smallInt = MsSqlTypeDescriptor.Create(SqlDbType.SmallInt, "smallint", 5, 0, 2);
        private static readonly MsSqlTypeDescriptor _int = MsSqlTypeDescriptor.Create(SqlDbType.Int, "int", 10, 0, 4);
        private static readonly MsSqlTypeDescriptor _bigInt = MsSqlTypeDescriptor.Create(SqlDbType.BigInt, "bigint", 19, 0, 8);
        private static readonly MsSqlTypeDescriptor _money = MsSqlTypeDescriptor.Create(SqlDbType.Money, "money", 19, 4, 8);
        private static readonly MsSqlTypeDescriptor _smallMoney = MsSqlTypeDescriptor.Create(SqlDbType.SmallMoney, "smallmoney", 10, 4, 4);
        private static readonly MsSqlTypeDescriptor _float = MsSqlTypeDescriptor.Create(SqlDbType.Float, "float", 53, 0, 8);
        private static readonly MsSqlTypeDescriptor _real = MsSqlTypeDescriptor.Create(SqlDbType.Real, "real", 24, 0, 4);
        private static readonly MsSqlTypeDescriptor _uniqueIdentifier = MsSqlTypeDescriptor.Create(SqlDbType.UniqueIdentifier, "uniqueidentifier", 0, 0, 16);
        private static readonly MsSqlTypeDescriptor _text = MsSqlTypeDescriptor.Create(SqlDbType.Text, "text", 0, 0, 16);
        private static readonly MsSqlTypeDescriptor _ntext = MsSqlTypeDescriptor.Create(SqlDbType.NText, "ntext", 0, 0, 16);
        private static readonly MsSqlTypeDescriptor _image = MsSqlTypeDescriptor.Create(SqlDbType.Image,"image",  0, 0, 16);
        private static readonly MsSqlTypeDescriptor _xml = MsSqlTypeDescriptor.Create(SqlDbType.Xml, "xml", 0, 0, -1);
        private static readonly MsSqlTypeDescriptor _hierarchyId = MsSqlTypeDescriptor.Create(SqlDbType.Udt, "hierarchyid", 0, 0, 892);
        private static readonly MsSqlTypeDescriptor _geometry = MsSqlTypeDescriptor.Create(SqlDbType.Udt, "geometry", 0, 0, -1);
        private static readonly MsSqlTypeDescriptor _geography = MsSqlTypeDescriptor.Create(SqlDbType.Udt,"geography",  0, 0, -1);


        private static readonly Dictionary<string, Func<byte?,byte?,long?,MsSqlTypeDescriptor>> _typeMap = new Dictionary<string, Func<byte?, byte?, long?, MsSqlTypeDescriptor>>(StringComparer.OrdinalIgnoreCase)
        {
            { "bit", (_,_,_) => _bit },
            { "datetime", (_,_,_) => _dateTime },
            { "datetime2", (_,_,_) => _dateTime2 },
            { "smalldatetime", (_,_,_) => _smallDateTime },
            { "datetimeoffset",(_,_,_) =>  _dateTimeOffset },
            { "date", (_,_,_) => _date },
            { "timestamp", (_,_,_) => _timestamp},
            { "time", (_,_,_) => _time },
            { "tinyint", (_,_,_) => _tinyInt },
            { "smallint", (_,_,_) => _smallInt },
            { "int", (_,_,_) => _int },
            { "bigint", (_,_,_) => _bigInt },
            { "money", (_,_,_) => _money },
            { "smallmoney", (_,_,_) => _smallMoney},
            { "float", (_,_,_) => _float },
            { "real", (_,_,_) => _real },
            { "uniqueidentifier", (_,_,_) => _uniqueIdentifier },
            { "text", (_,_,_) => _text },
            { "ntext", (_,_,_) => _ntext },
            { "image", (_,_,_) => _image },
            { "xml", (_,_,_) => _xml },
            { "hierarchyid", (_,_,_) => _hierarchyId },
            { "geometry", (_,_,_) => _geometry },
            { "geography", (_,_,_) => _geography },
            { "binary", (_,_,l) => MsSqlTypeDescriptor.Create(SqlDbType.Binary, "binary", 0, 0, l) },
            { "varbinary", (_,_,l) => MsSqlTypeDescriptor.Create(SqlDbType.VarBinary, "varbinary", 0, 0, l) },
            { "varchar", (_,_,l) => MsSqlTypeDescriptor.Create(SqlDbType.VarChar, "varchar", 0, 0, l) },
            { "nvarchar", (_,_,l) => MsSqlTypeDescriptor.Create(SqlDbType.NVarChar,"nvarchar",  0, 0, l)},
            { "char", (_,_,l) => MsSqlTypeDescriptor.Create(SqlDbType.Char, "char", 0, 0, l) },
            { "nchar", (_,_,l) => MsSqlTypeDescriptor.Create(SqlDbType.NChar, "nchar", 0, 0, l) },
            { "numeric", (p,s,_) => MsSqlTypeDescriptor.Create(SqlDbType.Decimal, "decimal", p, s, 5) }, //anything reported as "numeric" is actually a db type of "decimal"
            { "decimal", (p,s,_) => MsSqlTypeDescriptor.Create(SqlDbType.Decimal, "decimal", p, s, 5) },
        };
        #endregion

        #region interface
        public static MsSqlTypeDescriptor Bit => _typeMap["bit"](null, null, null);
        public static MsSqlTypeDescriptor DateTime => _typeMap["datetime"](null, null, null);
        public static MsSqlTypeDescriptor DateTime2 => _typeMap["dateTime2"](null, null, null);
        public static MsSqlTypeDescriptor SmallDateTime => _typeMap["smalldatetime"](null, null, null);
        public static MsSqlTypeDescriptor DateTimeOffset => _typeMap["datetimeoffset"](null, null, null);
        public static MsSqlTypeDescriptor Date => _typeMap["date"](null, null, null);
        public static MsSqlTypeDescriptor Timestamp => _typeMap["timestamp"](null, null, null);
        public static MsSqlTypeDescriptor Time => _typeMap["time"](null, null, null);
        public static MsSqlTypeDescriptor TinyInt => _typeMap["tinyint"](null, null, null);
        public static MsSqlTypeDescriptor SmallInt => _typeMap["smallint"](null, null, null);
        public static MsSqlTypeDescriptor Int => _typeMap["int"](null, null, null);
        public static MsSqlTypeDescriptor BigInt => _typeMap["bigint"](null, null, null);
        public static MsSqlTypeDescriptor Money => _typeMap["money"](null, null, null);
        public static MsSqlTypeDescriptor Float => _typeMap["float"](null, null, null);
        public static MsSqlTypeDescriptor UniqueIdentifier => _typeMap["uniqueidentifier"](null, null, null);
        public static MsSqlTypeDescriptor Text => _typeMap["text"](null, null, null);
        public static MsSqlTypeDescriptor NText => _typeMap["ntext"](null, null, null);
        public static MsSqlTypeDescriptor Image => _typeMap["image"](null, null, null);
        public static MsSqlTypeDescriptor Xml => _typeMap["xml"](null, null, null);
        public static MsSqlTypeDescriptor HierarchyId => _typeMap["hierarchyid"](null, null, null);
        public static MsSqlTypeDescriptor Geometry => _typeMap["geometry"](null, null, null);
        public static MsSqlTypeDescriptor Geography => _typeMap["geography"](null, null, null);
        #endregion

        #region methods
        public static MsSqlTypeDescriptor VarChar(long length) => _typeMap["varchar"](null, null, length);
        public static MsSqlTypeDescriptor Char(long length) => _typeMap["char"](null, null, length);
        public static MsSqlTypeDescriptor NVarChar(long length) => _typeMap["nvarchar"](null, null, length);
        public static MsSqlTypeDescriptor NChar(long length) => _typeMap["nchar"](null, null, length);
        public static MsSqlTypeDescriptor Decimal(byte precision, byte scale) => _typeMap["decimal"](precision, scale, null);
        public static MsSqlTypeDescriptor Binary(long maxLength) => _typeMap["binary"](null, null, maxLength);
        public static MsSqlTypeDescriptor VarBinary(long maxLength) => _typeMap["varbinary"](null, null, maxLength);

        public static MsSqlTypeDescriptor? GetTypeDescriptor(string typeName, byte? precision, byte? scale, long? maxLength)
            => _typeMap.ContainsKey(typeName) ? _typeMap[typeName](precision, scale, maxLength) : null;
        #endregion
    }
}
