namespace HatTrick.Model.MsSql.Test.Integration;

public class ModelTests : IntegrationTestBase
{
    [Fact]
    public void Does_model_have_correct_number_of_schemas()
    {
        //given
        var result = Model.Schemas;

        //when & then
        result.Should().HaveCount(3)
            .And.OnlyHaveUniqueItems()
            .And.AllBeOfType<MsSqlSchema>();
    }

    [Fact]
    public void Does_model_have_correct_number_of_tables_for_dbo_schema()
    {
        //given
        var result = Model.Schemas["dbo"].Tables;

        //when & then
        result.Should().HaveCount(8)
            .And.OnlyHaveUniqueItems()
            .And.AllBeOfType<MsSqlTable>();
    }

    [Fact]
    public void Does_model_have_correct_number_of_tables_for_sec_schema()
    {
        //given
        var result = Model.Schemas["sec"].Tables;

        //when & then
        result.Should().HaveCount(1)
            .And.OnlyHaveUniqueItems()
            .And.AllBeOfType<MsSqlTable>();
    }

    [Fact]
    public void Does_model_have_correct_number_of_views_for_dbo_schema()
    {
        //given
        var result = Model.Schemas["dbo"].Views;

        //when & then
        result.Should().HaveCount(1)
            .And.OnlyHaveUniqueItems()
            .And.AllBeOfType<MsSqlView>();
    }

    [Fact]
    public void Does_model_have_correct_number_of_stored_procedures_for_dbo_schema()
    {
        //given
        var result = Model.Schemas["dbo"].Procedures;

        //when & then
        result.Should().HaveCount(4)
            .And.OnlyHaveUniqueItems()
            .And.AllBeOfType<MsSqlProcedure>();
    }

    [Fact]
    public void Does_model_have_correct_number_of_parameters_for_all_stored_procedures_for_dbo_schema()
    {
        //given
        var result = Model.Schemas["dbo"].Procedures.SelectMany(x => x.Parameters);

        //when & then
        result.Should().HaveCount(6)
            .And.AllBeOfType<MsSqlParameter>()
            .Which.All(x => x.Name.StartsWith("P"));
    }

    [Fact]
    public void Does_model_have_correct_number_of_relationships_for_all_tables_for_dbo_schema()
    {
        //given
        var result = Model.Schemas["dbo"].Tables.SelectMany(t => t.Relationships);

        //when & then
        result.Should().HaveCount(6)
            .And.OnlyHaveUniqueItems()
            .And.AllBeOfType<MsSqlRelationship>();
    }

    [Fact]
    public void Does_model_have_correct_number_of_indexes_for_person_table_for_dbo_schema()
    {
        //given
        var result = Model.Schemas["dbo"].Tables["Person"].Indexes;

        //when & then
        result.Should().HaveCount(3)
            .And.OnlyHaveUniqueItems()
            .And.AllBeOfType<MsSqlIndex>();
    }

    [Fact]
    public void Does_model_have_correct_number_of_index_columns_for_index_on_person_table_for_dbo_schema()
    {
        //given
        var result = Model.Schemas["dbo"].Tables["Person"].Indexes["IX_LastName_FirstName"].IndexedColumns;

        //when & then
        result.Should().HaveCount(2)
            .And.OnlyHaveUniqueItems()
            .And.AllBeOfType<MsSqlIndexedColumn>();
    }

    [Fact]
    public void Does_model_have_correct_index_order_for_index_on_person_table_for_dbo_schema()
    {
        //given
        var result = Model.Schemas["dbo"].Tables["Person"].Indexes["IX_LastLoginDate"].IndexedColumns.Single().IsDescending;

        //when & then
        result.Should().BeTrue();
    }

    [Fact]
    public void Does_model_have_correct_number_of_columns_for_all_tables_for_dbo_schema()
    {
        //given
        var result = Model.Schemas["dbo"].Tables.SelectMany(x => x.Columns);

        //when & then
        result.Should().HaveCount(67)
            .And.OnlyHaveUniqueItems()
            .And.AllBeOfType<MsSqlTableColumn>();
    }

    [Fact]
    public void Does_model_have_correct_number_of_columns_for_person_table_for_dbo_schema()
    {
        //given
        var result = Model.Schemas["dbo"].Tables["Person"].Columns;

        //when & then
        result.Should().HaveCount(11)
            .And.OnlyHaveUniqueItems()
            .And.AllBeOfType<MsSqlTableColumn>();
    }

    [Fact]
    public void Does_model_have_correct_number_of_columns_for_all_views_for_dbo_schema()
    {
        //given
        var result = Model.Schemas["dbo"].Views.SelectMany(x => x.Columns);

        //when & then
        result.Should().HaveCount(3)
            .And.OnlyHaveUniqueItems()
            .And.AllBeOfType<MsSqlViewColumn>();
    }

    [Fact]
    public void Does_model_have_correct_number_of_columns_for_single_view_for_dbo_schema()
    {
        //given
        var result = Model.Schemas["dbo"].Views.Single().Columns;

        //when & then
        result.Should().HaveCount(3)
            .And.OnlyHaveUniqueItems()
            .And.AllBeOfType<MsSqlViewColumn>();
    }

    [Fact]
    public void Can_apply_action_for_id_column_for_person_table_for_dbo_schema()
    {
        //given
        var column = Model.Schemas["dbo"].Tables["Person"].Columns["Id"];

        //when
        column.Apply(c => c.Meta = "foo");
        var result = Model.Schemas["dbo"].Tables["Person"].Columns["Id"];

        //then
        result.Meta.Should().Be("foo");
    }
}