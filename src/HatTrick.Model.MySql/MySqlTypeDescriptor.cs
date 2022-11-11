using HatTrick.Model.Sql;
using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace HatTrick.Model.MySql
{
    public class MySqlTypeDescriptor : TypeDescriptor<MySqlDbType>, IEquatable<MySqlTypeDescriptor>
    {
        public string? ColumnType { get; private set; }
        public long? CharacterOctetLength { get; private set; }

        protected MySqlTypeDescriptor(MySqlDbType mySqlDbType, string dbTypeName, string? columnType, byte? precision, byte? scale, long? maxLength, long? characterOctetLength) 
            : base(mySqlDbType, dbTypeName, precision, scale, maxLength)
        {
            ColumnType = columnType;
            CharacterOctetLength = characterOctetLength;
        }

        public static MySqlTypeDescriptor Create(MySqlDbType mySqlDbType, string dbTypeName, string? columnType, byte? precision, byte? scale, long? maxLength, long? characterOctetLength)
            => new MySqlTypeDescriptor(mySqlDbType, dbTypeName, columnType, precision, scale, maxLength, characterOctetLength);

        public static bool operator ==(MySqlTypeDescriptor obj1, MySqlTypeDescriptor obj2)
            => obj1.Equals(obj2);

        public static bool operator !=(MySqlTypeDescriptor obj1, MySqlTypeDescriptor obj2)
            => !obj1.Equals(obj2);

        public override bool Equals(object? obj)
            => obj is MySqlTypeDescriptor u && Equals(u);

        public bool Equals(MySqlTypeDescriptor? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;

            if (ColumnType is null && obj.ColumnType is not null) return false;
            if (ColumnType is not null && obj.ColumnType is null) return false;
            if (ColumnType is not null && !ColumnType.Equals(obj.ColumnType)) return false;

            if (CharacterOctetLength is null && obj.CharacterOctetLength is not null) return false;
            if (CharacterOctetLength is not null && obj.CharacterOctetLength is null) return false;
            if (CharacterOctetLength is not null && !CharacterOctetLength.Equals(obj.CharacterOctetLength)) return false;

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                const int multiplier = 16777619;

                int hash = base.GetHashCode();
                hash = (hash * multiplier) ^ ColumnType?.GetHashCode() ?? 0;
                hash = (hash * multiplier) ^ CharacterOctetLength?.GetHashCode() ?? 0;
                return hash;
            }
        }
    }
}
