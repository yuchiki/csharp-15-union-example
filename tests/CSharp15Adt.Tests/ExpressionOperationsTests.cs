using static CSharp15Adt.ExpressionOperations;
using Xunit;

namespace CSharp15Adt.Tests;

public sealed class ExpressionOperationsTests
{
    [Fact]
    public void CalculateReturnsIntegerValue()
    {
        Assert.Equal(3, Calculate(new CInt(3)));
    }

    [Fact]
    public void CalculateAddsOperands()
    {
        Assert.Equal(5, Calculate(new Add(new CInt(2), new CInt(3))));
    }

    [Fact]
    public void CalculateSubtractsOperands()
    {
        Assert.Equal(2, Calculate(new Sub(new CInt(5), new CInt(3))));
    }

    [Fact]
    public void CalculateMultipliesOperands()
    {
        Assert.Equal(6, Calculate(new Mul(new CInt(2), new CInt(3))));
    }

    [Fact]
    public void CalculateDividesOperands()
    {
        Assert.Equal(4, Calculate(new Div(new CInt(12), new CInt(3))));
    }

    [Fact]
    public void FormatRendersNestedExpression()
    {
        Expr expression = new Add(
            new Mul(new CInt(3), new CInt(2)),
            new Div(new CInt(20), new CInt(5)));

        Assert.Equal(
            "Add(Mul(CInt(3), CInt(2)), Div(CInt(20), CInt(5)))",
            Format(expression));
    }

    [Fact]
    public void FormatRendersSubtraction()
    {
        Expr expression = new Sub(new CInt(5), new CInt(3));

        Assert.Equal("Sub(CInt(5), CInt(3))", Format(expression));
    }
}
