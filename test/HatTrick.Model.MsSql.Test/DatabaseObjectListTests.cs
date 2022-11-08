namespace HatTrick.Model.MsSql.Tests;

public class DatabaseObjectListTests
{
    private MsSqlModel GetModel()
    {
        var model = new MsSqlModel();

        var schema = new MsSqlSchema { Name = "dbo" };
        model.Schemas.Add(schema);

        var table = new MsSqlTable { Name = "Table_1" };
        schema.Tables.Add(table);

        var column = new MsSqlTableColumn { Name = "Column_1" };
        table.Columns.Add(column);

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

