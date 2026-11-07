namespace CSharp15Adt;

internal static class ExpressionOperations
{
    internal static int Calculate(Expr expression) => expression switch
    {
        CInt(var value) => value,
        Add(var left, var right) => Calculate(left) + Calculate(right),
        Sub(var left, var right) => Calculate(left) - Calculate(right),
        Mul(var left, var right) => Calculate(left) * Calculate(right),
        Div(var left, var right) => Calculate(left) / Calculate(right),
    };

    internal static string Format(Expr expression) => expression switch
    {
        CInt(var value) => $"CInt({value})",
        Add(var left, var right) => $"Add({Format(left)}, {Format(right)})",
        Sub(var left, var right) => $"Sub({Format(left)}, {Format(right)})",
        Mul(var left, var right) => $"Mul({Format(left)}, {Format(right)})",
        Div(var left, var right) => $"Div({Format(left)}, {Format(right)})",
    };
}
