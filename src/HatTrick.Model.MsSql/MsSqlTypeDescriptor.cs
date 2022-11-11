using HatTrick.Model.Sql;
using System.Data;

namespace HatTrick.Model.MsSql
{
    public class MsSqlTypeDescriptor : TypeDescriptor<SqlDbType>
    {
        protected MsSqlTypeDescriptor(SqlDbType dbType, string dbTypeName, byte? precision, byte? scale, long? maxLength) 
            : base(dbType, dbTypeName, precision, scale, maxLength)
        {

        }

        public static MsSqlTypeDescriptor Create(SqlDbType dbType, string typeName, byte? precision, byte? scale, long? maxLength)
            => new MsSqlTypeDescriptor(dbType, typeName, precision, scale, maxLength);
    }
}
