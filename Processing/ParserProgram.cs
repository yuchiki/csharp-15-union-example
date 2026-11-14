namespace CSharp15Adt.Processing;

internal static class ParserProgram
{
    internal static int Run(TextWriter output, TextWriter error, params string[] arguments)
    {
        try
        {
            if (arguments.Length != 1)
            {
                throw new FormatException("Pass the expression as one quoted argument.");
            }

            var expression = Parser.Parse(arguments[0]);
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
