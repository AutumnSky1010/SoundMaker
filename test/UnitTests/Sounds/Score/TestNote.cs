using SoundMaker.Sounds.Score;

namespace SoundMakerTests.UnitTests.Sounds.Score;
public class TestNote
{
    [Fact(DisplayName = "初期化が正しく行えるかのテスト")]
    public void InitializeTest()
    {
        var isDotted = true;
        var lengthType = LengthType.Whole;
        var scaleNum = 4;
        var scale = Scale.A;
        var note = new Note(scale, scaleNum, lengthType, isDotted);
        Assert.Equal(scale, note.Scale);
        Assert.Equal(scaleNum, note.ScaleNumber);
        Assert.Equal(lengthType, note.Length);
        Assert.Equal(isDotted, note.IsDotted);
        Assert.Equal(100, note.Volume);

        // 例外のテスト
        _ = Assert.Throws<ArgumentException>(() => new Note(Scale.G, 0, lengthType, isDotted));
        _ = Assert.Throws<ArgumentException>(() => new Note(Scale.D, 8, lengthType, isDotted));
        _ = Assert.Throws<ArgumentException>(() => new Note(Scale.G, 9, lengthType, isDotted));
        _ = Assert.Throws<ArgumentException>(() => new Note(Scale.G, -1, lengthType, isDotted));
    }
    [Fact(DisplayName = "簡単なコンストラクタで初期化が正しく行えるかのテスト")]
    public void EasinessInitializeTest()
    {
        var isDotted = true;
        var lengthType = LengthType.Whole;
        var note = new Note(lengthType, isDotted);

        Assert.Equal(isDotted, note.IsDotted);
        Assert.Equal(lengthType, note.Length);
        Assert.Equal(100, note.Volume);
    }

    [Fact(DisplayName = "音量が正しく変更されるかのテスト")]
    public void VolumeTest()
    {
        var note = new Note(LengthType.Half)
        {
            Volume = 101
        };
        Assert.Equal(100, note.Volume);
        note.Volume = -1;
        Assert.Equal(0, note.Volume);
        note.Volume = 100;
        Assert.Equal(100, note.Volume);
    }
}
