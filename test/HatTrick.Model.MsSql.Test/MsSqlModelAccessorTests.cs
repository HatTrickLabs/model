namespace HatTrick.Model.MsSql.Tests;

public class MsSqlModelAccessorTests
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
    public void Can_resolve_model()
    {
        //given
        var model = GetModel();
        var accessor = new MsSqlModelAccessor(model);

        //when
        var resolved = accessor.ResolveItem(".");

        //then
        resolved.Should().NotBeNull().And.Be(model);
    }

    [Fact]
    public void Can_resolve_schema()
    {
        //given
        var model = GetModel();
        var accessor = new MsSqlModelAccessor(model);

        //when
        var resolved = accessor.ResolveItem("dbo");

        //then
        resolved.Should().NotBeNull().And.Be(model.Schemas.Single());
    }

    [Fact]
    public void Can_resolve_table()
    {
        //given
        var model = GetModel();
        var accessor = new MsSqlModelAccessor(model);

        //when
        var resolved = accessor.ResolveItem("dbo.Table_1");

        //then
        resolved.Should().NotBeNull().And.Be(model.Schemas.Single().Tables.Single());
    }

    [Fact]
    public void Can_resolve_column()
    {
        //given
        var model = GetModel();
        var accessor = new MsSqlModelAccessor(model);

        //when
        var resolved = accessor.ResolveItem("dbo.Table_1.Column_1");

        //then
        resolved.Should().NotBeNull().And.Be(model.Schemas.Single().Tables.Single().Columns.Single());
    }

    [Fact]
    public void Does_invalid_path_return_null()
    {
        //given
        var model = GetModel();
        var accessor = new MsSqlModelAccessor(model);

        //when
        var resolved = accessor.ResolveItem("dbo.Table_1.Column_XYZ");

        //then
        resolved.Should().BeNull();
    }
}
