using SoundMaker.ScoreData.SMSC;

namespace SoundMakerTests.UnitTests.ScoreData.SMSC;

public class LexerTest
{
    [Fact(DisplayName = "コメントは解析しないか")]
    public void TestReadAll_Comment()
    {
        var data = @"// Comment";
        var lexer = new Lexer(data);
        var tokens = lexer.ReadAll();
        Assert.Empty(tokens);
    }

    [Fact(DisplayName = "通常の音符を解析できるか")]
    public void TestReadAll_Note()
    {
        var data = @"// Note
C4, 16
";
        var lexer = new Lexer(data);
        var expected = new List<Token>()
        {
            new(TokenType.Alphabet, "C", 2),
            new(TokenType.Number, "4", 2),
            new(TokenType.Comma, ",", 2),
            new(TokenType.Number, "16", 2),
        };
        var tokens = lexer.ReadAll();
        Assert.Equal(expected.Count, tokens.Count);

        AssertToken(expected[0], tokens[0]);
        AssertToken(expected[1], tokens[1]);
        AssertToken(expected[2], tokens[2]);
        AssertToken(expected[3], tokens[3]);
    }

    [Fact(DisplayName = "付点音符を解析できるか")]
    public void TestReadAll_DottedNote()
    {
        var data = @"// Note
C4, 16.
";
        var lexer = new Lexer(data);
        var expected = new List<Token>()
        {
            new(TokenType.Alphabet, "C", 2),
            new(TokenType.Number, "4", 2),
            new(TokenType.Comma, ",", 2),
            new(TokenType.Number, "16", 2),
            new(TokenType.Dot, ".", 2),
        };
        var tokens = lexer.ReadAll();
        Assert.Equal(expected.Count, tokens.Count);

        AssertToken(expected[0], tokens[0]);
        AssertToken(expected[1], tokens[1]);
        AssertToken(expected[2], tokens[2]);
        AssertToken(expected[3], tokens[3]);
        AssertToken(expected[4], tokens[4]);
    }

    [Fact(DisplayName = "半音上の音符を解析できるか")]
    public void TestReadAll_SharpNote()
    {
        var data = @"// Note
C#4, 16
";
        var lexer = new Lexer(data);
        var expected = new List<Token>()
        {
            new(TokenType.Alphabet, "C", 2),
            new(TokenType.Sharp, "#", 2),
            new(TokenType.Number, "4", 2),
            new(TokenType.Comma, ",", 2),
            new(TokenType.Number, "16", 2),
        };
        var tokens = lexer.ReadAll();
        Assert.Equal(expected.Count, tokens.Count);

        AssertToken(expected[0], tokens[0]);
        AssertToken(expected[1], tokens[1]);
        AssertToken(expected[2], tokens[2]);
        AssertToken(expected[3], tokens[3]);
        AssertToken(expected[4], tokens[4]);
    }

    private void AssertToken(Token expected, Token actual)
    {
        Assert.Equal(expected.Type, actual.Type);
        Assert.Equal(expected.Literal, actual.Literal);
        Assert.Equal(expected.LineNumber, actual.LineNumber);
    }
}
