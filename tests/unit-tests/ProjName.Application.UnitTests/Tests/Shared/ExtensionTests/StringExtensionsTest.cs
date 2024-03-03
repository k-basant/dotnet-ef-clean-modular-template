using ProjName.Application.Shared.Extensions;

namespace ProjName.Application.UnitTests.Tests.Shared.ExtensionTests;

public class StringExtensionsTest : TestBase
{
    public StringExtensionsTest(StartupFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public void IsNullOrEmpty_ShouldReturnFalse_WhenNotEmpty()
    {
        var actual = "SomeRandomString".IsNullOrEmpty();
        actual.Should().BeFalse();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void IsNullOrEmpty_ShouldReturnTrue_WhenNullOrEmpty(string str)
    {
        var actual = str.IsNullOrEmpty();
        actual.Should().BeTrue();
    }

    [Fact]
    public void IsNotNullOrEmpty_ShouldReturnTrue_WhenNotEmpty()
    {
        var actual = "SomeRandomString".IsNotNullOrEmpty();
        actual.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void IsNotNullOrEmpty_ShouldReturnFalse_WhenNullOrEmpty(string str)
    {
        var actual = str.IsNotNullOrEmpty();
        actual.Should().BeFalse();
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData("", "")]
    [InlineData("aCamelCase", "aCamelCase")]
    [InlineData("APascalCase", "aPascalCase")]
    [InlineData("A_snake_Case", "a_snake_Case")]
    [InlineData("_AnyRandomString", "_AnyRandomString")]
    public void ToCamelCase_ReturnsCamelCasedString(string str, string expected)
    {
        var actual = str.ToCamelCase();
        actual.Should().Be(expected);
    }
}
