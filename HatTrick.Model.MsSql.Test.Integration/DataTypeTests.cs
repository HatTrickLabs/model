namespace HatTrick.Model.MsSql.Test.Integration;

public class DataTypeTests : IntegrationTestBase
{
    [Theory]
    [InlineData(1, "Bit", "bit", SqlDbType.Bit, 1,0, 1L, false)]
    [InlineData(2, "NullableBit", "bit", SqlDbType.Bit, 1, 0, 1L, true)]
    [InlineData(3, "DateTime", "datetime", SqlDbType.DateTime, 23, 3, 8L, false)]
    [InlineData(4, "NullableDateTime", "datetime", SqlDbType.DateTime, 23, 3, 8L, true)]
    [InlineData(5, "SmallDateTime", "smalldatetime", SqlDbType.SmallDateTime, 16, 0, 4L, false)]
    [InlineData(6, "NullableSmallDateTime", "smalldatetime", SqlDbType.SmallDateTime, 16, 0, 4L, true)]
    [InlineData(7, "DateTime2", "datetime2", SqlDbType.DateTime2, 27, 7, 8L, false)]
    [InlineData(8, "NullableDateTime2", "datetime2", SqlDbType.DateTime2, 27, 7, 8L, true)]
    [InlineData(9, "DateTimeOffset", "datetimeoffset", SqlDbType.DateTimeOffset, 34, 7, 10L, false)]
    [InlineData(10, "NullableDateTimeOffset", "datetimeoffset", SqlDbType.DateTimeOffset, 34, 7, 10L, true)]
    [InlineData(11, "Date", "date", SqlDbType.Date, 10, 0, 3L, false)]
    [InlineData(12, "NullableDate", "date", SqlDbType.Date, 10, 0, 3L, true)]
    [InlineData(13, "Timestamp", "timestamp", SqlDbType.Timestamp, 0, 0, 8L, false)]        
    [InlineData(14, "Time", "time", SqlDbType.Time, 16, 7, 5L, false)]
    [InlineData(15, "NullableTime", "time", SqlDbType.Time, 16, 7, 5L, true)]        
    [InlineData(16, "VarChar", "varchar", SqlDbType.VarChar, 0, 0, 20L, false)]
    [InlineData(17, "NullableVarChar", "varchar", SqlDbType.VarChar, 0, 0, 20L, true)]
    [InlineData(18, "Char", "char", SqlDbType.Char, 0, 0, 20L, false)]
    [InlineData(19, "NullableChar", "char", SqlDbType.Char, 0, 0, 20L, true)]
    [InlineData(20, "NVarChar", "nvarchar", SqlDbType.NVarChar, 0, 0, 40L, false)]
    [InlineData(21, "NullableNVarChar", "nvarchar", SqlDbType.NVarChar, 0, 0, 40L, true)]
    [InlineData(22, "NChar", "nchar", SqlDbType.NChar, 0, 0, 40L, false)]
    [InlineData(23, "NullableNChar", "nchar", SqlDbType.NChar, 0, 0, 40L, true)]
    [InlineData(24, "TinyInt", "tinyint", SqlDbType.TinyInt, 3, 0, 1L, false)]
    [InlineData(25, "NullableTinyInt", "tinyint", SqlDbType.TinyInt, 3, 0, 1L, true)]
    [InlineData(26, "SmallInt", "smallint", SqlDbType.SmallInt, 5, 0, 2L, false)]
    [InlineData(27, "NullableSmallInt", "smallint", SqlDbType.SmallInt, 5, 0, 2L, true)]
    [InlineData(28, "Int", "int", SqlDbType.Int, 10, 0, 4L, false)]
    [InlineData(29, "NullableInt", "int", SqlDbType.Int, 10, 0, 4L, true)]
    [InlineData(30, "BigInt", "bigint", SqlDbType.BigInt, 19, 0, 8L, false)]
    [InlineData(31, "NullableBigInt", "bigint", SqlDbType.BigInt, 19, 0, 8L, true)]
    [InlineData(32, "Money", "money", SqlDbType.Money, 19, 4, 8L, false)]
    [InlineData(33, "NullableMoney", "money", SqlDbType.Money, 19, 4, 8L, true)]
    [InlineData(34, "SmallMoney", "smallmoney", SqlDbType.SmallMoney, 10, 4, 4L, false)]
    [InlineData(35, "NullableSmallMoney", "smallmoney", SqlDbType.SmallMoney, 10, 4, 4L, true)]
    [InlineData(36, "Decimal", "decimal", SqlDbType.Decimal, 5, 4, 5L, false)]
    [InlineData(37, "NullableDecimal", "decimal", SqlDbType.Decimal, 5, 4, 5L, true)]
    [InlineData(38, "Float", "float", SqlDbType.Float, 53, 0, 8L, false)]
    [InlineData(39, "NullableFloat", "float", SqlDbType.Float, 53, 0, 8L, true)]
    [InlineData(40, "Real", "real", SqlDbType.Real, 24, 0, 4L, false)]
    [InlineData(41, "NullableReal", "real", SqlDbType.Real, 24, 0, 4L, true)]
    [InlineData(42, "Uniqueidentifier", "uniqueidentifier", SqlDbType.UniqueIdentifier, 0, 0, 16L, false)]
    [InlineData(43, "NullableUniqueidentifier", "uniqueidentifier", SqlDbType.UniqueIdentifier, 0, 0, 16L, true)]
    [InlineData(44, "Binary", "binary", SqlDbType.Binary, 0, 0, 1L, false)]
    [InlineData(45, "NullableBinary", "binary", SqlDbType.Binary, 0, 0, 1L, true)]
    [InlineData(46, "VarBinary", "varbinary", SqlDbType.VarBinary, 0, 0, 20L, false)]
    [InlineData(47, "NullableVarBinary", "varbinary", SqlDbType.VarBinary, 0, 0, 20L, true)]
    [InlineData(48, "Image", "image", SqlDbType.Image, 0, 0, 16L, false)]
    [InlineData(49, "NullableImage", "image", SqlDbType.Image, 0, 0, 16L, true)]
    [InlineData(50, "Text", "text", SqlDbType.Text, 0, 0, 16L, false)]
    [InlineData(51, "NullableText", "text", SqlDbType.Text, 0, 0, 16L, true)]
    [InlineData(52, "NText", "ntext", SqlDbType.NText, 0, 0, 16L, false)]
    [InlineData(53, "NullableNText", "ntext", SqlDbType.NText, 0, 0, 16L, true)]
    [InlineData(54, "HierarchyId", "hierarchyid", SqlDbType.Udt, 0, 0, 892L, false)]
    [InlineData(55, "NullableHierarchyId", "hierarchyid", SqlDbType.Udt, 0, 0, 892L, true)]
    [InlineData(56, "Geometry", "geometry", SqlDbType.Udt, 0, 0, -1L, false)]
    [InlineData(57, "NullableGeometry", "geometry", SqlDbType.Udt, 0, 0, -1L, true)]
    [InlineData(58, "Geography", "geography", SqlDbType.Udt, 0, 0, -1L, false)]
    [InlineData(59, "NullableGeography", "geography", SqlDbType.Udt, 0, 0, -1L, true)]
    [InlineData(60, "Xml", "xml", SqlDbType.Xml, 0, 0, -1L, false)]
    [InlineData(61, "NullableXml", "xml", SqlDbType.Xml, 0, 0, -1L, true)]

    public void Does_column_have_correct_data_attributes(int ordinal, string columnName, string dbTypeName, SqlDbType dbType, int? precision, int? scale, long? maxLength, bool isNullable)
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

        result.IsIdentity.Should().BeFalse();
        result.IsComputed.Should().BeFalse();
        result.DefaultDefinition.Should().BeNull();
    }
}