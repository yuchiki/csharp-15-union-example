namespace CSharp15Adt.Processing;

internal static class ParserProgram
{
    internal static int Run(TextReader input, TextWriter output, TextWriter error, params string[] arguments)
    {
        try
        {
            if (arguments.Length > 1)
            {
                throw new FormatException("Pass the expression as one quoted argument.");
            }

            string text;
            if (arguments.Length == 1)
            {
                text = arguments[0];
            }
            else
            {
                var buffer = new char[Parser.MaximumInputLength + 1];
                var count = input.ReadBlock(buffer, 0, buffer.Length);
                text = new string(buffer, 0, count);
            }

            var expression = Parser.Parse(text);
            output.WriteLine(ExpressionOperations.Calculate(expression));
            return 0;
        }
        catch (FormatException exception)
        {
            error.WriteLine(exception.Message);
            return 1;
        }
        catch (DivideByZeroException)
        {
            error.WriteLine("Division by zero.");
            return 1;
        }
        catch (OverflowException)
        {
            error.WriteLine("Arithmetic result is outside the Int32 range.");
            return 1;
        }
    }
}
