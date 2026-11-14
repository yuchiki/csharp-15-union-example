namespace CSharp15Adt.Processing;

internal static class ParserProgram
{
    internal static int Run(TextReader input, TextWriter output, TextWriter error)
    {
        try
        {
            var expression = Parser.Parse(input.ReadToEnd());
            output.WriteLine(ExpressionOperations.Format(expression));
            return 0;
        }
        catch (FormatException exception)
        {
            error.WriteLine(exception.Message);
            return 1;
        }
    }
}
