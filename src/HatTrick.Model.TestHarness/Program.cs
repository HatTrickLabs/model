using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HatTrick.Model.MsSql;
using HatTrick.Reflection;

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
                TestResolveObjects(sqlModel);
                TestObjectValueOverrides(sqlModel);
                TestApplyObjectMeta(sqlModel);
                TestRemoveObjects(sqlModel);
            }

            sw.Stop();
            Console.WriteLine($"processed in {sw.ElapsedMilliseconds} milliseconds.  Press [Enter] to exit.");
            Console.ReadLine();
        }

        static void TestResolveObjects(MsSqlModel model)
        {
            //walk the dictionary stack
            MsSqlTable person1 = model.Schemas["dbo"].Tables["Person"];
            MsSqlColumn firstName1 = person1.Columns["FirstName"];
            MsSqlColumn zip1 = model.Schemas["dbo"].Tables["Address"].Columns["Zip"];
            SqlDbType birthDateType1 = model.Schemas["dbo"].Tables["Person"].Columns["BirthDate"].SqlType;

            //resolve items
            MsSqlModel mdl = model.ResolveItem("/") as MsSqlModel;
            MsSqlTable person2 = model.ResolveItem("dbo.Person") as MsSqlTable;
            MsSqlColumn firstName2 = model.ResolveItem("dbo.Person.FirstName") as MsSqlColumn;
            MsSqlColumn zip2 = model.ResolveItem("dbo.Address.Zip") as MsSqlColumn;
            SqlDbType birthDateType2 = (model.ResolveItem("dbo.Person.BirthDate") as MsSqlColumn).SqlType;

            //not found
            var isNull = model.ResolveItem("dbo.Address.ABCCC");
        }

        static void TestObjectValueOverrides(MsSqlModel model)
        {
            //example override and element
            model.Schemas["dbo"].Tables["Address"].Columns["Line2"].Apply((c) =>
            {
                c.Name = "AddressSet";
                c.MaxLength = 50;
                c.SqlType = SqlDbType.VarChar;
                c.SqlTypeName = "varchar";
                c.IsNullable = false;
            });

            model.Schemas["dbo"].Tables["Purchase"].Apply((t) =>
            {
                t.Name = "PurchaseSet";
            });
        }

        static void TestApplyObjectMeta(MsSqlModel model)
        {
            //walking the dictionary stack
            model.Schemas["dbo"].Tables["Address"].Columns["AddressType"].Meta = "code-gen-type=AddressTypeCode";

            //or resolve by expression
            model.ResolveItem("dbo.Address.AddressType").Meta = "code-gen-type=AddressTypeCode";
        }

        static void TestRemoveObjects(MsSqlModel model)
        {
            //remove entire schema
            model.Schemas.Remove("sec");

            //remove table
            model.Schemas["dbo"].Tables.Remove("TypeCode");
        }
    }
}
