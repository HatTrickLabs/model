using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace HatTrick.Model.MsSql.Tests
{
    public class MsSqlModelTests : UnitTestBase
    {
        private MsSqlModel GetModel()
        {
            var model = new MsSqlModel();

            var schema = new MsSqlSchema { Name = "Schema_1" };
            model.Schemas.Add(schema.Name, schema);
            model.Schemas.Add("Schema_2", new MsSqlSchema { Name = "Schema_2" });

            var table = new MsSqlTable { Name = "Table_1" };
            schema.Tables.Add(table.Name, table);
            schema.Tables.Add("Table_2", new MsSqlTable { Name = "Table_2" });

            table.Columns.Add("Column_1", new MsSqlTableColumn { Name = "Column_1", IsIdentity = true });
            table.Columns.Add("Column_2", new MsSqlTableColumn { Name = "Column_2", IsIdentity = false });
            table.Columns.Add("Column_3", new MsSqlTableColumn { Name = "Column_3", IsIdentity = false });

            return model;
        }

        [Fact]
        public void Can_remove_schema()
        {
            //given
            var model = GetModel();

            //when
            model.Schemas.Remove("Schema_1");

            //then
            Assert.Throws<KeyNotFoundException>(() => model.Schemas["Schema_1"]);
        }

        [Fact]
        public void Can_remove_table()
        {
            //given
            var model = GetModel();
            var accessor = new MsSqlModelAccessor(model);

            //when
            model.Schemas["Schema_1"].Tables.Remove("Table_1");
            var resolved = accessor.ResolveItemSet("Schema_1.*");

            //then
            model.Schemas["Schema_1"].Tables.Values.Should().HaveCount(1)
                .And.AllBeAssignableTo<MsSqlTable>();
        }

        [Fact]
        public void Can_remove_column()
        {
            //given
            var model = GetModel();
            var accessor = new MsSqlModelAccessor(model);

            //when
            model.Schemas["Schema_1"].Tables["Table_1"].Columns.Remove("Column_1");
            var resolved = accessor.ResolveItemSet<MsSqlColumn>("Schema_1.Table_1");

            //then
            model.Schemas["Schema_1"].Tables["Table_1"].Columns.Values.Should().HaveCount(2)
                .And.OnlyHaveUniqueItems()
                .And.AllBeAssignableTo<MsSqlColumn>();
        }
    }
}
