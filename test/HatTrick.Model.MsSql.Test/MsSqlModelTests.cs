namespace HatTrick.Model.MsSql.Tests;

public class MsSqlModelTests
{
    private MsSqlModel GetModel()
    {
        var model = new MsSqlModel();

        var schema = new MsSqlSchema { Identifier = "Schema_1", Name = "Schema_1" };
        model.Schemas.Add(schema);
        model.Schemas.Add(new MsSqlSchema { Identifier = "Schema_2", Name = "Schema_2" });

        var table = new MsSqlTable { Identifier = "Table_1", Name = "Table_1" };
        schema.Tables.Add(table);
        schema.Tables.Add(new MsSqlTable { Identifier = "Table_2", Name = "Table_2" });

        table.Columns.Add(new MsSqlTableColumn { Identifier = "Column_1", Name = "Column_1", IsIdentity = true });
        table.Columns.Add(new MsSqlTableColumn { Identifier = "Column_2", Name = "Column_2", IsIdentity = false });
        table.Columns.Add(new MsSqlTableColumn { Identifier = "Column_3", Name = "Column_3", IsIdentity = false });

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
        Assert.Throws<DatabaseObjectNotFoundException<MsSqlSchema>>(() => model.Schemas["Schema_1"]);
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
        model.Schemas["Schema_1"].Tables.Should().HaveCount(1)
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
        model.Schemas["Schema_1"].Tables["Table_1"].Columns.Should().HaveCount(2)
            .And.OnlyHaveUniqueItems()
            .And.AllBeAssignableTo<MsSqlColumn>();
    }
}
