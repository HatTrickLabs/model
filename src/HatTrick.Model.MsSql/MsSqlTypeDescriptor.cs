using HatTrick.Model.Sql;
using System.Data;

namespace HatTrick.Model.MsSql
{
    public class MsSqlTypeDescriptor : TypeDescriptor<SqlDbType>
    {
        public MsSqlTypeDescriptor(string dbTypeName, SqlDbType dbType) : base(dbTypeName, dbType)
        {

        }
    }
}
