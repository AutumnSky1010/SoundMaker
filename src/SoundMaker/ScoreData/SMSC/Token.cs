namespace SoundMaker.ScoreData.SMSC;
internal record class Token
{
    public Token(TokenType type, string literal)
    {
        Type = type;
        Literal = literal;
    }

    public TokenType Type { get; }

    public string Literal { get; }
}
