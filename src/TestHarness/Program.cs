using System;
using System.Collections;
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
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            //init builder
            MsSqlModelBuilder builder = new MsSqlModelBuilder("server=localhost;initial catalog=CQGeneticResponse;integrated security=true");

            //provide on error action callback
            builder.OnError += (ex) =>
            {
                Console.WriteLine($"Error!! {ex.Message}");
            };

            //build model
            MsSqlModel sqlModel = builder.Build();

            //example remove an element
            sqlModel.Schemas.Remove("els");

            //example override and element
            sqlModel.Schemas["arc"].Tables["Genotype"].Columns["HGVS"].Apply((c) =>
            {
                c.MaxLength = 100;
                c.SqlType = SqlDbType.NVarChar;
                c.SqlTypeName = "nvarchar";
                c.IsNullable = false;
            });

            sw.Stop();
            Console.WriteLine($"processed in {sw.ElapsedMilliseconds} milliseconds.  Press [Enter] to exit.");
            Console.ReadLine();
        }
    }
}
