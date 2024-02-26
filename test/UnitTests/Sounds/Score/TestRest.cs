using SoundMaker.Sounds.Score;

namespace SoundMakerTests.UnitTests.Sounds.Score;
public class TestRest
{
    [Fact(DisplayName = "")]
    public void InitializeTest()
    {
        var isDotted = false;
        var length = LengthType.Eighth;
        var rest = new Rest(length, isDotted);
        Assert.Equal(length, rest.Length);
        Assert.Equal(isDotted, rest.IsDotted);
    }
}
