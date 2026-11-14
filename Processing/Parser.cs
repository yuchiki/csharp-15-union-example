using System.Globalization;
using CSharp15Adt.Data;

namespace CSharp15Adt.Processing;

internal sealed class Parser
{
    internal const int MaximumInputLength = 1024;
    private const int MaximumDepth = 64;
    private readonly string _input;
    private int _position;
    private int _depth;

    private Parser(string input) => _input = input;

    internal static Expr Parse(string input)
    {
        ArgumentNullException.ThrowIfNull(input);
        if (input.Length > MaximumInputLength)
        {
            throw new FormatException($"Input exceeds {MaximumInputLength} characters at position {MaximumInputLength}.");
        }
        var parser = new Parser(input);
        var expression = parser.ParseExpression();
        parser.SkipWhitespace();
        if (parser._position != input.Length)
        {
            throw parser.Error("Unexpected character");
        }

        return expression;
    }

    // expression = term (('+' | '-') term)*
    private Expr ParseExpression()
    {
        var left = ParseTerm();
        while (true)
        {
            if (Consume('+'))
            {
                left = new Add(left, ParseTerm());
            }
            else if (Consume('-'))
            {
                left = new Sub(left, ParseTerm());
            }
            else
            {
                return left;
            }
        }
    }

    // term = factor (('*' | '/') factor)*
    private Expr ParseTerm()
    {
        var left = ParseFactor();
        while (true)
        {
            if (Consume('*'))
            {
                left = new Mul(left, ParseFactor());
            }
            else if (Consume('/'))
            {
                left = new Div(left, ParseFactor());
            }
            else
            {
                return left;
            }
        }
    }

    // factor = ('+' | '-') factor | '(' expression ')' | integer
    private Expr ParseFactor()
    {
        if (_depth >= MaximumDepth)
        {
            throw Error($"Expression exceeds maximum nesting depth {MaximumDepth}");
        }

        _depth++;
        try
        {
            return ParseFactorCore();
        }
        finally
        {
            _depth--;
        }
    }

    private Expr ParseFactorCore()
    {
        if (Consume('+'))
        {
            return ParseFactor();
        }

        if (Consume('-'))
        {
            return new Sub(new CInt(0), ParseFactor());
        }

        if (Consume('('))
        {
            var expression = ParseExpression();
            if (!Consume(')'))
            {
                throw Error("Expected ')'");
            }

            return expression;
        }

        SkipWhitespace();
        var start = _position;
        while (_position < _input.Length && char.IsAsciiDigit(_input[_position]))
        {
            _position++;
        }

        if (start == _position)
        {
            throw Error("Expected an integer or '('");
        }

        if (!int.TryParse(_input.AsSpan(start, _position - start),
                NumberStyles.None, CultureInfo.InvariantCulture, out var value))
        {
            throw Error("Integer literal is outside the Int32 range");
        }

        return new CInt(value);
    }

    private bool Consume(char token)
    {
        SkipWhitespace();
        if (_position == _input.Length || _input[_position] != token)
        {
            return false;
        }

        _position++;
        return true;
    }

    private void SkipWhitespace()
    {
        while (_position < _input.Length && char.IsWhiteSpace(_input[_position]))
        {
            _position++;
        }
    }

    private FormatException Error(string message) =>
        new($"{message} at position {_position}.");
}
