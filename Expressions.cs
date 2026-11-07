namespace CSharp15Adt;

internal sealed record CInt(int Value);
internal sealed record Add(Expr Left, Expr Right);
internal sealed record Sub(Expr Left, Expr Right);
internal sealed record Mul(Expr Left, Expr Right);
internal sealed record Div(Expr Left, Expr Right);

internal union Expr(CInt, Add, Sub, Mul, Div);

internal static class ExpressionSamples
{
    internal static readonly Expr Expression1 =
        new Add(
            new Mul(new CInt(3), new CInt(2)),
            new Div(new CInt(20), new CInt(5)));
}
