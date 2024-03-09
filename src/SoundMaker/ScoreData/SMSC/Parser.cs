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

    /// <summary>
    /// 解析する
    /// </summary>
    /// <returns>解析結果</returns>
    public List<ISoundComponent> Parse()
    {
        var statements = new List<ISoundComponent>();
        _ = new List<Error>();
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

    /// <summary>
    /// 文を解析する
    /// </summary>
    /// <returns>解析結果</returns>
    private ParseResult<ISoundComponent> ParseStatement()
    {
        // 現在のトークンの種類を見たいだけなのでPeekする。
        if (!_tokens.TryPeek(out var current))
        {
            return new(null, new(SMSCReadErrorType.UndefinedStatement, _tokens.PrevToken?.LineNumber ?? 0));
        }
        // 文の解析を現在のトークンに基づいて行う。
        var statementResult = current.Type switch
        {
            TokenType.Tie => ParseTie(),
            TokenType.Tuplet => ParseTuplet(),
            TokenType.Rest => ParseRest(),
            // 上記に当てはまらない場合は音符として解析する。
            _ => ParseNote()
        };
        return statementResult;
    }

    /// <summary>
    /// タイを解析する
    /// </summary>
    /// <returns>解析結果</returns>
    private ParseResult<ISoundComponent> ParseTie()
    {
        // トークンが'tie'かを確認する
        if (!_tokens.TryDequeue(out var current) || current.Type is not TokenType.Tie)
        {
            return new(null, new(SMSCReadErrorType.UndefinedStatement, _tokens.PrevToken?.LineNumber ?? 0));
        }
        // トークンが'('かを確認する
        if (!_tokens.TryDequeue(out current) || current.Type is not TokenType.LeftParentheses)
        {
            return new(null, new(SMSCReadErrorType.NotFoundLeftParentheses, _tokens.PrevToken?.LineNumber ?? 0));
        }
        // 音程を解析する
        var scaleResult = ParseScale();
        if (!scaleResult.TryGetValue(out var scale))
        {
            return new(null, scaleResult.Error);
        }

        // 引数内の長さを表す情報を解析する。トークンがなくなるか')'になるまで解析する。
        // 長さの情報は可変長引数として取る。
        var notes = new List<Note>();
        while (_tokens.TryPeek(out current) && current.Type is not TokenType.RightParentheses)
        {
            // ','の場合はDequeueする。tie(音程, 長さ, 長さ, ...)という並びなので、while文の先頭にこの処理を置く。
            // 音程解析はwhileの前で実装済みなので、", 長さ, 長さ, ..."という部分をここで解析する。
            if (current.Type is TokenType.Comma)
            {
                _ = _tokens.Dequeue();
            }
            else
            {
                return new(null, new(SMSCReadErrorType.NotFoundComma, current.LineNumber));
            }

            // 長さを解析する。
            var lengthResult = ParseLength();
            if (!lengthResult.TryGetValue(out var length))
            {
                return new(null, lengthResult.Error);
            }

            // 音符を作成する。
            var note = new Note(scale.Scale, scale.ScaleNumber, length.LengthType, length.IsDotted);
            notes.Add(note);
        }
        // トークンが空になっている場合、')'が存在していないので、エラーを出力。
        if (_tokens.Count == 0)
        {
            return new(null, new(SMSCReadErrorType.NotFoundRightParentheses, _tokens.PrevToken?.LineNumber ?? 0));
        }
        // ')'を破棄
        _ = _tokens.Dequeue();

        // タイを作成する。
        Tie tie;
        // 音符が0個の場合は解析できていない。
        if (notes.Count == 0)
        {
            return new(null, new(SMSCReadErrorType.UndefinedStatement, _tokens.PrevToken?.LineNumber ?? 0));
        }
        // 個数が1個の場合はadditionalNotesに空配列を渡す
        else if (notes.Count == 1)
        {
            tie = new Tie(notes[0], Array.Empty<Note>());
        }
        // 個数が2個以上の場合は、先頭をbaseとし、残りをadditionalNotesとする。
        else
        {
            tie = new Tie(notes[0], notes.GetRange(1, notes.Count - 1));
        }
        // 解析結果を返す。
        return new(tie, null);
    }

    /// <summary>
    /// 連符を解析する
    /// </summary>
    /// <returns>解析結果</returns>
    private ParseResult<ISoundComponent> ParseTuplet()
    {
        // トークンが'tup'かを確認する
        if (!_tokens.TryDequeue(out var current) || current.Type is not TokenType.Tuplet)
        {
            return new(null, new(SMSCReadErrorType.UndefinedStatement, _tokens.PrevToken?.LineNumber ?? 0));
        }
        // トークンが'('かを確認する
        if (!_tokens.TryDequeue(out current) || current.Type is not TokenType.LeftParentheses)
        {
            return new(null, new(SMSCReadErrorType.NotFoundLeftParentheses, _tokens.PrevToken?.LineNumber ?? 0));
        }
        // 長さを解析する
        var lengthResult = ParseLength();

        // 連符にするSoundComponentを解析する
        var components = new List<ISoundComponent>();
        // 引数内の長さを表す情報を解析する。トークンがなくなるか')'になるまで解析する。
        // 情報は可変長引数として取る。
        while (_tokens.TryPeek(out current) && current.Type is not TokenType.RightParentheses)
        {
            // ','の場合はDequeueする。tup(長さ, 音の部品, 音の部品, ...)という並びなので、while文の先頭にこの処理を置く。
            // 長さの解析はwhileの前で実装済みなので、", 音の部品, 音の部品, ..."という部分をここで解析する。
            if (current.Type is TokenType.Comma)
            {
                _ = _tokens.Dequeue();
            }
            else
            {
                return new(null, new(SMSCReadErrorType.NotFoundComma, current.LineNumber));
            }

            // 条件分岐の為にトークンをピークする
            if (!_tokens.TryPeek(out current))
            {
                return new(null, new(SMSCReadErrorType.UndefinedStatement, _tokens.PrevToken?.LineNumber ?? 0));
            }

            // タイの場合はタイを解析する
            if (current.Type is TokenType.Tie)
            {
                var componentResult = ParseTie();
                if (!componentResult.TryGetValue(out var component))
                {
                    return new(null, componentResult.Error);
                }
                components.Add(component);
            }
            // 連符の場合は連符を解析(再帰呼び出し)する。
            else if (current.Type is TokenType.Tuplet)
            {
                var componentResult = ParseTuplet();
                if (!componentResult.TryGetValue(out var component))
                {
                    return new(null, componentResult.Error);
                }
                components.Add(component);
            }
            // 休符の場合は休符を解析するが、連符専用の書き方なのでここで実装する。
            else if (current.Type is TokenType.Rest)
            {
                // 'rest'のトークンを破棄
                _ = _tokens.Dequeue();
                var isDottedRest = false;
                // '.'の場合は付点休符とする
                if (_tokens.TryPeek(out var dotToken) && dotToken.Type is TokenType.Dot)
                {
                    _ = _tokens.Dequeue();
                    isDottedRest = true;
                }
                // 連符なので長さは適当に入れる
                var rest = new Rest(LengthType.Whole, isDottedRest);
                components.Add(rest);
            }
            // 上記以外は音符として解析するが、連符専用の書き方なのでここで実装する。
            else
            {
                // 音程を解析する
                var scaleResult = ParseScale();
                if (!scaleResult.TryGetValue(out var scale))
                {
                    return new(null, scaleResult.Error);
                }
                var isDottedNote = false;
                // '.'の場合は付点音符とする
                if (_tokens.TryPeek(out var dotToken) && dotToken.Type is TokenType.Dot)
                {
                    _ = _tokens.Dequeue();
                    isDottedNote = true;
                }
                // 連符なので長さは適当に入れる
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

    /// <summary>
    /// 音符を解析する
    /// </summary>
    /// <returns>解析結果</returns>
    private ParseResult<ISoundComponent> ParseNote()
    {
        // 音程を解析する
        var scaleResult = ParseScale();
        if (!scaleResult.TryGetValue(out var scale))
        {
            return new(null, scaleResult.Error);
        }
        // ','かを判定する
        if (!_tokens.TryDequeue(out var current) || current.Type is not TokenType.Comma)
        {
            return new(null, new Error(SMSCReadErrorType.NotFoundComma, _tokens.PrevToken?.LineNumber ?? 0));
        }
        // 長さを解析する
        var lengthResult = ParseLength();
        if (!lengthResult.TryGetValue(out var length))
        {
            return new(null, lengthResult.Error);
        }
        var note = new Note(scale.Scale, scale.ScaleNumber, length.LengthType, length.IsDotted);
        return new(note, null);
    }

    /// <summary>
    /// 休符を解析する
    /// </summary>
    /// <returns></returns>
    private ParseResult<ISoundComponent> ParseRest()
    {
        // 'rest'かを判定する
        if (!_tokens.TryDequeue(out var current) || current.Type is not TokenType.Rest)
        {
            return new(null, new(SMSCReadErrorType.UndefinedStatement, _tokens.PrevToken?.LineNumber ?? 0));
        }
        // ','かを判定する
        if (!_tokens.TryDequeue(out current) || current.Type is not TokenType.Comma)
        {
            return new(null, new(SMSCReadErrorType.NotFoundComma, _tokens.PrevToken?.LineNumber ?? 0));
        }
        // 長さを解析する
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
        // 最低でもトークンが2つ取れないと解析できないので、ここで判定する。
        if (!_tokens.TryDequeue(out var current) || !_tokens.TryDequeue(out var next))
        {
            return new(null, new Error(SMSCReadErrorType.InvalidScale, _tokens.PrevToken?.LineNumber ?? 0));
        }

        Scale? scaleNullable;
        // 音程はアルファベットで書かれている
        if (current.Type is not TokenType.Alphabet)
        {
            return new(null, new Error(SMSCReadErrorType.InvalidScale, current.LineNumber));
        }

        int scaleNumber;
        // 次のトークンが'#'の場合は半音上の音程として解析する
        if (next.Type is TokenType.Sharp)
        {
            if (!_tokens.TryDequeue(out var nextNext))
            {
                return new(null, new Error(SMSCReadErrorType.InvalidScale, next.LineNumber));
            }
            if (nextNext.Type is not TokenType.Number)
            {
                return new(null, new Error(SMSCReadErrorType.InvalidScale, nextNext.LineNumber));
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
                return new(null, new Error(SMSCReadErrorType.InvalidScale, current.LineNumber));
            }

            scaleNumber = int.Parse(nextNext.Literal);

            var scaleResult = new ScaleResult(scaleNullable.Value, scaleNumber);
            return new(scaleResult, null);
        }
        // 次のトークンが数字の場合はナチュラルな音程
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
                return new(null, new Error(SMSCReadErrorType.InvalidScale, current.LineNumber));
            }

            var scaleResult = new ScaleResult(scaleNullable.Value, scaleNumber);
            return new(scaleResult, null);
        }
        // 上記条件に当てはまらない場合はエラー
        return new(null, new(SMSCReadErrorType.InvalidScale, current.LineNumber));
    }

    /// <summary>
    /// 長さを解析する
    /// </summary>
    /// <returns>解析結果</returns>
    private ParseResult<LengthResult> ParseLength()
    {
        LengthType? lengthTypeNullable;
        var isDotted = false;

        if (!_tokens.TryDequeue(out var current))
        {
            return new(null, new Error(SMSCReadErrorType.InvalidLength, _tokens.PrevToken?.LineNumber ?? 0));
        }

        // 数字かを判定する
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

        // 次のトークンが'.'だった場合は付点音符とする。
        if (_tokens.TryPeek(out current) && current.Type is TokenType.Dot)
        {
            // '.'を破棄
            _ = _tokens.Dequeue();
            isDotted = true;
        }

        // 不正な長さの場合
        if (lengthTypeNullable is null)
        {
            return new(null, new Error(SMSCReadErrorType.InvalidLength, _tokens.PrevToken?.LineNumber ?? 0));
        }

        var length = new LengthResult(lengthTypeNullable.Value, isDotted);
        return new(length, null);
    }
}
