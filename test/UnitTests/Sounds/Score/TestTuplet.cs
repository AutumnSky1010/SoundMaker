using SoundMaker.Sounds;
using SoundMaker.Sounds.Score;
using SoundMaker.Sounds.WaveTypes;

namespace SoundMakerTests.UnitTests.Sounds.Score;
public class TestTuplet
{
    private class SoundComponent : ISoundComponent
    {
        public ushort[] GenerateWave(SoundFormat format, int tempo, int length, WaveTypeBase waveType)
            => new ushort[0];

        public ushort[] GenerateWave(SoundFormat format, int tempo, WaveTypeBase waveType)
            => new ushort[0];

        public int GetWaveArrayLength(SoundFormat format, int tempo) => 0;
    }

    [Fact(DisplayName = "初期化が正しく行えるかのテスト")]
    public void InitializeTest()
    {
        bool isDotted = true;
        var components = new List<SoundComponent>()
        {
            new SoundComponent(),
            new SoundComponent(),
            new SoundComponent()
        };
        var tuplet = new Tuplet(components, LengthType.Whole, isDotted);
        Assert.Equal(components.Count, tuplet.Count);
        Assert.Equal(isDotted, tuplet.IsDotted);
    }

    [Fact(DisplayName = "インデックスでサウンドコンポーネントが取得されるかのテスト")]
    public void GetItemTest()
    {
        var components = new List<SoundComponent>()
        {
            new SoundComponent(),
            new SoundComponent(),
            new SoundComponent()
        };
        var tuplet = new Tuplet(components, LengthType.Whole);
        Assert.Equal(components.Count, tuplet.Count);
        Assert.Equal(components[2], tuplet[2]);
        Assert.Throws<IndexOutOfRangeException>(() => new Tuplet(components, LengthType.Whole)[-1]);
        Assert.Throws<IndexOutOfRangeException>(() => new Tuplet(components, LengthType.Whole)[3]);
    }
}
