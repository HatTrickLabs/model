namespace HatTrick.Model.MsSql.Tests;

public class MsSqlModelAccessorSetTests
{
    private MsSqlModel GetModel()
    {
        var model = new MsSqlModel();

        var schema = new MsSqlSchema { Name = "Schema_1" };
        model.Schemas.Add(schema);
        model.Schemas.Add(new MsSqlSchema { Name = "Schema_2" });

        var table = new MsSqlTable { Name = "Table_1" };
        schema.Tables.Add(table);
        schema.Tables.Add(new MsSqlTable { Name = "Table_2" });

        table.Columns.Add(new MsSqlTableColumn { Name = "Column_1", IsIdentity = true });
        table.Columns.Add(new MsSqlTableColumn { Name = "Column_2", IsIdentity = false });
        table.Columns.Add(new MsSqlTableColumn { Name = "Column_3", IsIdentity = false });

        return model;
    }

    [Fact]
    public void Can_resolve_model()
    {
        //given
        var model = GetModel();
        var accessor = new MsSqlModelAccessor(model);

        //when
        var resolved = accessor.ResolveItemSet<MsSqlModel>(".");

        //then
        resolved.Should().HaveCount(1);
    }

    [Fact]
    public void Can_resolve_tables_with_table_wildcard()
    {
        //given
        var model = GetModel();
        var accessor = new MsSqlModelAccessor(model);

        //when
        var resolved = accessor.ResolveItemSet("Schema_1.*");

        //then
        resolved.Should().HaveCount(2)
            .And.OnlyHaveUniqueItems()
            .And.AllBeAssignableTo<MsSqlTable>();
    }

    [Fact]
    public void Can_resolve_columns_with_column_wildcard()
    {
        //given
        var model = GetModel();
        var accessor = new MsSqlModelAccessor(model);

        //when
        var resolved = accessor.ResolveItemSet("Schema_1.Table_1.*");

        //then
        resolved.Should().HaveCount(3)
            .And.OnlyHaveUniqueItems()
            .And.AllBeAssignableTo<MsSqlColumn>();
    }

    [Fact]
    public void Can_resolve_tables_with_table_wildcard_plus_ending_character()
    {
        //given
        var model = GetModel();
        var accessor = new MsSqlModelAccessor(model);

        //when
        var resolved = accessor.ResolveItemSet("Schema_1.*_1");

        //then
        resolved.Should().HaveCount(1)
            .And.AllBeAssignableTo<MsSqlTable>();
    }

    [Fact]
    public void Can_resolve_columns_with_column_wildcard_plus_ending_character()
    {
        //given
        var model = GetModel();
        var accessor = new MsSqlModelAccessor(model);

        //when
        var resolved = accessor.ResolveItemSet("Schema_1.Table_1.*_1");

        //then
        resolved.Should().HaveCount(1)
            .And.AllBeAssignableTo<MsSqlColumn>();
    }

    [Fact]
    public void Can_resolve_tables_with_schema_wildcard()
    {
        //given
        var model = GetModel();
        var accessor = new MsSqlModelAccessor(model);

        //when
        var resolved = accessor.ResolveItemSet("*");

        //then
        resolved.Should().HaveCount(2)
            .And.AllBeAssignableTo<MsSqlSchema>();
    }

    [Fact]
    public void Can_resolve_tables_with_schema_wildcard_and_table_wildcard()
    {
        //given
        var model = GetModel();
        var accessor = new MsSqlModelAccessor(model);

        //when
        var resolved = accessor.ResolveItemSet("*.*");

        //then
        resolved.Should().HaveCount(2)
            .And.OnlyHaveUniqueItems()
            .And.HaveCount(x => resolved.Select(a => a is MsSqlTable).Count() == 2);
    }

    [Fact]
    public void Can_resolve_columns_with_schema_name_and_table_wildcard()
    {
        //given
        var model = GetModel();
        var accessor = new MsSqlModelAccessor(model);

        //when
        var resolved = accessor.ResolveItemSet("Schema_1.*.Column_1");

        //then
        resolved.Should().HaveCount(1)
            .And.AllBeAssignableTo<MsSqlColumn>();
    }

    [Fact]
    public void Can_resolve_columns_with_schema_name_and_table_wildcard_and_column_wildcard_and_column_property_filter()
    {
        //given
        var model = GetModel();
        var accessor = new MsSqlModelAccessor(model);

        //when
        var resolved = accessor.ResolveItemSet<MsSqlColumn>("Schema_1.*.*", c => c.IsIdentity);

        //then
        resolved.Should().HaveCount(1)
            .And.AllBeAssignableTo<MsSqlColumn>();
    }

    [Fact]
    public void Can_resolve_columns_with_schema_name_and_table_wildcard_and_column_wildcard_and_null_for_column_property_filter()
    {
        //given
        var model = GetModel();
        var accessor = new MsSqlModelAccessor(model);

        //when
        var resolved = accessor.ResolveItemSet<MsSqlColumn>("Schema_1.*.*", null!);

        //then
        resolved.Should().HaveCount(3)
            .And.OnlyHaveUniqueItems()
            .And.AllBeAssignableTo<MsSqlColumn>();
    }

    [Fact]
    public void Can_resolve_tables_with_schema_name_and_complex_table_wildcard()
    {
        //given
        var model = GetModel();
        var accessor = new MsSqlModelAccessor(model);

        //when
        var resolved = accessor.ResolveItemSet<MsSqlTable>("Schema_1.*_*", null!);

        //then
        resolved.Should().HaveCount(2)
            .And.OnlyHaveUniqueItems()
            .And.AllBeAssignableTo<MsSqlTable>();
    }

    [Fact]
    public void Can_resolve_columns_with_schema_and_table_wildcards()
    {
        //given
        var model = GetModel();
        var accessor = new MsSqlModelAccessor(model);

        //when
        var resolved = accessor.ResolveItemSet<MsSqlColumn>("*.*.Column_1");

        //then
        resolved.Should().HaveCount(1)
            .And.OnlyHaveUniqueItems()
            .And.AllBeAssignableTo<MsSqlColumn>();
    }
}
