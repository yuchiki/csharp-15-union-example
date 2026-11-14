namespace CSharp15Adt.Processing;

internal static class ParserProgram
{
    internal static int Run(TextReader input, TextWriter output, TextWriter error)
    {
        try
        {
            var buffer = new char[Parser.MaximumInputLength + 1];
            var count = input.ReadBlock(buffer, 0, buffer.Length);
            var expression = Parser.Parse(new string(buffer, 0, count));
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
