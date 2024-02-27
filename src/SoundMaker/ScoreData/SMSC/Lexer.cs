using System.Text;
using System.Text.RegularExpressions;

namespace SoundMaker.ScoreData.SMSC;
internal class Lexer
{
    private readonly Regex _alphaRegex = new("[a-z]|[A-z]");

    private readonly string _data = "";

    public Lexer(string data)
    {
        _data = data;
    }

    public List<Token> ReadAll()
    {
        var tokens = new List<Token>();
        foreach (var line in _data.Split('\n'))
        {
            var chars = line.ToCharArray();
            var otherTypeLiteralBuilder = new StringBuilder();
            for (var i = 0; i < chars.Length; i++)
            {
                char? next = i + 1 < chars.Length ? chars[i + 1] : null;
                // others
                if (IsOtherChar(chars[i], next))
                {
                    _ = otherTypeLiteralBuilder.Append(chars[i]);
                }
                else if (otherTypeLiteralBuilder.Length != 0)
                {
                    var literal = otherTypeLiteralBuilder.ToString();
                    var type = MatchOtherType(literal);
                    tokens.Add(new(type, otherTypeLiteralBuilder.ToString()));
                    _ = otherTypeLiteralBuilder.Clear();
                }

                // comment out
                if (i + 1 < chars.Length && chars[i] is '/' && next is '/')
                {
                    break;
                }

                // space
                if (char.IsWhiteSpace(chars[i]))
                {
                    continue;
                }

                // number
                if (char.IsDigit(chars[i]))
                {
                    tokens.Add(new(TokenType.Number, chars[i].ToString()));
                    continue;
                }

                // symbols
                Token token = chars[i] switch
                {
                    '.' => new(TokenType.Dot, "."),
                    '#' => new(TokenType.Sharp, "#"),
                    '(' => new(TokenType.LeftParentheses, "("),
                    ')' => new(TokenType.RightParentheses, ")"),
                    ',' => new(TokenType.Comma, ","),
                    _ => new(TokenType.Unknown, chars[i].ToString()),
                };
                tokens.Add(token);
            }
        }
        return tokens;
    }

    private TokenType MatchOtherType(string other)
    {
        if (_alphaRegex.IsMatch(other))
        {
            return other switch
            {
                "tie" => TokenType.Tie,
                "tup" => TokenType.Tuplet,
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
            !char.IsWhiteSpace(character) &&
            !char.IsDigit(character);
    }

    private bool IsCommentPrefix(char character, char nextCharacter)
    {
        return character is '/' && nextCharacter is '/';
    }

    private bool IsSymbol(char character)
    {
        return character is '.' or '#' or '(' or ')' or ',';
    }
}
