namespace HatTrick.Model.MsSql.Tests;

public class DataTypeTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(byte.MaxValue)]
    [InlineData(long.MaxValue)]
    public void Does_method_for_varchar_equate_to_type_descriptor_method_when_values_are_consistent(long length)
    {
        //given
        var type = MsSqlDataTypes.VarChar(length);

        //when
        var equal = type == MsSqlDataTypes.GetTypeDescriptor("varchar", null, null, length)!;

        //then
        equal.Should().BeTrue();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(byte.MaxValue)]
    [InlineData(long.MaxValue)]
    public void Does_method_for_varchar_not_equate_to_type_descriptor_method_when_values_are_inconsistent(long length)
    {
        //given
        var type = MsSqlDataTypes.VarChar(length);

        //when
        var equal = type == MsSqlDataTypes.GetTypeDescriptor("varchar", null, null, length - 1)!;

        //then
        equal.Should().BeFalse();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(byte.MaxValue)]
    [InlineData(long.MaxValue)]
    public void Does_method_for_char_equate_to_type_descriptor_method_when_values_are_consistent(long length)
    {
        //given
        var type = MsSqlDataTypes.Char(length);

        //when
        var equal = type == MsSqlDataTypes.GetTypeDescriptor("char", null, null, length)!;

        //then
        equal.Should().BeTrue();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(byte.MaxValue)]
    [InlineData(long.MaxValue)]
    public void Does_method_for_char_not_equate_to_type_descriptor_method_when_values_are_inconsistent(long length)
    {
        //given
        var type = MsSqlDataTypes.Char(length);

        //when
        var equal = type == MsSqlDataTypes.GetTypeDescriptor("char", null, null, length - 1)!;

        //then
        equal.Should().BeFalse();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(byte.MaxValue)]
    [InlineData(long.MaxValue)]
    public void Does_method_for_nvarchar_equate_to_type_descriptor_method_when_values_are_consistent(long length)
    {
        //given
        var type = MsSqlDataTypes.NVarChar(length);

        //when
        var equal = type == MsSqlDataTypes.GetTypeDescriptor("nvarchar", null, null, length)!;

        //then
        equal.Should().BeTrue();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(byte.MaxValue)]
    [InlineData(long.MaxValue)]
    public void Does_method_for_nvarchar_not_equate_to_type_descriptor_method_when_values_are_inconsistent(long length)
    {
        //given
        var type = MsSqlDataTypes.NVarChar(length);

        //when
        var equal = type == MsSqlDataTypes.GetTypeDescriptor("nvarchar", null, null, length - 1)!;

        //then
        equal.Should().BeFalse();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(byte.MaxValue)]
    [InlineData(long.MaxValue)]
    public void Does_method_for_nchar_equate_to_type_descriptor_method_when_values_are_consistent(long length)
    {
        //given
        var type = MsSqlDataTypes.NChar(length);

        //when
        var equal = type == MsSqlDataTypes.GetTypeDescriptor("nchar", null, null, length)!;

        //then
        equal.Should().BeTrue();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(byte.MaxValue)]
    [InlineData(long.MaxValue)]
    public void Does_method_for_nchar_not_equate_to_type_descriptor_method_when_values_are_inconsistent(long length)
    {
        //given
        var type = MsSqlDataTypes.NChar(length);

        //when
        var equal = type == MsSqlDataTypes.GetTypeDescriptor("nchar", null, null, length - 1)!;

        //then
        equal.Should().BeFalse();
    }

    [Theory]
    [InlineData(1, 0)]
    [InlineData(100, 4)]
    [InlineData(byte.MaxValue, 12)]
    public void Does_method_for_decimal_equate_to_type_descriptor_method_when_values_are_consistent(byte precision, byte scale)
    {
        //given
        var type = MsSqlDataTypes.Decimal(precision, scale);

        //when
        var equal = type == MsSqlDataTypes.GetTypeDescriptor("decimal", precision, scale, null)!;

        //then
        equal.Should().BeTrue();
    }

    [Theory]
    [InlineData(1, 0)]
    [InlineData(100, 4)]
    [InlineData(byte.MaxValue, 12)]
    public void Does_method_for_decimal_not_equate_to_type_descriptor_method_when_values_are_inconsistent(byte precision, byte scale)
    {
        //given
        var type = MsSqlDataTypes.Decimal(precision, scale);

        //when
        var equal = type == MsSqlDataTypes.GetTypeDescriptor("decimal", (byte)(precision - 1), (byte)(scale - 1), null)!;

        //then
        equal.Should().BeFalse();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(byte.MaxValue)]
    [InlineData(long.MaxValue)]
    public void Does_method_for_binary_equate_to_type_descriptor_method_when_values_are_consistent(long length)
    {
        //given
        var type = MsSqlDataTypes.Binary(length);

        //when
        var equal = type == MsSqlDataTypes.GetTypeDescriptor("binary", null, null, length)!;

        //then
        equal.Should().BeTrue();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(byte.MaxValue)]
    [InlineData(long.MaxValue)]
    public void Does_method_for_binary_not_equate_to_type_descriptor_method_when_values_are_inconsistent(long length)
    {
        //given
        var type = MsSqlDataTypes.Binary(length);

        //when
        var equal = type == MsSqlDataTypes.GetTypeDescriptor("binary", null, null, length - 1)!;

        //then
        equal.Should().BeFalse();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(byte.MaxValue)]
    [InlineData(long.MaxValue)]
    public void Does_method_for_varbinary_equate_to_type_descriptor_method_when_values_are_consistent(long length)
    {
        //given
        var type = MsSqlDataTypes.VarBinary(length);

        //when
        var equal = type == MsSqlDataTypes.GetTypeDescriptor("varbinary", null, null, length)!;

        //then
        equal.Should().BeTrue();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(byte.MaxValue)]
    [InlineData(long.MaxValue)]
    public void Does_method_for_varbinary_not_equate_to_type_descriptor_method_when_values_are_inconsistent(long length)
    {
        //given
        var type = MsSqlDataTypes.VarBinary(length);

        //when
        var equal = type == MsSqlDataTypes.GetTypeDescriptor("varbinary", null, null, length - 1)!;

        //then
        equal.Should().BeFalse();
    }
}