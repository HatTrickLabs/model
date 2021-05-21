using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
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
            MsSqlModelBuilder builder = new MsSqlModelBuilder("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=MsSqlDbExTest;Integrated Security=true;Connect Timeout=5");
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
                TestResolveObject(sqlModel);
                TestResolveObjectSet(sqlModel);
                TestObjectValueOverrides(sqlModel);
                TestApplyObjectMeta(sqlModel);
                TestRemoveObjects(sqlModel);
            }

            sw.Stop();
            Console.WriteLine($"processed in {sw.ElapsedMilliseconds} milliseconds.  Press [Enter] to exit.");
            Console.ReadLine();
        }

        static void TestResolveObject(MsSqlModel model)
        {
            //walk the dictionary stack
            //walk the dictionary stack
            MsSqlTable person1 = model.Schemas["dbo"].Tables["Person"];
            MsSqlColumn firstName1 = person1.Columns["FirstName"];
            MsSqlColumn zip1 = model.Schemas["dbo"].Tables["Address"].Columns["Zip"];
            SqlDbType birthDateType1 = model.Schemas["dbo"].Tables["Person"].Columns["BirthDate"].SqlType;

            //resolve item by path
            SqlModelAccessor accessor = new SqlModelAccessor(model);
            MsSqlModel mdl = accessor.ResolveItem(".") as MsSqlModel;

            MsSqlTable person2 = accessor.ResolveItem("dbo.Person") as MsSqlTable;
            MsSqlColumn firstName2 = accessor.ResolveItem("dbo.Person.FirstName") as MsSqlColumn;
            MsSqlColumn zip2 = accessor.ResolveItem("dbo.Address.Zip") as MsSqlColumn;
            SqlDbType birthDateType2 = (accessor.ResolveItem("dbo.Person.BirthDate") as MsSqlColumn).SqlType;

            //not found
            var isNull = accessor.ResolveItem("dbo.Address.ABCCC");
        }

        static void TestResolveObjectSet(MsSqlModel model)
        {
            //Segment Depth Chart
            //--------------------------
            //[0] Schema
            //[1] Table
            //[1] View
            //[1] Procedure
            //[1] Relationship
            //[2] Index
            //[2] Column(table, view)
            //[2] Parameter
            //[2] TableExtendedProperty
            //[2] ViewExtendedProperty
            //[3] TableColumnExtendedProperty
            //[3] TableColumnExtendedProperty

            var accessor = new SqlModelAccessor(model);

            //resolve the model itself as  IList<MsSqlModel>
            IList<MsSqlModel> set = accessor.ResolveItemSet<MsSqlModel>(".");

            //resolve item set for all objects at depth 1 within the dbo schema (table, view, procedure, relationship)
            IList<INamedMeta> set1 = accessor.ResolveItemSet("dbo.*"); 
            //resolve all columns and indexes within the address table
            IList<INamedMeta> set2 = accessor.ResolveItemSet("dbo.Address.*");

            //resolve all tables within the dbo schema
            IList<MsSqlTable> set3 = accessor.ResolveItemSet<MsSqlTable>("dbo.*");
            //resolve all tables within any schema that start with the letter 'P'
            IList<MsSqlTable> set4 = accessor.ResolveItemSet<MsSqlTable>("*.P*");
            //resolve all tables within the dbo schema that contain the '_' character (association tables)
            IList<MsSqlTable> set5 = accessor.ResolveItemSet<MsSqlTable>("dbo.*_*");

            //resolve all columns within the dbo schema that match: name: Id, IsIdentity: true
            IList<MsSqlColumn> set6 = accessor.ResolveItemSet<MsSqlColumn>("dbo.*.Id", (c) => c.IsIdentity);
            IList<MsSqlColumn> set7 = accessor.ResolveItemSet<MsSqlColumn>("dbo.*.Id", null);

            IList<MsSqlTrigger> set8 = accessor.ResolveItemSet<MsSqlTrigger>("dbo.*.*");
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
            SqlModelAccessor accessor = new SqlModelAccessor(model);
            accessor.ResolveItem("dbo.Address.AddressType").Meta = "code-gen-type=AddressTypeCode";
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
