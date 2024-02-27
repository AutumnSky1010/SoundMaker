namespace SoundMaker.ScoreData.SMSC;
internal enum TokenType
{
    // Symbols
    Dot,              // .
    Sharp,            // #
    LeftParentheses,  // (
    RightParentheses, // )
    Comma,            // ,
    // Characters
    Alphabet,
    Number,
    // keywords
    Tie,
    Tuplet,
    Rest,
    // ?
    Unknown,
}
