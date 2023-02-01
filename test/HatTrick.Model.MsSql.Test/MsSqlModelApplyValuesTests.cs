namespace HatTrick.Model.MsSql.Tests;

public class MsSqlModelApplyValuesTests 
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
    public void Can_apply_column_properties()
    {
        //given
        var model = GetModel();

        //when
        model.Schemas["Schema_1"]!.Tables["Table_1"]!.Columns["Column_1"]!.Apply((c) =>
        {
            c.Name = "Not_Column_1";
            c.MaxLength = 50;
            c.SqlType = SqlDbType.VarChar;
            c.SqlTypeName = "varchar";
            c.IsNullable = true;
            c.Meta["foo"] = "bar";
        });

        var resolved = model.Schemas["Schema_1"]!.Tables["Table_1"]!.Columns["Not_Column_1"];

        //then
        resolved.MaxLength.Should().Be(50);
        resolved.SqlType.Should().Be(SqlDbType.VarChar);
        resolved.SqlTypeName.Should().Be("varchar");
        resolved.IsNullable.Should().BeTrue();
        resolved.Meta.Should().HaveElementAt(0, new KeyValuePair<string, object>("foo", "bar"));
    }

    [Fact]
    public void Can_apply_table_properties()
    {
        //given
        var model = GetModel();

        //when
        model.Schemas["Schema_1"]!.Tables["Table_1"]!.Apply((c) =>
        {
            c.Name = "Not_Table_1";
            c.Meta["foo"] = "bar";
        });

        var resolved = model.Schemas["Schema_1"]!.Tables["Not_Table_1"]!;

        //then
        resolved.Meta.Should().HaveElementAt(0, new KeyValuePair<string, object>("foo", "bar"));
    }
}
