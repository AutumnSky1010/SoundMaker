using SoundMaker.Sounds;
using SoundMaker.Sounds.Score;
using SoundMaker.Sounds.WaveTypes;

namespace SoundMakerTests.UnitTests.Sounds.Score;
public class TestTuplet
{
    private class SoundComponent : ISoundComponent
    {
        public ISoundComponent Clone()
        {
            throw new NotImplementedException();
        }

        public short[] GenerateWave(SoundFormat format, int tempo, int length, WaveTypeBase waveType)
        {
            return new short[0];
        }

        public short[] GenerateWave(SoundFormat format, int tempo, WaveTypeBase waveType)
        {
            return new short[0];
        }

        public int GetWaveArrayLength(SoundFormat format, int tempo)
        {
            return 0;
        }
    }

    [Fact(DisplayName = "初期化が正しく行えるかのテスト")]
    public void InitializeTest()
    {
        var isDotted = true;
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
        _ = Assert.Throws<IndexOutOfRangeException>(() => new Tuplet(components, LengthType.Whole)[-1]);
        _ = Assert.Throws<IndexOutOfRangeException>(() => new Tuplet(components, LengthType.Whole)[3]);
    }
}
