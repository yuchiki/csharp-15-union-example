using System.Globalization;
using CSharp15Adt.Data;

namespace CSharp15Adt.Processing;

internal sealed class Parser
{
    private readonly string _input;
    private int _position;
    private bool IsAtEnd => _position == _input.Length;

    private Parser(string input) => _input = input;

    internal static Expr Parse(string input)
    {
        ArgumentNullException.ThrowIfNull(input);
        var parser = new Parser(input);
        var expression = parser.ParseExpression();
        parser.SkipWhitespace();
        if (!parser.IsAtEnd)
        {
            throw parser.Error("Unexpected character");
        }

        return expression;
    }

    // expression = term (('+' | '-') term)*
    private Expr ParseExpression() => ParseBinary(ParseTerm,
        '+', (left, right) => new Add(left, right),
        '-', (left, right) => new Sub(left, right));

    // term = factor (('*' | '/') factor)*
    private Expr ParseTerm() => ParseBinary(ParseFactor,
        '*', (left, right) => new Mul(left, right),
        '/', (left, right) => new Div(left, right));

    private Expr ParseBinary(Func<Expr> parseOperand,
        char firstOperator, Func<Expr, Expr, Expr> createFirst,
        char secondOperator, Func<Expr, Expr, Expr> createSecond)
    {
        var left = parseOperand();
        while (true)
        {
            if (Consume(firstOperator))
            {
                left = createFirst(left, parseOperand());
            }
            else if (Consume(secondOperator))
            {
                left = createSecond(left, parseOperand());
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

        return ParseInteger();
    }

    private Expr ParseInteger()
    {
        SkipWhitespace();
        var start = _position;
        while (!IsAtEnd && char.IsAsciiDigit(_input[_position]))
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
        if (IsAtEnd || _input[_position] != token)
        {
            return false;
        }

        _position++;
        return true;
    }

    private void SkipWhitespace()
    {
        while (!IsAtEnd && char.IsWhiteSpace(_input[_position]))
        {
            _position++;
        }
    }

    private FormatException Error(string message) =>
        new($"{message} at position {_position}.");
}
