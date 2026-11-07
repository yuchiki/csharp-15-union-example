namespace CSharp15Adt;

internal sealed record CInt(int Value);
internal sealed record Add(Expr Left, Expr Right);
internal sealed record Sub(Expr Left, Expr Right);
internal sealed record Mul(Expr Left, Expr Right);
internal sealed record Div(Expr Left, Expr Right);

internal union Expr(CInt, Add, Sub, Mul, Div)
{
    public override bool Equals(object? other) =>
        other is Expr expression && object.Equals(Value, expression.Value);

    public override int GetHashCode() => Value?.GetHashCode() ?? 0;

    public static bool operator ==(Expr left, Expr right) => left.Equals(right);
    public static bool operator !=(Expr left, Expr right) => !left.Equals(right);
}

internal static class ExpressionSamples
{
    internal static readonly Expr Expression1 = CreateExpression();
    internal static readonly Expr Expression2 = CreateExpression();

    private static Expr CreateExpression() =>
        new Add(
            new Mul(new CInt(3), new CInt(2)),
            new Div(new CInt(20), new CInt(5)));
}
