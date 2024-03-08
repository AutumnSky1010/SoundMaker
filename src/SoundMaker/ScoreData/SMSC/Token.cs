namespace SoundMaker.ScoreData.SMSC;
internal record class Token
{
    public Token(TokenType type, string literal, int lineNumber)
    {
        Type = type;
        Literal = literal;
        LineNumber = lineNumber;
    }

    public TokenType Type { get; }

    public string Literal { get; }

    public int LineNumber { get; }
}
