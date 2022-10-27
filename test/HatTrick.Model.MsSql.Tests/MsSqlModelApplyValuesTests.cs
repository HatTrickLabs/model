using FluentAssertions;
using System.Data;
using Xunit;

namespace HatTrick.Model.MsSql.Tests
{
    public class MsSqlModelApplyValuesTests : UnitTestBase 
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
        public void Can_apply_column_properties()
        {
            //given
            var model = GetModel();

            //when
            model.Schemas["Schema_1"].Tables["Table_1"].Columns["Column_1"].Apply((c) =>
            {
                c.Name = "Not_Column_1";
                c.MaxLength = 50;
                c.SqlType = SqlDbType.VarChar;
                c.SqlTypeName = "varchar";
                c.IsNullable = true;
                c.Meta = "foobar";
            });

            var resolved = model.Schemas["Schema_1"].Tables["Table_1"].Columns["Column_1"];

            //then
            resolved.Name.Should().Be("Not_Column_1");
            resolved.MaxLength.Should().Be(50);
            resolved.SqlType.Should().Be(SqlDbType.VarChar);
            resolved.SqlTypeName.Should().Be("varchar");
            resolved.IsNullable.Should().BeTrue();
            resolved.Meta.Should().Be("foobar");
        }

        [Fact]
        public void Can_apply_table_properties()
        {
            //given
            var model = GetModel();

            //when
            model.Schemas["Schema_1"].Tables["Table_1"].Apply((c) =>
            {
                c.Name = "Not_Table_1";
                c.Meta = "foobar";
            });

            var resolved = model.Schemas["Schema_1"].Tables["Table_1"];

            //then
            resolved.Name.Should().Be("Not_Table_1");
            resolved.Meta.Should().Be("foobar");
        }
    }
}
