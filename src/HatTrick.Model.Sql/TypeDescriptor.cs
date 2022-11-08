using System;
using System.Data;

namespace HatTrick.Model.Sql
{
    public abstract class TypeDescriptor<T> : IEquatable<TypeDescriptor<T>>
    {
        public T DbType { get; private set; }
        public string DbTypeName { get; private set; }

        protected TypeDescriptor(string dbTypeName, T dbType)
        {
            DbTypeName = dbTypeName;
            DbType = dbType;
        }

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

            return true;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                const int multiplier = 16777619;

                int hash = base.GetHashCode();
                hash = (hash * multiplier) ^ DbTypeName.GetHashCode();
                return hash;
            }
        }
    }
}
