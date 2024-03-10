using System.Text;
using System.Text.RegularExpressions;

namespace SoundMaker.ScoreData.SMSC;
internal class Lexer
{
    private readonly Regex _alphaRegex = new("[a-z]|[A-Z]");

    private readonly string _data = "";

    public Lexer(string data)
    {
        _data = data;
    }

    public List<Token> ReadAll()
    {
        var tokens = new List<Token>();
        var data = _data.Replace("\r\n", "\n").Replace('\r', '\n');
        var lines = data.Split('\n');
        for (var i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            var lineNumber = i + 1;
            var chars = line.ToCharArray();
            var otherTypeLiteralBuilder = new StringBuilder();
            var numberLiteralBuilder = new StringBuilder();
            for (var j = 0; j < chars.Length; j++)
            {
                char? next = j + 1 < chars.Length ? chars[j + 1] : null;
                // numbers
                if (char.IsNumber(chars[j]))
                {
                    _ = numberLiteralBuilder.Append(chars[j]);
                    if (otherTypeLiteralBuilder.Length != 0)
                    {
                        var literal = otherTypeLiteralBuilder.ToString();
                        var type = MatchOtherType(literal);
                        tokens.Add(new(type, otherTypeLiteralBuilder.ToString(), lineNumber));
                        _ = otherTypeLiteralBuilder.Clear();
                    }
                    continue;
                }
                // others
                if (IsOtherChar(chars[j], next))
                {
                    _ = otherTypeLiteralBuilder.Append(chars[j]);
                    if (numberLiteralBuilder.Length != 0)
                    {
                        var literal = numberLiteralBuilder.ToString();
                        tokens.Add(new(TokenType.Number, literal, lineNumber));
                        _ = numberLiteralBuilder.Clear();
                    }
                    continue;
                }

                if (numberLiteralBuilder.Length != 0)
                {
                    var literal = numberLiteralBuilder.ToString();
                    tokens.Add(new(TokenType.Number, literal, lineNumber));
                    _ = numberLiteralBuilder.Clear();
                }
                if (otherTypeLiteralBuilder.Length != 0)
                {
                    var literal = otherTypeLiteralBuilder.ToString();
                    var type = MatchOtherType(literal);
                    tokens.Add(new(type, literal, lineNumber));
                    _ = otherTypeLiteralBuilder.Clear();
                }

                // comment out
                if (j + 1 < chars.Length && IsCommentPrefix(chars[j], next))
                {
                    break;
                }

                // space
                if (char.IsWhiteSpace(chars[j]))
                {
                    continue;
                }

                // symbols
                Token token = chars[j] switch
                {
                    '.' => new(TokenType.Dot, ".", lineNumber),
                    '#' => new(TokenType.Sharp, "#", lineNumber),
                    '(' => new(TokenType.LeftParentheses, "(", lineNumber),
                    ')' => new(TokenType.RightParentheses, ")", lineNumber),
                    ',' => new(TokenType.Comma, ",", lineNumber),
                    _ => new(TokenType.Unknown, chars[j].ToString(), lineNumber),
                };
                tokens.Add(token);
            }

            if (numberLiteralBuilder.Length != 0)
            {
                var literal = numberLiteralBuilder.ToString();
                tokens.Add(new(TokenType.Number, literal, lineNumber));
                _ = numberLiteralBuilder.Clear();
            }

            if (otherTypeLiteralBuilder.Length != 0)
            {
                var literal = otherTypeLiteralBuilder.ToString();
                var type = MatchOtherType(literal);
                tokens.Add(new(type, literal, lineNumber));
                _ = otherTypeLiteralBuilder.Clear();
            }
        }
        return tokens;
    }

    private TokenType MatchOtherType(string other)
    {
        if (int.TryParse(other, out _))
        {
            return TokenType.Number;
        }
        if (_alphaRegex.IsMatch(other))
        {
            return other switch
            {
                "tie" => TokenType.Tie,
                "tup" => TokenType.Tuplet,
                "rest" => TokenType.Rest,
                _ => TokenType.Alphabet,
            };
        }
        return TokenType.Unknown;
    }

    private bool IsOtherChar(char character, char? nextCharacter)
    {
        var next = nextCharacter ?? 'a'; // 'a' is not comment prefix.
        return
            !IsSymbol(character) &&
            !IsCommentPrefix(character, next) &&
            !char.IsWhiteSpace(character);
    }

    private bool IsCommentPrefix(char character, char? nextCharacter)
    {
        return character is '/' && nextCharacter is '/';
    }

    private bool IsSymbol(char character)
    {
        return character is '.' or '#' or '(' or ')' or ',';
    }
}
