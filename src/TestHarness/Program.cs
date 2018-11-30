using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HatTrick.Model.MsSql;

namespace TestHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            MsSqlModelBuilder builder = new MsSqlModelBuilder("server=localhost;initial catalog=CQGeneticResponse;integrated security=true");
            MsSqlModel sqlModel = builder.Build();

            sqlModel.Schemas.Remove("els");

            sqlModel.Schemas["arc"].Tables["Genotype"].Columns["HGVS"].Apply((c) =>
            {
                c.MaxLength = 100;
                c.SqlType = SqlDbType.NVarChar;
                c.SqlTypeName = "nvarchar";
                c.IsNullable = false;
            });
        }
    }
}
