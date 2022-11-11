namespace HatTrick.Model.MySql.Tests;

public class DataTypeTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(byte.MaxValue)]
    public void Does_method_for_bit_equate_to_type_descriptor_method_when_values_are_consistent(byte precision)
    {
        //given
        var type = MySqlDataTypes.Bit(precision);

        //when
        var equal = type == MySqlDataTypes.GetTypeDescriptor("bit", $"bit({precision})", precision, null, null, null)!;

        //then
        equal.Should().BeTrue();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(byte.MaxValue)]
    public void Does_method_for_bit_not_equate_to_type_descriptor_method_when_values_are_inconsistent(byte precision)
    {
        //given
        var type = MySqlDataTypes.Bit(precision);

        //when
        var equal = type == MySqlDataTypes.GetTypeDescriptor("bit", $"bit({precision})", (byte)(precision - 1), null, null, null)!;

        //then
        equal.Should().BeFalse();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(byte.MaxValue)]
    [InlineData(long.MaxValue)]
    public void Does_method_for_varchar_equate_to_type_descriptor_method_when_values_are_consistent(long length)
    {
        //given
        var type = MySqlDataTypes.VarChar(length);

        //when
        var equal = type == MySqlDataTypes.GetTypeDescriptor("varchar", null, null, null, length, null)!;

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
        var type = MySqlDataTypes.VarChar(length);

        //when
        var equal = type == MySqlDataTypes.GetTypeDescriptor("varchar", null, null, null, length - 1, null)!;

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
        var type = MySqlDataTypes.Char(length);

        //when
        var equal = type == MySqlDataTypes.GetTypeDescriptor("char", null, null, null, length, null)!;

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
        var type = MySqlDataTypes.Char(length);

        //when
        var equal = type == MySqlDataTypes.GetTypeDescriptor("char", null, null, null, length - 1, null)!;

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
        var type = MySqlDataTypes.Decimal(precision, scale);

        //when
        var equal = type == MySqlDataTypes.GetTypeDescriptor("decimal", null, precision, scale, null, null)!;

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
        var type = MySqlDataTypes.Decimal(precision, scale);

        //when
        var equal = type == MySqlDataTypes.GetTypeDescriptor("decimal", null, (byte)(precision - 1), (byte)(scale - 1), null, null)!;

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
        var type = MySqlDataTypes.VarBinary(length);

        //when
        var equal = type == MySqlDataTypes.GetTypeDescriptor("varbinary", null, null, null, length, null)!;

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
        var type = MySqlDataTypes.VarBinary(length);

        //when
        var equal = type == MySqlDataTypes.GetTypeDescriptor("varbinary", null, null, null, length - 1, null)!;

        //then
        equal.Should().BeFalse();
    }

    [Theory]
    [InlineData("a", "b", "c")]
    [InlineData("aa", "bb", "cc")]
    [InlineData("aaa", "bbb", "ccc")]
    public void Does_method_for_enum_equate_to_type_descriptor_method_when_values_are_consistent(string a, string b, string c)
    {
        //given
        var type = MySqlDataTypes.Enum(a, b, c);

        //when
        var equal = type == MySqlDataTypes.GetTypeDescriptor("enum", $"enum('{a}','{b}','{c}')", null, null, null, null)!;

        //then
        equal.Should().BeTrue();
    }

    [Theory]
    [InlineData("a", "b", "c")]
    [InlineData("aa", "bb", "cc")]
    [InlineData("aaa", "bbb", "ccc")]
    public void Does_method_for_enum_not_equate_to_type_descriptor_method_when_values_are_inconsistent(string a, string b, string c)
    {
        //given
        var type = MySqlDataTypes.Enum(a, b, c);

        //when
        var equal = type == MySqlDataTypes.GetTypeDescriptor("enum", $"enum('{a}','{b}','{c}','{a}')", null, null, null, null)!;

        //then
        equal.Should().BeFalse();
    }

    [Theory]
    [InlineData("a", "b", "c")]
    [InlineData("aa", "bb", "cc")]
    [InlineData("aaa", "bbb", "ccc")]
    public void Does_method_for_set_equate_to_type_descriptor_method_when_values_are_consistent(string a, string b, string c)
    {
        //given
        var type = MySqlDataTypes.Set(a, b, c);

        //when
        var equal = type == MySqlDataTypes.GetTypeDescriptor("set", $"set('{a}','{b}','{c}')", null, null, null, null)!;

        //then
        equal.Should().BeTrue();
    }

    [Theory]
    [InlineData("a", "b", "c")]
    [InlineData("aa", "bb", "cc")]
    [InlineData("aaa", "bbb", "ccc")]
    public void Does_method_for_set_not_equate_to_type_descriptor_method_when_values_are_inconsistent(string a, string b, string c)
    {
        //given
        var type = MySqlDataTypes.Set(a, b, c);

        //when
        var equal = type == MySqlDataTypes.GetTypeDescriptor("set", $"set('{a}','{b}','{c}','{a}')", null, null, null, null)!;

        //then
        equal.Should().BeFalse();
    }
}