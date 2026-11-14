using CSharp15Adt;
using static CSharp15Adt.ExpressionOperations;

Expr expression = new Add(
    new Mul(new CInt(3), new CInt(2)),
    new Div(new CInt(20), new CInt(5)));

Console.WriteLine(Calculate(expression));
Console.WriteLine(Format(expression));
