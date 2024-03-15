using System.Diagnostics.CodeAnalysis;

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
