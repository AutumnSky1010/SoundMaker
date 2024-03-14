namespace SoundMaker.ScoreData.SMSC;
internal enum TokenType
{
    // Symbols
    Dot,
    Sharp,
    LeftParentheses,
    RightParentheses,
    Semicolon,
    Comma,
    // Characters
    Alphabet,
    Number,
    // keywords
    Tie,
    Tuplet,
    Rest,
    // line break
    LineBreak,
    // ?
    Unknown,
}
