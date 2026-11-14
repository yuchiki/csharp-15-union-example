using CSharp15Adt.Processing;
using Xunit;

namespace CSharp15Adt.Tests;

public sealed class ParserProgramTests
{
    [Theory]
    [InlineData("3 * 2 + 20 / 5\n", "10")]
    [InlineData("1 +\n2", "3")]
    public void RunWritesCalculationResult(string text, string expected)
    {
        using var output = new StringWriter();
        using var error = new StringWriter();

        Assert.Equal(0, ParserProgram.Run(output, error, text));
        Assert.Equal(expected + Environment.NewLine, output.ToString());
        Assert.Empty(error.ToString());
    }

    [Theory]
    [InlineData("", "Expected an integer or '(' at position 0.")]
    [InlineData("1 +", "Expected an integer or '(' at position 3.")]
    [InlineData("1 2", "Unexpected character at position 2.")]
    [InlineData("1 / 0", "Division by zero.")]
    [InlineData("2147483647 + 1", "Arithmetic result is outside the Int32 range.")]
    [InlineData("-2147483647 - 2", "Arithmetic result is outside the Int32 range.")]
    [InlineData("2147483647 * 2", "Arithmetic result is outside the Int32 range.")]
    public void RunReportsInvalidInput(string text, string expected)
    {
        using var output = new StringWriter();
        using var error = new StringWriter();

        Assert.Equal(1, ParserProgram.Run(output, error, text));
        Assert.Empty(output.ToString());
        Assert.Equal(expected + Environment.NewLine, error.ToString());
    }

    [Fact]
    public void RunRejectsMissingArgument()
    {
        using var output = new StringWriter();
        using var error = new StringWriter();

        Assert.Equal(1, ParserProgram.Run(output, error));
        Assert.Empty(output.ToString());
        Assert.Contains("one quoted argument", error.ToString());
    }

    [Fact]
    public void RunRejectsMultipleArguments()
    {
        using var output = new StringWriter();
        using var error = new StringWriter();

        Assert.Equal(1, ParserProgram.Run(output, error, "1", "+", "2"));
        Assert.Empty(output.ToString());
        Assert.Contains("one quoted argument", error.ToString());
    }
}
