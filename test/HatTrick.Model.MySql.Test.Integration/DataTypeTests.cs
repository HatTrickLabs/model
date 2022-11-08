namespace HatTrick.Model.MySql.Test.Integration;

public class DataTypeTests : IntegrationTestBase
{
    public DataTypeTests() : base("unit_test")
    { }

    [Theory]
    [InlineData(1, "Bit", "bit", MySqlDbType.Bit, "bit(6)", 6, null, null, false, null)]
    [InlineData(2, "NullableBit", "bit", MySqlDbType.Bit, "bit(6)", 6, null, null, true, null)]
    [InlineData(3, "DateTime", "datetime", MySqlDbType.DateTime, "datetime", null, null, null, false, null)]
    [InlineData(4, "NullableDateTime", "datetime", MySqlDbType.DateTime, "datetime", null, null, null, true, null)]
    [InlineData(5, "Date", "date", MySqlDbType.Date, "date", null, null, null, false, null)]
    [InlineData(6, "NullableDate", "date", MySqlDbType.Date, "date", null, null, null, true, null)]
    [InlineData(7, "Timestamp", "timestamp", MySqlDbType.Timestamp, "timestamp", null, null, null, false, null)]
    [InlineData(8, "NullableTimestamp", "timestamp", MySqlDbType.Timestamp, "timestamp", null, null, null, true, null)]
    [InlineData(9, "Time", "time", MySqlDbType.Time, "time", null, null, null, false, null)]
    [InlineData(10, "NullableTime", "time", MySqlDbType.Time, "time", null, null, null, true, null)]
    [InlineData(11, "Year", "year", MySqlDbType.Year, "year", null, null, null, false, null)]
    [InlineData(12, "NullableYear", "year", MySqlDbType.Year, "year", null, null, null, true, null)]
    [InlineData(13, "VarChar", "varchar", MySqlDbType.VarChar, "varchar(20)", null, null, 20L, false, 80L)]
    [InlineData(14, "NullableVarChar", "varchar", MySqlDbType.VarChar, "varchar(20)", null, null, 20L, true, 80L)]
    [InlineData(15, "Char", "char", MySqlDbType.String, "char(20)", null, null, 20L, false, 80L)]
    [InlineData(16, "NullableChar", "char", MySqlDbType.String, "char(20)", null, null, 20L, true, 80L)]
    [InlineData(17, "Blob", "blob", MySqlDbType.Blob, "blob", null, null, 65535L, false, 65535L)]
    [InlineData(18, "NullableBlob", "blob", MySqlDbType.Blob, "blob", null, null, 65535L, true, 65535L)]
    [InlineData(19, "TinyBlob", "tinyblob", MySqlDbType.TinyBlob, "tinyblob", null, null, 255L, false, 255L)]
    [InlineData(20, "NullableTinyBlob", "tinyblob", MySqlDbType.TinyBlob, "tinyblob", null, null, 255L, true, 255L)]
    [InlineData(21, "MediumBlob", "mediumblob", MySqlDbType.MediumBlob, "mediumblob", null, null, 16777215L, false, 16777215L)]
    [InlineData(22, "NullableMediumBlob", "mediumblob", MySqlDbType.MediumBlob, "mediumblob", null, null, 16777215L, true, 16777215L)]
    [InlineData(23, "LongBlob", "longblob", MySqlDbType.LongBlob, "longblob", null, null, 4294967295L, false, 4294967295L)]
    [InlineData(24, "NullableLongBlob", "longblob", MySqlDbType.LongBlob, "longblob", null, null, 4294967295L, true, 4294967295L)]
    [InlineData(25, "TinyInt", "tinyint", MySqlDbType.Byte, "tinyint", 3, 0, null, false, null)]
    [InlineData(26, "NullableTinyInt", "tinyint", MySqlDbType.Byte, "tinyint", 3, 0, null, true, null)]
    [InlineData(27, "SmallInt", "smallint", MySqlDbType.Int16, "smallint", 5, 0, null, false, null)]
    [InlineData(28, "NullableSmallInt", "smallint", MySqlDbType.Int16, "smallint", 5, 0, null, true, null)]
    [InlineData(29, "MediumInt", "mediumint", MySqlDbType.Int24, "mediumint", 7, 0, null, false, null)]
    [InlineData(30, "NullableMediumInt", "mediumint", MySqlDbType.Int24, "mediumint", 7, 0, null, true, null)]
    [InlineData(31, "Int", "int", MySqlDbType.Int32, "int", 10, 0, null, false, null)]
    [InlineData(32, "NullableInt", "int", MySqlDbType.Int32, "int", 10, 0, null, true, null)]
    [InlineData(33, "BigInt", "bigint", MySqlDbType.Int64, "bigint", 19, 0, null, false, null)]
    [InlineData(34, "NullableBigInt", "bigint", MySqlDbType.Int64, "bigint", 19, 0, null, true, null)]
    [InlineData(35, "Double", "double", MySqlDbType.Double, "double", 22, null, null, false, null)]
    [InlineData(36, "NullableDouble", "double", MySqlDbType.Double, "double", 22, null, null, true, null)]
    [InlineData(37, "Decimal", "decimal", MySqlDbType.Decimal, "decimal(5,4)", 5, 4, null, false, null)]
    [InlineData(38, "NullableDecimal", "decimal", MySqlDbType.Decimal, "decimal(5,4)", 5, 4, null, true, null)]
    [InlineData(39, "Float", "float", MySqlDbType.Float, "float", 12, null, null, false, null)]
    [InlineData(40, "NullableFloat", "float", MySqlDbType.Float, "float", 12, null, null, true, null)]
    [InlineData(41, "Binary", "binary", MySqlDbType.Binary, "binary(1)", null, null, 1L, false, 1L)]
    [InlineData(42, "NullableBinary", "binary", MySqlDbType.Binary, "binary(1)", null, null, 1L, true, 1L)]
    [InlineData(43, "VarBinary", "varbinary", MySqlDbType.VarBinary, "varbinary(20)", null, null, 20L, false, 20L)]
    [InlineData(44, "NullableVarBinary", "varbinary", MySqlDbType.VarBinary, "varbinary(20)", null, null, 20L, true, 20L)]
    [InlineData(45, "TinyText", "tinytext", MySqlDbType.TinyText, "tinytext", null, null, 255L, false, 255L)]
    [InlineData(46, "NullableTinyText", "tinytext", MySqlDbType.TinyText, "tinytext", null, null, 255L, true, 255L)]
    [InlineData(47, "Text", "text", MySqlDbType.Text, "text", null, null, 65535L, false, 65535L)]
    [InlineData(48, "NullableText", "text", MySqlDbType.Text, "text", null, null, 65535L, true, 65535L)]
    [InlineData(49, "MediumText", "mediumtext", MySqlDbType.MediumText, "mediumtext", null, null, 16777215L, false, 16777215L)]
    [InlineData(50, "NullableMediumText", "mediumtext", MySqlDbType.MediumText, "mediumtext", null, null, 16777215L, true, 16777215L)]
    [InlineData(51, "LongText", "longtext", MySqlDbType.LongText, "longtext", null, null, 4294967295L, false, 4294967295L)]
    [InlineData(52, "NullableLongText", "longtext", MySqlDbType.LongText, "longtext", null, null, 4294967295L, true, 4294967295L)]
    [InlineData(53, "Set", "set", MySqlDbType.Set, "set('a','b','c','d')", null, null, 7L, false, 28L)]
    [InlineData(54, "NullableSet", "set", MySqlDbType.Set, "set('a','b','c','d')", null, null, 7L, true, 28L)]
    [InlineData(55, "Enum", "enum", MySqlDbType.Enum, "enum('a','b','c','d')", null, null, 1L, false, 4L)]
    [InlineData(56, "NullableEnum", "enum", MySqlDbType.Enum, "enum('a','b','c','d')", null, null, 1L, true, 4L)]

    public void Does_column_have_correct_data_attributes(int ordinal, string columnName, string dbTypeName, MySqlDbType dbType, string columnType, int? precision, int? scale, long? maxLength, bool isNullable, long? characterOctetLength)
    {
        //given
        var result = Model.Schemas["unit_test"].Tables["DataType"].Columns[columnName];

        //when & then
        using AssertionScope _ = new();
        result.OrdinalPosition.Should().Be(ordinal);
        result.SqlType.Should().Be(dbType);
        result.SqlTypeName.Should().Be(dbTypeName);
        result.IsNullable.Should().Be(isNullable);
        result.Precision.Should().Be((byte?)precision);
        result.Scale.Should().Be((byte?)scale);
        result.MaxLength.Should().Be(maxLength);
        result.Name.Should().Be(columnName);
        result.ColumnType.Should().Be(columnType);
        result.CharacterOctetLength.Should().Be(characterOctetLength);

        result.AutoIncrement.Should().BeFalse();
        result.GenerationExpression.Should().BeNull();
        result.DefaultDefinition.Should().BeNull();
    }
}