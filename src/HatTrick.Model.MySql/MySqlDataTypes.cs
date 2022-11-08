using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System;

namespace HatTrick.Model.MySql
{
    public static class MySqlDataTypes
    {
        #region internals
        private static readonly Dictionary<string, MySqlDbType> _typeMap = new Dictionary<string, MySqlDbType>(StringComparer.OrdinalIgnoreCase)
        {
            { "bit", MySqlDbType.Bit },
            { "datetime", MySqlDbType.DateTime },
            { "date", MySqlDbType.Date },
            { "timestamp", MySqlDbType.Timestamp },
            { "time", MySqlDbType.Time },
            { "year", MySqlDbType.Year },
            { "varchar", MySqlDbType.VarChar },
            { "char", MySqlDbType.String },
            { "blob", MySqlDbType.Blob },
            { "tinyblob", MySqlDbType.TinyBlob },
            { "mediumblob", MySqlDbType.MediumBlob },
            { "longblob", MySqlDbType.LongBlob },
            { "tinyint", MySqlDbType.Byte },
            { "smallint", MySqlDbType.Int16 },
            { "mediumint", MySqlDbType.Int24 },
            { "int", MySqlDbType.Int32 },
            { "bigint", MySqlDbType.Int64 },
            { "double", MySqlDbType.Double },
            { "decimal", MySqlDbType.Decimal },
            { "float", MySqlDbType.Float },
            { "binary", MySqlDbType.Binary },
            { "varbinary", MySqlDbType.VarBinary },
            { "tinytext", MySqlDbType.TinyText },
            { "text", MySqlDbType.Text },
            { "mediumtext", MySqlDbType.MediumText },
            { "longtext", MySqlDbType.LongText },
            { "enum", MySqlDbType.Enum },
            { "set", MySqlDbType.Set },
        };

        private static readonly MySqlTypeDescriptor _bit = new MySqlTypeDescriptor("bit", MySqlDbType.Bit);
        private static readonly MySqlTypeDescriptor _dateTime = new MySqlTypeDescriptor("datetime", MySqlDbType.DateTime);
        private static readonly MySqlTypeDescriptor _date = new MySqlTypeDescriptor("date", MySqlDbType.Date);
        private static readonly MySqlTypeDescriptor _timestamp = new MySqlTypeDescriptor("timestamp", MySqlDbType.Timestamp);
        private static readonly MySqlTypeDescriptor _time = new MySqlTypeDescriptor("time", MySqlDbType.Time);
        private static readonly MySqlTypeDescriptor _year = new MySqlTypeDescriptor("year", MySqlDbType.Year);
        private static readonly MySqlTypeDescriptor _varchar = new MySqlTypeDescriptor("varchar", MySqlDbType.VarChar);
        private static readonly MySqlTypeDescriptor _char = new MySqlTypeDescriptor("char", MySqlDbType.String);
        private static readonly MySqlTypeDescriptor _blob = new MySqlTypeDescriptor("blob", MySqlDbType.Blob);
        private static readonly MySqlTypeDescriptor _tinyblob = new MySqlTypeDescriptor("tinyblob", MySqlDbType.TinyBlob);
        private static readonly MySqlTypeDescriptor _mediumblob = new MySqlTypeDescriptor("mediumblob", MySqlDbType.MediumBlob);
        private static readonly MySqlTypeDescriptor _longblob = new MySqlTypeDescriptor("longblob", MySqlDbType.LongBlob);
        private static readonly MySqlTypeDescriptor _byte = new MySqlTypeDescriptor("tinyint", MySqlDbType.Byte);
        private static readonly MySqlTypeDescriptor _int16 = new MySqlTypeDescriptor("smallint", MySqlDbType.Int16);
        private static readonly MySqlTypeDescriptor _int24 = new MySqlTypeDescriptor("mediumint", MySqlDbType.Int24);
        private static readonly MySqlTypeDescriptor _int32 = new MySqlTypeDescriptor("int", MySqlDbType.Int32);
        private static readonly MySqlTypeDescriptor _int64 = new MySqlTypeDescriptor("bigint", MySqlDbType.Int64);
        private static readonly MySqlTypeDescriptor _double = new MySqlTypeDescriptor("double", MySqlDbType.Double);
        private static readonly MySqlTypeDescriptor _decimal = new MySqlTypeDescriptor("decimal", MySqlDbType.Decimal);
        private static readonly MySqlTypeDescriptor _float = new MySqlTypeDescriptor("float", MySqlDbType.Float);
        private static readonly MySqlTypeDescriptor _binary = new MySqlTypeDescriptor("binary", MySqlDbType.Binary);
        private static readonly MySqlTypeDescriptor _varbinary = new MySqlTypeDescriptor("varbinary", MySqlDbType.VarBinary);
        private static readonly MySqlTypeDescriptor _tinytext = new MySqlTypeDescriptor("tinytext", MySqlDbType.TinyText);
        private static readonly MySqlTypeDescriptor _text = new MySqlTypeDescriptor("text", MySqlDbType.Text);
        private static readonly MySqlTypeDescriptor _mediumtext = new MySqlTypeDescriptor("mediumtext", MySqlDbType.MediumText);
        private static readonly MySqlTypeDescriptor _longtext = new MySqlTypeDescriptor("longtext", MySqlDbType.LongText);
        private static readonly MySqlTypeDescriptor _enum = new MySqlTypeDescriptor("enum", MySqlDbType.Enum);
        private static readonly MySqlTypeDescriptor _set = new MySqlTypeDescriptor("set", MySqlDbType.Set);
        #endregion

        #region interface
        public static MySqlTypeDescriptor Bit => _bit;
        public static MySqlTypeDescriptor DateTime => _dateTime;
        public static MySqlTypeDescriptor Date => _date;
        public static MySqlTypeDescriptor Timestamp => _timestamp;
        public static MySqlTypeDescriptor Time => _time;
        public static MySqlTypeDescriptor Year => _year;
        public static MySqlTypeDescriptor VarChar => _varchar;
        public static MySqlTypeDescriptor Char => _char;
        public static MySqlTypeDescriptor Blob => _blob;
        public static MySqlTypeDescriptor TinyBlob => _tinyblob;
        public static MySqlTypeDescriptor MediumBlob => _mediumblob;
        public static MySqlTypeDescriptor LongBlob => _longblob;
        public static MySqlTypeDescriptor Byte => _byte;
        public static MySqlTypeDescriptor Int16 => _int16;
        public static MySqlTypeDescriptor Int24 => _int24;
        public static MySqlTypeDescriptor Int32 => _int32;
        public static MySqlTypeDescriptor Int64 => _int64;
        public static MySqlTypeDescriptor Double => _double;
        public static MySqlTypeDescriptor Decimal => _decimal;
        public static MySqlTypeDescriptor Float => _float;
        public static MySqlTypeDescriptor Binary => _binary;
        public static MySqlTypeDescriptor VarBinary => _varbinary;
        public static MySqlTypeDescriptor TinyText => _tinytext;
        public static MySqlTypeDescriptor Text => _text;
        public static MySqlTypeDescriptor MediumText => _mediumtext;
        public static MySqlTypeDescriptor LongText => _longtext;
        public static MySqlTypeDescriptor Enum => _enum;
        public static MySqlTypeDescriptor Set => _set;
        #endregion

        #region methods
        public static MySqlTypeDescriptor? GetTypeDescriptor(string typeName)
        {
            if (_typeMap.ContainsKey(typeName))
                return new MySqlTypeDescriptor(typeName, _typeMap[typeName]);
            return null;
        }
        #endregion
    }
}
