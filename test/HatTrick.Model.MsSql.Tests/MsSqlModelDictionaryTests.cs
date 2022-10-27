using FluentAssertions;
using HatTrick.Model.Sql;
using NSubstitute;
using NSubstitute.Extensions;
using System;
using Xunit;

namespace HatTrick.Model.MsSql.Tests
{
    public class MsSqlModelDictionaryTests : UnitTestBase
    {
        private MsSqlModel GetModel()
        {
            var model = new MsSqlModel();

            var schema = new MsSqlSchema { Name = "dbo" };
            model.Schemas.Add(schema.Name, schema);

            var table = new MsSqlTable { Name = "Table_1" };
            schema.Tables.Add(table.Name, table);

            var column = new MsSqlTableColumn { Name = "Column_1" };
            table.Columns.Add(column.Name, column);

            return model;
        }

        [Fact]
        public void Can_access_schema_from_model()
        {
            //given
            var model = GetModel();

            //when & then
            model.Schemas["dbo"].Should().NotBeNull()
                .And.BeOfType<MsSqlSchema>()
                .Which.Name.Should().Be("dbo");
        }

        [Fact]
        public void Can_access_table_from_model()
        {
            //given
            var model = GetModel();

            //when & then
            model.Schemas["dbo"].Tables["Table_1"].Should().NotBeNull()
                .And.BeOfType<MsSqlTable>()
                .Which.Name.Should().Be("Table_1");
        }

        [Fact]
        public void Can_access_column_from_model()
        {
            //given
            var model = GetModel();

            //when & then
            model.Schemas["dbo"].Tables["Table_1"].Columns["Column_1"].Should().NotBeNull()
                .And.BeAssignableTo<MsSqlColumn>()
                .Which.Name.Should().Be("Column_1");
        }
    }
}
