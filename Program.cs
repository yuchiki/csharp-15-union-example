namespace CSharp15Adt;

internal static class Program
{
    private static void Main()
    {
        Console.WriteLine(ExpressionOperations.Calculate(ExpressionSamples.Expression1));
        Console.WriteLine(ExpressionOperations.Format(ExpressionSamples.Expression1));
        Console.WriteLine(ExpressionSamples.Expression1.Equals(ExpressionSamples.Expression2));
    }
}
