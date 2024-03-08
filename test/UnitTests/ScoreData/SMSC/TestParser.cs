using SoundMaker.ScoreData.SMSC;
using SoundMaker.Sounds.Score;

namespace SoundMakerTests.UnitTests.ScoreData.SMSC;

public class TestParser
{
    [Fact(DisplayName = "普通の音符を解析できるか")]
    public void TestParse_Note()
    {
        var data = @"C4, 16";
        var lexer = new Lexer(data);
        var tokens = lexer.ReadAll();
        var parser = new Parser(tokens);

        var expectedCollection = new List<ISoundComponent>()
        {
            new Note(Scale.C, 4, LengthType.Sixteenth),
        };
        var actualCollection = parser.Parse();
        Assert.Equal(expectedCollection.Count, actualCollection.Count);

        var expected = (Note)expectedCollection[0];
        var actual = Assert.IsType<Note>(actualCollection[0]);
        Assert.Equal(expected.Scale, actual.Scale);
        Assert.Equal(expected.ScaleNumber, actual.ScaleNumber);
        Assert.Equal(expected.Length, actual.Length);
        Assert.Equal(expected.IsDotted, actual.IsDotted);
    }

    [Fact(DisplayName = "付点音符を解析できるか")]
    public void TestParse_DottedNote()
    {
        var data = @"C4, 16.";
        var lexer = new Lexer(data);
        var tokens = lexer.ReadAll();
        var parser = new Parser(tokens);

        var expectedCollection = new List<ISoundComponent>()
        {
            new Note(Scale.C, 4, LengthType.Sixteenth, true),
        };
        var actualCollection = parser.Parse();
        Assert.Equal(expectedCollection.Count, actualCollection.Count);

        var expected = (Note)expectedCollection[0];
        var actual = Assert.IsType<Note>(actualCollection[0]);
        Assert.Equal(expected.Scale, actual.Scale);
        Assert.Equal(expected.ScaleNumber, actual.ScaleNumber);
        Assert.Equal(expected.Length, actual.Length);
        Assert.Equal(expected.IsDotted, actual.IsDotted);
    }

    [Fact(DisplayName = "半音上の音符を解析できるか")]
    public void TestParse_SharpNote()
    {
        var data = @"C#4, 16";
        var lexer = new Lexer(data);
        var tokens = lexer.ReadAll();
        var parser = new Parser(tokens);

        var expectedCollection = new List<ISoundComponent>()
        {
            new Note(Scale.CSharp, 4, LengthType.Sixteenth),
        };
        var actualCollection = parser.Parse();
        Assert.Equal(expectedCollection.Count, actualCollection.Count);

        var expected = (Note)expectedCollection[0];
        var actual = Assert.IsType<Note>(actualCollection[0]);
        Assert.Equal(expected.Scale, actual.Scale);
        Assert.Equal(expected.ScaleNumber, actual.ScaleNumber);
        Assert.Equal(expected.Length, actual.Length);
        Assert.Equal(expected.IsDotted, actual.IsDotted);
    }

    [Fact(DisplayName = "普通の休符を解析できるか")]
    public void TestParse_Rest()
    {
        var data = @"rest, 16";
        var lexer = new Lexer(data);
        var tokens = lexer.ReadAll();
        var parser = new Parser(tokens);

        var expectedCollection = new List<ISoundComponent>()
        {
            new Rest(LengthType.Sixteenth),
        };
        var actualCollection = parser.Parse();
        Assert.Equal(expectedCollection.Count, actualCollection.Count);

        var expected = (Rest)expectedCollection[0];
        var actual = Assert.IsType<Rest>(actualCollection[0]);
        Assert.Equal(expected.Length, actual.Length);
        Assert.Equal(expected.IsDotted, actual.IsDotted);
    }

    [Fact(DisplayName = "付点休符を解析できるか")]
    public void TestParse_DottedRest()
    {
        var data = @"rest, 16.";
        var lexer = new Lexer(data);
        var tokens = lexer.ReadAll();
        var parser = new Parser(tokens);

        var expectedCollection = new List<ISoundComponent>()
        {
            new Rest(LengthType.Sixteenth, true),
        };
        var actualCollection = parser.Parse();
        Assert.Equal(expectedCollection.Count, actualCollection.Count);

        var expected = (Rest)expectedCollection[0];
        var actual = Assert.IsType<Rest>(actualCollection[0]);
        Assert.Equal(expected.Length, actual.Length);
        Assert.Equal(expected.IsDotted, actual.IsDotted);
    }

    [Fact(DisplayName = "タイを解析できるか")]
    public void TestParse_Tie()
    {
        var data = @"tie(C#4, 16, 8, 8, 8.)";
        var lexer = new Lexer(data);
        var tokens = lexer.ReadAll();
        var parser = new Parser(tokens);

        var expectedCollection = new List<ISoundComponent>()
        {
            new Tie(new(Scale.CSharp, 4, LengthType.Sixteenth), new List<Note>()
            {
                new(LengthType.Eighth),
                new(LengthType.Eighth),
                new(LengthType.Eighth, true),
            }),
        };
        var actualCollection = parser.Parse();
        Assert.Equal(expectedCollection.Count, actualCollection.Count);

        var expected = (Tie)expectedCollection[0];
        var actual = Assert.IsType<Tie>(actualCollection[0]);
        Assert.Equal(expected.Count, actual.Count);
    }

    [Fact(DisplayName = "連符を解析できるか")]
    public void TestParse_Tuplet()
    {
        var data = @"tup(16, C#4, rest, E4.)";
        var lexer = new Lexer(data);
        var tokens = lexer.ReadAll();
        var parser = new Parser(tokens);

        var expectedCollection = new List<ISoundComponent>()
        {
            new Tuplet(new List<ISoundComponent>()
            {
                new Note(Scale.CSharp, 4, LengthType.Whole),
                new Rest(LengthType.Eighth),
                new Note(Scale.E, 4,LengthType.Whole, true),
            }, LengthType.Sixteenth),
        };
        var actualCollection = parser.Parse();
        Assert.Equal(expectedCollection.Count, actualCollection.Count);

        var expected = (Tuplet)expectedCollection[0];
        var actual = Assert.IsType<Tuplet>(actualCollection[0]);
        Assert.Equal(expected.Count, actual.Count);
    }

    [Fact(DisplayName = "連符内のタイや連符を解析できるか")]
    public void TestParse_TupletInTupletAndTie()
    {
        var data = @"tup(16, tup(16, rest, rest), tie(C#4, 16, 8, 8, 8.))";
        var lexer = new Lexer(data);
        var tokens = lexer.ReadAll();
        var parser = new Parser(tokens);

        var rest = new Rest(LengthType.Eighth);
        var expectedCollection = new List<ISoundComponent>()
        {
            new Tuplet(new List<ISoundComponent>()
            {
                new Tuplet(new List<ISoundComponent>()
                {
                    rest, rest
                }, LengthType.Sixteenth),
                new Tie(new(Scale.CSharp, 4, LengthType.Sixteenth), new List<Note>()
                {
                    new(LengthType.Eighth),
                    new(LengthType.Eighth),
                    new(LengthType.Eighth, true),
                }),
            }, LengthType.Sixteenth),
        };
        var actualCollection = parser.Parse();
        Assert.Equal(expectedCollection.Count, actualCollection.Count);

        var expected = (Tuplet)expectedCollection[0];
        var actual = Assert.IsType<Tuplet>(actualCollection[0]);
        Assert.Equal(expected.Count, actual.Count);
    }
}
