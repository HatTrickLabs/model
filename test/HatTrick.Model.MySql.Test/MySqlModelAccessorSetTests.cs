namespace HatTrick.Model.MySql.Tests;

public class MySqlModelAccessorSetTests
{
    private MySqlModel GetModel()
    {
        var model = new MySqlModel();

        var schema = new MySqlSchema { Name = "Schema_1" };
        model.Schemas.Add(schema);
        model.Schemas.Add(new MySqlSchema { Name = "Schema_2" });

        var table = new MySqlTable { Name = "Table_1" };
        schema.Tables.Add(table);
        schema.Tables.Add(new MySqlTable { Name = "Table_2" });

        table.Columns.Add(new MySqlTableColumn { Name = "Column_1", AutoIncrement = true });
        table.Columns.Add(new MySqlTableColumn { Name = "Column_2", AutoIncrement = false });
        table.Columns.Add(new MySqlTableColumn { Name = "Column_3", AutoIncrement = false });

        return model;
    }

    [Fact]
    public void Can_resolve_model()
    {
        //given
        var model = GetModel();
        var accessor = new MySqlModelAccessor(model);

        //when
        var resolved = accessor.ResolveItemSet<MySqlModel>(".");

        //then
        resolved.Should().HaveCount(1);
    }

    [Fact]
    public void Can_resolve_tables_with_table_wildcard()
    {
        //given
        var model = GetModel();
        var accessor = new MySqlModelAccessor(model);

        //when
        var resolved = accessor.ResolveItemSet("Schema_1.*");

        //then
        resolved.Should().HaveCount(2)
            .And.OnlyHaveUniqueItems()
            .And.AllBeAssignableTo<MySqlTable>();
    }

    [Fact]
    public void Can_resolve_columns_with_column_wildcard()
    {
        //given
        var model = GetModel();
        var accessor = new MySqlModelAccessor(model);

        //when
        var resolved = accessor.ResolveItemSet("Schema_1.Table_1.*");

        //then
        resolved.Should().HaveCount(3)
            .And.OnlyHaveUniqueItems()
            .And.AllBeAssignableTo<MySqlColumn>();
    }

    [Fact]
    public void Can_resolve_tables_with_table_wildcard_plus_ending_character()
    {
        //given
        var model = GetModel();
        var accessor = new MySqlModelAccessor(model);

        //when
        var resolved = accessor.ResolveItemSet("Schema_1.*_1");

        //then
        resolved.Should().HaveCount(1)
            .And.AllBeAssignableTo<MySqlTable>();
    }

    [Fact]
    public void Can_resolve_columns_with_column_wildcard_plus_ending_character()
    {
        //given
        var model = GetModel();
        var accessor = new MySqlModelAccessor(model);

        //when
        var resolved = accessor.ResolveItemSet("Schema_1.Table_1.*_1");

        //then
        resolved.Should().HaveCount(1)
            .And.AllBeAssignableTo<MySqlColumn>();
    }

    [Fact]
    public void Can_resolve_tables_with_schema_wildcard()
    {
        //given
        var model = GetModel();
        var accessor = new MySqlModelAccessor(model);

        //when
        var resolved = accessor.ResolveItemSet("*");

        //then
        resolved.Should().HaveCount(2)
            .And.AllBeAssignableTo<MySqlSchema>();
    }

    [Fact]
    public void Can_resolve_tables_with_schema_wildcard_and_table_wildcard()
    {
        //given
        var model = GetModel();
        var accessor = new MySqlModelAccessor(model);

        //when
        var resolved = accessor.ResolveItemSet("*.*");

        //then
        resolved.Should().HaveCount(2)
            .And.OnlyHaveUniqueItems()
            .And.HaveCount(x => resolved.Select(a => a is MySqlTable).Count() == 2);
    }

    [Fact]
    public void Can_resolve_columns_with_schema_name_and_table_wildcard()
    {
        //given
        var model = GetModel();
        var accessor = new MySqlModelAccessor(model);

        //when
        var resolved = accessor.ResolveItemSet("Schema_1.*.Column_1");

        //then
        resolved.Should().HaveCount(1)
            .And.AllBeAssignableTo<MySqlColumn>();
    }

    [Fact]
    public void Can_resolve_columns_with_schema_name_and_table_wildcard_and_column_wildcard_and_column_property_filter()
    {
        //given
        var model = GetModel();
        var accessor = new MySqlModelAccessor(model);

        //when
        var resolved = accessor.ResolveItemSet<MySqlColumn>("Schema_1.*.*", c => c.AutoIncrement);

        //then
        resolved.Should().HaveCount(1)
            .And.AllBeAssignableTo<MySqlColumn>();
    }

    [Fact]
    public void Can_resolve_columns_with_schema_name_and_table_wildcard_and_column_wildcard_and_null_for_column_property_filter()
    {
        //given
        var model = GetModel();
        var accessor = new MySqlModelAccessor(model);

        //when
        var resolved = accessor.ResolveItemSet<MySqlColumn>("Schema_1.*.*", null!);

        //then
        resolved.Should().HaveCount(3)
            .And.OnlyHaveUniqueItems()
            .And.AllBeAssignableTo<MySqlColumn>();
    }

    [Fact]
    public void Can_resolve_tables_with_schema_name_and_complex_table_wildcard()
    {
        //given
        var model = GetModel();
        var accessor = new MySqlModelAccessor(model);

        //when
        var resolved = accessor.ResolveItemSet<MySqlTable>("Schema_1.*_*", null!);

        //then
        resolved.Should().HaveCount(2)
            .And.OnlyHaveUniqueItems()
            .And.AllBeAssignableTo<MySqlTable>();
    }
}
