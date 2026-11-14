using CSharp15Adt.Data;
using System.Text;

namespace CSharp15Adt.Processing;

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

    internal static string Format(Expr expression)
    {
        var builder = new StringBuilder();
        Append(expression, builder);
        return builder.ToString();
    }

    private static StringBuilder Append(Expr expression, StringBuilder builder) => expression switch
    {
        CInt(var value) => builder.Append("CInt(").Append(value).Append(')'),
        Add(var left, var right) => AppendBinary("Add", left, right, builder),
        Sub(var left, var right) => AppendBinary("Sub", left, right, builder),
        Mul(var left, var right) => AppendBinary("Mul", left, right, builder),
        Div(var left, var right) => AppendBinary("Div", left, right, builder),
    };

    private static StringBuilder AppendBinary(string name, Expr left, Expr right, StringBuilder builder)
    {
        builder.Append(name).Append('(');
        Append(left, builder);
        builder.Append(", ");
        Append(right, builder);
        return builder.Append(')');
    }
}
