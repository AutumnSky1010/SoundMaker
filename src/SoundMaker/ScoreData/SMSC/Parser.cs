using SoundMaker.Sounds.Score;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("SoundMakerTests")]

namespace SoundMaker.ScoreData.SMSC;

internal class TokenQueue
{
    private readonly Queue<Token> _tokens;

    public TokenQueue(IEnumerable<Token> tokens)
    {
        _tokens = new(tokens);
    }

    public Token? PrevToken { get; private set; }

    public int Count => _tokens.Count;

    public bool TryDequeue([MaybeNullWhen(false)] out Token token)
    {
        if (_tokens.TryDequeue(out token))
        {
            PrevToken = token;
            return true;
        }
        return false;
    }

    public Token Dequeue()
    {
        return _tokens.Dequeue();
    }

    public bool TryPeek([MaybeNullWhen(false)] out Token token)
    {
        return _tokens.TryPeek(out token);
    }
}
internal class Parser
{
    private record LengthResult(LengthType LengthType, bool IsDotted);

    private record ScaleResult(Scale Scale, int ScaleNumber);

    private record ParseResult<T>(T? Value, Error? Error) where T : class
    {
        public bool TryGetValue([MaybeNullWhen(false)] out T value)
        {
            if (Value is null || Error is not null)
            {
                value = null;
                return false;
            }
            value = Value;
            return true;
        }
    }

    private readonly TokenQueue _tokens;

    public Parser(IEnumerable<Token> tokens)
    {
        _tokens = new(tokens);
    }

    public List<ISoundComponent> Parse()
    {
        var statements = new List<ISoundComponent>();
        while (_tokens.Count > 0)
        {
            var statementResult = ParseStatement();
            if (statementResult.TryGetValue(out var statement))
            {
                statements.Add(statement);
            }
        }
        return statements;
    }

    private ParseResult<ISoundComponent> ParseStatement()
    {
        if (!_tokens.TryPeek(out var current))
        {
            return new(null, new(SMSCReadErrorType.UndefinedStatement, _tokens.PrevToken?.LineNumber ?? 0));
        }
        var statementResult = current.Type switch
        {
            TokenType.Tie => ParseTie(),
            TokenType.Tuplet => ParseTuplet(),
            TokenType.Rest => ParseRest(),
            _ => ParseNote()
        };
        return statementResult;
    }

    private ParseResult<ISoundComponent> ParseTie()
    {

        if (!_tokens.TryDequeue(out var current) || current.Type is not TokenType.Tie)
        {
            return new(null, new(SMSCReadErrorType.UndefinedStatement, _tokens.PrevToken?.LineNumber ?? 0));
        }
        if (!_tokens.TryDequeue(out current) || current.Type is not TokenType.LeftParentheses)
        {
            return new(null, new(SMSCReadErrorType.NotFoundLeftParentheses, _tokens.PrevToken?.LineNumber ?? 0));
        }
        var scaleResult = ParseScale();
        if (!scaleResult.TryGetValue(out var scale))
        {
            return new(null, scaleResult.Error);
        }

        var notes = new List<Note>();
        // 引数内の長さを表す情報を解析する
        while (_tokens.TryPeek(out current) && current.Type is not TokenType.RightParentheses)
        {
            if (current.Type is TokenType.Comma)
            {
                _ = _tokens.Dequeue();
            }
            else
            {
                return new(null, new(SMSCReadErrorType.NotFoundComma, current.LineNumber));
            }

            var lengthResult = ParseLength();
            if (!lengthResult.TryGetValue(out var length))
            {
                return new(null, lengthResult.Error);
            }

            var note = new Note(scale.Scale, scale.ScaleNumber, length.LengthType, length.IsDotted);
            notes.Add(note);
        }
        // トークンが空になっている場合、')'が存在していない
        if (_tokens.Count == 0)
        {
            return new(null, new(SMSCReadErrorType.NotFoundRightParentheses, _tokens.PrevToken?.LineNumber ?? 0));
        }
        // ')'を破棄
        _ = _tokens.Dequeue();

        Tie tie;
        if (notes.Count == 0)
        {
            return new(null, new(SMSCReadErrorType.UndefinedStatement, _tokens.PrevToken?.LineNumber ?? 0));
        }
        else if (notes.Count == 1)
        {
            tie = new Tie(notes[0], new List<Note>());
        }
        else
        {
            tie = new Tie(notes[0], notes.GetRange(1, notes.Count - 1));
        }

        return new(tie, null);
    }

    private ParseResult<ISoundComponent> ParseTuplet()
    {
        if (!_tokens.TryDequeue(out var current) || current.Type is not TokenType.Tuplet)
        {
            return new(null, new(SMSCReadErrorType.UndefinedStatement, _tokens.PrevToken?.LineNumber ?? 0));
        }
        if (!_tokens.TryDequeue(out current) || current.Type is not TokenType.LeftParentheses)
        {
            return new(null, new(SMSCReadErrorType.NotFoundLeftParentheses, _tokens.PrevToken?.LineNumber ?? 0));
        }
        var lengthResult = ParseLength();

        var components = new List<ISoundComponent>();
        while (_tokens.TryPeek(out current) && current.Type is not TokenType.RightParentheses)
        {
            if (current.Type is TokenType.Comma)
            {
                _ = _tokens.Dequeue();
            }
            else
            {
                return new(null, new(SMSCReadErrorType.NotFoundComma, current.LineNumber));
            }

            if (!_tokens.TryPeek(out current))
            {
                return new(null, new(SMSCReadErrorType.UndefinedStatement, _tokens.PrevToken?.LineNumber ?? 0));
            }

            if (current.Type is TokenType.Tie)
            {
                var componentResult = ParseTie();
                if (!componentResult.TryGetValue(out var component))
                {
                    return new(null, componentResult.Error);
                }
                components.Add(component);
            }
            else if (current.Type is TokenType.Tuplet)
            {
                var componentResult = ParseTuplet();
                if (!componentResult.TryGetValue(out var component))
                {
                    return new(null, componentResult.Error);
                }
                components.Add(component);
            }
            else if (current.Type is TokenType.Rest)
            {
                _ = _tokens.Dequeue();
                var isDottedRest = false;
                if (_tokens.TryPeek(out var dotToken) && dotToken.Type is TokenType.Dot)
                {
                    _ = _tokens.Dequeue();
                    isDottedRest = true;
                }
                var rest = new Rest(LengthType.Whole, isDottedRest);
                components.Add(rest);
            }
            else
            {
                var scaleResult = ParseScale();
                if (!scaleResult.TryGetValue(out var scale))
                {
                    return new(null, scaleResult.Error);
                }
                var isDottedNote = false;
                if (_tokens.TryPeek(out var dotToken) && dotToken.Type is TokenType.Dot)
                {
                    _ = _tokens.Dequeue();
                    isDottedNote = true;
                }
                var note = new Note(scale.Scale, scale.ScaleNumber, LengthType.Whole, isDottedNote);
                components.Add(note);
            }
        }

        // トークンが空になっている場合、')'が存在していない
        if (_tokens.Count == 0)
        {
            return new(null, new(SMSCReadErrorType.NotFoundRightParentheses, _tokens.PrevToken?.LineNumber ?? 0));
        }
        // ')'を破棄
        _ = _tokens.Dequeue();

        if (!lengthResult.TryGetValue(out var length))
        {
            return new(null, lengthResult.Error);
        }
        var tuplet = new Tuplet(components, length.LengthType, length.IsDotted);
        return new(tuplet, null);
    }

    private ParseResult<ISoundComponent> ParseNote()
    {
        var scaleResult = ParseScale();
        if (!scaleResult.TryGetValue(out var scale))
        {
            return new(null, scaleResult.Error);
        }
        if (!_tokens.TryDequeue(out var current) || current.Type is not TokenType.Comma)
        {
            return new(null, new Error(SMSCReadErrorType.NotFoundComma, _tokens.PrevToken?.LineNumber ?? 0));
        }
        var lengthResult = ParseLength();
        if (!lengthResult.TryGetValue(out var length))
        {
            return new(null, lengthResult.Error);
        }
        var note = new Note(scale.Scale, scale.ScaleNumber, length.LengthType, length.IsDotted);
        return new(note, null);
    }

    private ParseResult<ISoundComponent> ParseRest()
    {
        if (!_tokens.TryDequeue(out var current) || current.Type is not TokenType.Rest)
        {
            return new(null, new(SMSCReadErrorType.UndefinedStatement, _tokens.PrevToken?.LineNumber ?? 0));
        }
        if (!_tokens.TryDequeue(out current) || current.Type is not TokenType.Comma)
        {
            return new(null, new(SMSCReadErrorType.NotFoundComma, _tokens.PrevToken?.LineNumber ?? 0));
        }
        var lengthResult = ParseLength();
        if (!lengthResult.TryGetValue(out var length))
        {
            return new(null, lengthResult.Error);
        }
        var rest = new Rest(length.LengthType, length.IsDotted);
        return new(rest, null);
    }

    /// <summary>
    /// 音の高さを解析する。例) C4, C#4
    /// </summary>
    /// <returns></returns>
    private ParseResult<ScaleResult> ParseScale()
    {
        if (!_tokens.TryDequeue(out var current) || !_tokens.TryDequeue(out var next))
        {
            return new ParseResult<ScaleResult>(null, new Error(SMSCReadErrorType.InvalidScale, _tokens.PrevToken?.LineNumber ?? 0));
        }

        Scale? scaleNullable;
        if (current.Type is not TokenType.Alphabet)
        {
            return new ParseResult<ScaleResult>(null, new Error(SMSCReadErrorType.InvalidScale, current.LineNumber));
        }

        int scaleNumber;
        if (next.Type is TokenType.Sharp)
        {
            if (!_tokens.TryDequeue(out var nextNext))
            {
                return new ParseResult<ScaleResult>(null, new Error(SMSCReadErrorType.InvalidScale, next.LineNumber));
            }
            if (nextNext.Type is not TokenType.Number)
            {
                return new ParseResult<ScaleResult>(null, new Error(SMSCReadErrorType.InvalidScale, nextNext.LineNumber));
            }

            scaleNullable = current.Literal switch
            {
                "A" or "a" => Scale.ASharp,
                "C" or "c" => Scale.CSharp,
                "D" or "d" => Scale.DSharp,
                "F" or "f" => Scale.FSharp,
                "G" or "g" => Scale.GSharp,
                _ => null,
            };

            if (scaleNullable is null)
            {
                return new ParseResult<ScaleResult>(null, new Error(SMSCReadErrorType.InvalidScale, current.LineNumber));
            }

            scaleNumber = int.Parse(nextNext.Literal);

            var scaleResult = new ScaleResult(scaleNullable.Value, scaleNumber);
            return new(scaleResult, null);
        }
        else if (next.Type is TokenType.Number)
        {
            scaleNumber = int.Parse(next.Literal);
            scaleNullable = current.Literal switch
            {
                "A" or "a" => Scale.A,
                "B" or "b" => Scale.B,
                "C" or "c" => Scale.C,
                "D" or "d" => Scale.D,
                "E" or "e" => Scale.E,
                "F" or "f" => Scale.F,
                "G" or "g" => Scale.G,
                _ => null,
            };

            if (scaleNullable is null)
            {
                return new ParseResult<ScaleResult>(null, new Error(SMSCReadErrorType.InvalidScale, current.LineNumber));
            }

            var scaleResult = new ScaleResult(scaleNullable.Value, scaleNumber);
            return new(scaleResult, null);
        }

        return new(null, new(SMSCReadErrorType.InvalidScale, current.LineNumber));
    }

    private ParseResult<LengthResult> ParseLength()
    {
        LengthType? lengthTypeNullable;
        var isDotted = false;

        if (!_tokens.TryDequeue(out var current))
        {
            return new(null, new Error(SMSCReadErrorType.InvalidLength, _tokens.PrevToken?.LineNumber ?? 0));
        }

        if (current.Type is not TokenType.Number)
        {
            return new(null, new Error(SMSCReadErrorType.InvalidLength, current.LineNumber));
        }

        lengthTypeNullable = int.Parse(current.Literal) switch
        {
            1 => LengthType.Whole,
            2 => LengthType.Half,
            4 => LengthType.Quarter,
            8 => LengthType.Eighth,
            16 => LengthType.Sixteenth,
            32 => LengthType.ThirtySecond,
            64 => LengthType.SixtyFourth,
            _ => null,
        };

        if (_tokens.TryPeek(out current) && current.Type is TokenType.Dot)
        {
            if (_tokens.TryDequeue(out current) && current.Type is TokenType.Dot)
            {
                isDotted = true;
            }
        }

        if (lengthTypeNullable is null)
        {
            return new(null, new Error(SMSCReadErrorType.InvalidLength, _tokens.PrevToken?.LineNumber ?? 0));
        }

        var length = new LengthResult(lengthTypeNullable.Value, isDotted);
        return new(length, null);
    }
}
