using System;
using System.Data;

namespace HatTrick.Model.Sql
{
    public abstract class TypeDescriptor<T> : IEquatable<TypeDescriptor<T>>
    {
        #region internals
        public T DbType { get; private set; }
        public string DbTypeName { get; private set; }
        public byte? Precision { get; private set; }
        public byte? Scale { get ; private set; }
        public long? MaxLength { get; private set; }
        #endregion

        #region constructors
        protected TypeDescriptor(T dbType, string dbTypeName, byte? precision, byte? scale, long? maxLength)
        {
            DbTypeName = dbTypeName;
            DbType = dbType;
            Precision = precision;
            Scale = scale;
            MaxLength = maxLength;
        }
        #endregion

        #region equality
        public static bool operator ==(TypeDescriptor<T> obj1, TypeDescriptor<T> obj2)
            => obj1.Equals(obj2);

        public static bool operator !=(TypeDescriptor<T> obj1, TypeDescriptor<T> obj2)
            => !obj1.Equals(obj2);

        public override bool Equals(object? obj)
            => obj is TypeDescriptor<T> u && Equals(u);

        public bool Equals(TypeDescriptor<T>? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;

            if (DbTypeName is null && obj.DbTypeName is not null) return false;
            if (DbTypeName is not null && obj.DbTypeName is null) return false;
            if (DbTypeName is not null && !DbTypeName.Equals(obj.DbTypeName)) return false;

            if (Precision is null && obj.Precision is not null) return false;
            if (Precision is not null && obj.Precision is null) return false;
            if (Precision is not null && !Precision.Equals(obj.Precision)) return false;

            if (Scale is null && obj.Scale is not null) return false;
            if (Scale is not null && obj.Scale is null) return false;
            if (Scale is not null && !Scale.Equals(obj.Scale)) return false;

            if (MaxLength is null && obj.MaxLength is not null) return false;
            if (MaxLength is not null && obj.MaxLength is null) return false;
            if (MaxLength is not null && !MaxLength.Equals(obj.MaxLength)) return false;

            return true;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                const int multiplier = 16777619;

                int hash = base.GetHashCode();
                hash = (hash * multiplier) ^ DbTypeName.GetHashCode();
                hash = (hash * multiplier) ^ Precision?.GetHashCode() ?? 0;
                hash = (hash * multiplier) ^ Scale?.GetHashCode() ?? 0;
                hash = (hash * multiplier) ^ MaxLength?.GetHashCode() ?? 0;
                return hash;
            }
        }
        #endregion
    }
}
