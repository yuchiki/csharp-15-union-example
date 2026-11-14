using CSharp15Adt.Processing;
using Xunit;
using static CSharp15Adt.Processing.ExpressionOperations;

namespace CSharp15Adt.Tests;

public sealed class ParserTests
{
    [Theory]
    [InlineData("42", 42)]
    [InlineData("3 * 2 + 20 / 5", 10)]
    [InlineData("2 + 3 * 4", 14)]
    [InlineData("(2 + 3) * 4", 20)]
    [InlineData("10 - 3 - 2", 5)]
    [InlineData("24 / 4 / 2", 3)]
    [InlineData("-(2 + 3) * +4", -20)]
    [InlineData("2--3", 5)]
    [InlineData(" \t 12 +\n3 ", 15)]
    public void ParseRespectsArithmeticGrammar(string input, int expected)
    {
        Assert.Equal(expected, Calculate(Parser.Parse(input)));
    }

    [Fact]
    public void ParseBuildsExpectedExpressionTree()
    {
        Assert.Equal("Add(Mul(CInt(3), CInt(2)), Div(CInt(20), CInt(5)))",
            Format(Parser.Parse("3 * 2 + 20 / 5")));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("1 +")]
    [InlineData("1 2")]
    [InlineData("(1 + 2")]
    [InlineData("1 + 2)")]
    [InlineData("()")]
    [InlineData("2 ** 3")]
    [InlineData("abc")]
    [InlineData("2147483648")]
    public void ParseRejectsInvalidInput(string input)
    {
        var error = Assert.Throws<FormatException>(() => Parser.Parse(input));
        Assert.Contains("position", error.Message);
    }

    [Fact]
    public void ParseRejectsNull()
    {
        Assert.Throws<ArgumentNullException>(() => Parser.Parse(null!));
    }
}
