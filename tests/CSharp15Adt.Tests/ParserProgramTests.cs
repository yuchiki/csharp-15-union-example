using CSharp15Adt.Processing;
using Xunit;

namespace CSharp15Adt.Tests;

public sealed class ParserProgramTests
{
    [Fact]
    public void RunRejectsOversizedInput()
    {
        using var input = new StringReader(new string('1', Parser.MaximumInputLength + 1));
        using var output = new StringWriter();
        using var error = new StringWriter();

        Assert.Equal(1, ParserProgram.Run(input, output, error));
        Assert.Empty(output.ToString());
        Assert.Contains("Input exceeds", error.ToString());
    }

    [Theory]
    [InlineData("3 * 2 + 20 / 5\n", "Add(Mul(CInt(3), CInt(2)), Div(CInt(20), CInt(5)))")]
    [InlineData("1 +\n2", "Add(CInt(1), CInt(2))")]
    [InlineData("1 / 0", "Div(CInt(1), CInt(0))")]
    public void RunWritesParsedTreeWithoutEvaluating(string text, string expected)
    {
        using var input = new StringReader(text);
        using var output = new StringWriter();
        using var error = new StringWriter();

        Assert.Equal(0, ParserProgram.Run(input, output, error));
        Assert.Equal(expected + Environment.NewLine, output.ToString());
        Assert.Empty(error.ToString());
    }

    [Theory]
    [InlineData("", "Expected an integer or '(' at position 0.")]
    [InlineData("1 +", "Expected an integer or '(' at position 3.")]
    [InlineData("1 2", "Unexpected character at position 2.")]
    public void RunReportsInvalidInput(string text, string expected)
    {
        using var input = new StringReader(text);
        using var output = new StringWriter();
        using var error = new StringWriter();

        Assert.Equal(1, ParserProgram.Run(input, output, error));
        Assert.Empty(output.ToString());
        Assert.Equal(expected + Environment.NewLine, error.ToString());
    }
}
