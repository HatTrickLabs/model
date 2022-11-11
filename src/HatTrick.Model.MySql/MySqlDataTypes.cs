using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System;
using System.Linq;

namespace HatTrick.Model.MySql
{
    public static class MySqlDataTypes
    {
        #region internals
        private static readonly MySqlTypeDescriptor _dateTime = MySqlTypeDescriptor.Create(MySqlDbType.DateTime, "datetime", "datetime", null, null, null, null);
        private static readonly MySqlTypeDescriptor _date = MySqlTypeDescriptor.Create(MySqlDbType.Date, "date", "date", null, null, null, null);
        private static readonly MySqlTypeDescriptor _timestamp = MySqlTypeDescriptor.Create(MySqlDbType.Timestamp, "timestamp", "timestamp", null, null, null, null);
        private static readonly MySqlTypeDescriptor _time = MySqlTypeDescriptor.Create(MySqlDbType.Time, "time", "time", null, null, null, null);
        private static readonly MySqlTypeDescriptor _year = MySqlTypeDescriptor.Create(MySqlDbType.Year, "year", "year", null, null, null, null);
        private static readonly MySqlTypeDescriptor _tinyInt = MySqlTypeDescriptor.Create(MySqlDbType.Byte, "tinyint", "tinyint", 3, 0, null, null);
        private static readonly MySqlTypeDescriptor _smallInt = MySqlTypeDescriptor.Create(MySqlDbType.Int16, "smallint", "smallint", 5, 0, null, null);
        private static readonly MySqlTypeDescriptor _mediumInt = MySqlTypeDescriptor.Create(MySqlDbType.Int24, "mediumint", "mediumint", 7, 0, null, null);
        private static readonly MySqlTypeDescriptor _int = MySqlTypeDescriptor.Create(MySqlDbType.Int32, "int", "int", 10, 0, null, null);
        private static readonly MySqlTypeDescriptor _bigInt = MySqlTypeDescriptor.Create(MySqlDbType.Int64, "bigint", "bigint", 19, 0, null, null);
        private static readonly MySqlTypeDescriptor _double = MySqlTypeDescriptor.Create(MySqlDbType.Double, "double", "double", 22, null, null, null);
        private static readonly MySqlTypeDescriptor _float = MySqlTypeDescriptor.Create(MySqlDbType.Float, "float", "float", 12, null, null, null);
        private static readonly MySqlTypeDescriptor _binary = MySqlTypeDescriptor.Create(MySqlDbType.Binary, "binary", "binary(1)", null, null, 1, 1);
        private static readonly MySqlTypeDescriptor _tinyText = MySqlTypeDescriptor.Create(MySqlDbType.TinyText, "tinytext", "tinytext", null, null, 255, 255);
        private static readonly MySqlTypeDescriptor _text = MySqlTypeDescriptor.Create(MySqlDbType.Text, "text", "text", null, null, 65535, 65535);
        private static readonly MySqlTypeDescriptor _mediumText = MySqlTypeDescriptor.Create(MySqlDbType.MediumText, "mediumtext", "mediumtext", null, null, 16777215, 16777215);
        private static readonly MySqlTypeDescriptor _longText = MySqlTypeDescriptor.Create(MySqlDbType.LongText, "longtext", "longtext", null, null, 4294967295, 4294967295);
        private static readonly MySqlTypeDescriptor _blob = MySqlTypeDescriptor.Create(MySqlDbType.Blob, "blob", "blob", null, null, 65535, 65535);
        private static readonly MySqlTypeDescriptor _tinyBlob = MySqlTypeDescriptor.Create(MySqlDbType.TinyBlob, "tinyblob", "tinyblob", null, null, 255, 255);
        private static readonly MySqlTypeDescriptor _mediumBlob = MySqlTypeDescriptor.Create(MySqlDbType.MediumBlob, "mediumblob", "mediumblob", null, null, 16777215, 16777215);
        private static readonly MySqlTypeDescriptor _longBlob = MySqlTypeDescriptor.Create(MySqlDbType.LongBlob, "longblob", "longblob", null, null, 4294967295, 4294967295);
        private static readonly MySqlTypeDescriptor _json = MySqlTypeDescriptor.Create(MySqlDbType.JSON, "json", "json", null, null, null, null);

        private static readonly Dictionary<string, Func<string?, byte?, byte?, long?, long?, MySqlTypeDescriptor>> _typeMap = new Dictionary<string, Func<string?, byte?, byte?, long?, long?, MySqlTypeDescriptor>>(StringComparer.OrdinalIgnoreCase)
        {
            { "datetime", (_,_,_,_,_) => _dateTime },
            { "date", (_,_,_,_,_) => _date },
            { "timestamp", (_,_,_,_,_) => _timestamp },
            { "time", (_,_,_,_,_) => _time },
            { "year", (_,_,_,_,_) => _year },
            { "tinyint", (_,_,_,_,_) => _tinyInt },
            { "smallint", (_,_,_,_,_) => _smallInt },
            { "mediumint", (_,_,_,_,_) => _mediumInt },
            { "int", (_,_,_,_,_) => _int },
            { "bigint", (_,_,_,_,_) => _bigInt },
            { "double", (_,_,_,_,_) => _double },
            { "float", (_,_,_,_,_) => _float },
            { "binary", (_,_,_,_,_) => _binary },
            { "tinytext", (_,_,_,_,_) => _tinyText },
            { "text", (_,_,_,_,_) => _text },
            { "mediumtext", (_,_,_,_,_) => _mediumText },
            { "longtext", (_,_,_,_,_) => _longText },
            { "blob", (_,_,_,_,_) => _blob },
            { "tinyblob", (_,_,_,_,_) => _tinyBlob },
            { "mediumblob", (_,_,_,_,_) => _mediumBlob },
            { "longblob", (_,_,_,_,_) => _longBlob },
            { "json", (_,_,_,_,_) => _json },
            { "bit", (_,p,_,_,_) => MySqlTypeDescriptor.Create(MySqlDbType.Bit, "bit", $"bit({p})", p, null, null, null) },
            { "varchar", (_,_,_,l,_) => MySqlTypeDescriptor.Create(MySqlDbType.VarChar, "varchar", $"varchar({l})", null, null, l, 4 * l) },
            { "char", (_,_,_,l,_) => MySqlTypeDescriptor.Create(MySqlDbType.String, "char", $"char({l})", null, null, l, 4 * l) },
            { "decimal", (_,p,s,_,_) => MySqlTypeDescriptor.Create(MySqlDbType.Decimal, "decimal", $"decimal({p},{s})", p, s, null, null) },
            { "varbinary", (_,_,_,l,_) => MySqlTypeDescriptor.Create(MySqlDbType.VarBinary,$"varbinary",  $"varbinary({l})", null, null, l, l) },
            { "enum", (ct,_,_,l,o) => MySqlTypeDescriptor.Create(MySqlDbType.Enum, $"enum", ct, null, null, l, o) },
            { "set", (ct,_,_,l,o) => MySqlTypeDescriptor.Create(MySqlDbType.Set, $"set", ct, null, null, l, o) },
        };
        #endregion

        #region interface
        public static MySqlTypeDescriptor DateTime => _typeMap["dateTime"](null, null, null, null, null);
        public static MySqlTypeDescriptor Date => _typeMap["date"](null, null, null, null, null);
        public static MySqlTypeDescriptor Timestamp => _typeMap["timestamp"](null, null, null, null, null);
        public static MySqlTypeDescriptor Time => _typeMap["time"](null, null, null, null, null);
        public static MySqlTypeDescriptor Year => _typeMap["year"](null, null, null, null, null);
        public static MySqlTypeDescriptor Blob => _typeMap["blob"](null, null, null, null, null);
        public static MySqlTypeDescriptor TinyBlob => _typeMap["tinyblob"](null, null, null, null, null);
        public static MySqlTypeDescriptor MediumBlob => _typeMap["mediumblob"](null, null, null, null, null);
        public static MySqlTypeDescriptor LongBlob => _typeMap["longblob"](null, null, null, null, null);
        public static MySqlTypeDescriptor Byte => _typeMap["tinyint"](null, null, null, null, null);
        public static MySqlTypeDescriptor Int16 => _typeMap["smallint"](null, null, null, null, null);
        public static MySqlTypeDescriptor Int24 => _typeMap["mediumint"](null, null, null, null, null);
        public static MySqlTypeDescriptor Int32 => _typeMap["int"](null, null, null, null, null);
        public static MySqlTypeDescriptor Int64 => _typeMap["long"](null, null, null, null, null);
        public static MySqlTypeDescriptor Double => _typeMap["double"](null, null, null, null, null);
        public static MySqlTypeDescriptor Float => _typeMap["float"](null, null, null, null, null);
        public static MySqlTypeDescriptor Binary => _typeMap["binary"](null, null, null, null, null);
        public static MySqlTypeDescriptor TinyText => _typeMap["tinytext"](null, null, null, null, null);
        public static MySqlTypeDescriptor Text => _typeMap["text"](null, null, null, null, null);
        public static MySqlTypeDescriptor MediumText => _typeMap["mediumtext"](null, null, null, null, null);
        public static MySqlTypeDescriptor LongText => _typeMap["longtext"](null, null, null, null, null);
        public static MySqlTypeDescriptor JSON => _typeMap["json"](null, null, null, null, null);
        #endregion

        #region methods
        public static MySqlTypeDescriptor Bit(byte precision) => _typeMap["bit"](string.Empty, precision, null, null, null);
        public static MySqlTypeDescriptor VarChar(long maxLength) => _typeMap["varchar"](string.Empty, null, null, maxLength, 4 * maxLength);
        public static MySqlTypeDescriptor Char(long maxLength) => _typeMap["char"](string.Empty, null, null, maxLength, 4 * maxLength);
        public static MySqlTypeDescriptor Decimal(byte precision, byte scale) => _typeMap["decimal"](string.Empty, precision, scale, null, null);
        public static MySqlTypeDescriptor VarBinary(long maxLength) => _typeMap["varbinary"](string.Empty, null, null, maxLength, maxLength);
        public static MySqlTypeDescriptor Enum(params string[] values) => _typeMap["enum"]($"enum({string.Join(",", values.Select(v => $"'{v}'"))})", null, null, null, null);
        public static MySqlTypeDescriptor Set(params string[] values) => _typeMap["set"]($"set({string.Join(",", values.Select(v => $"'{v}'"))})", null, null, null, null);

        public static MySqlTypeDescriptor? GetTypeDescriptor(string typeName, string? columnType, byte? precision, byte? scale, long? maxLength, long? characterOctetLength)
            => _typeMap.ContainsKey(typeName) ? _typeMap[typeName](columnType, precision, scale, maxLength, characterOctetLength) : null;
        #endregion

        #region  classes
        private record Attributes
        {
            public string? TypeName { get; set; }
            public string? ColumnType { get; set; }
            public byte? Precision { get; set; }
            public byte? Scale { get; set; }
            public long? MaxLength { get; set; }
            public long? CharacterOctetLength { get; set; }
        }
        #endregion
    }
}
