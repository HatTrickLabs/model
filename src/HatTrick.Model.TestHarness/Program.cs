using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HatTrick.Model.MsSql;

namespace HatTrick.Model.TestHarness
{
    class Program
    {
        /*********************************************************************************/
        /* To run this program as is, you must create an accessible sql server database 
        /* named MsSqlTest and run the \DbScripts\MsSqlTest.sql script located in this
        /* project to create the sample schema.  Or just change the connection string to point
        /* to any accessible Ms Sql Server Db and modify the overrides to work with your schema. 
        /*********************************************************************************/
        static void Main(string[] args)
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            //init builder
            MsSqlModelBuilder builder = new MsSqlModelBuilder("server=localhost;initial catalog=MsSqlTest;integrated security=true");

            //provide on error action callback
            bool error = false;
            builder.OnError += (ex) =>
            {
                error = true;
                Console.WriteLine($"Error!! {ex.Message}");
            };

            //build model
            MsSqlModel sqlModel = builder.Build();

            if (!error)
            {
                //example remove an element
                sqlModel.Schemas["dbo"].Tables.Remove("TypeCode");

                //example override and element
                sqlModel.Schemas["dbo"].Tables["Address"].Columns["Line2"].Apply((c) =>
                {
                    c.Name = "AddressSet";
                    c.MaxLength = 50;
                    c.SqlType = SqlDbType.VarChar;
                    c.SqlTypeName = "varchar";
                    c.IsNullable = false;
                });
            }

            sw.Stop();
            Console.WriteLine($"processed in {sw.ElapsedMilliseconds} milliseconds.  Press [Enter] to exit.");
            Console.ReadLine();
        }
    }
}
