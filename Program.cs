using CSharp15Adt.Processing;
using static CSharp15Adt.Processing.ExpressionOperations;

var expression = Parser.Parse("3 * 2 + 20 / 5");

Console.WriteLine(Calculate(expression));
Console.WriteLine(Format(expression));
