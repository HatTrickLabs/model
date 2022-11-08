using HatTrick.Model.Sql;
using MySql.Data.MySqlClient;
using System;

namespace HatTrick.Model.MySql
{
    public class MySqlTypeDescriptor : TypeDescriptor<MySqlDbType>
    {
        public MySqlTypeDescriptor(string dbTypeName, MySqlDbType dbType) : base(dbTypeName, dbType)
        {

        }
    }
}
