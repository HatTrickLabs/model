namespace HatTrick.Model.MySql.Tests;

public class MySqlModelApplyValuesTests 
{
    private MySqlModel GetModel()
    {
        var model = new MySqlModel();

        var schema = new MySqlSchema { Identifier = "Schema_1", Name = "Schema_1" };
        model.Schemas.Add(schema);
        model.Schemas.Add(new MySqlSchema { Identifier = "Schema_2", Name = "Schema_2" });

        var table = new MySqlTable { Identifier = "Table_1", Name = "Table_1" };
        schema.Tables.Add(table);
        schema.Tables.Add(new MySqlTable { Identifier = "Table_2", Name = "Table_2" });

        table.Columns.Add(new MySqlTableColumn { Identifier = "Column_1", Name = "Column_1", AutoIncrement = true });
        table.Columns.Add(new MySqlTableColumn { Identifier = "Column_2", Name = "Column_2", AutoIncrement = false });
        table.Columns.Add(new MySqlTableColumn { Identifier = "Column_3", Name = "Column_3", AutoIncrement = false });

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
            c.SqlType = MySqlDbType.VarChar;
            c.SqlTypeName = "varchar";
            c.IsNullable = true;
            c.Meta["foo"] = "bar";
        });

        var resolved = model.Schemas["Schema_1"]!.Tables["Table_1"]!.Columns["Not_Column_1"];

        //then
        resolved.MaxLength.Should().Be(50);
        resolved.SqlType.Should().Be(MySqlDbType.VarChar);
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
