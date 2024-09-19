using SoundMaker.Sounds;
using SoundMaker.Sounds.Score;
using SoundMaker.Sounds.SoundChannels;
using SoundMaker.Sounds.WaveTypes;
//using SoundMaker.Sounds.Score.Parsers;

namespace SoundMakerTests.UnitTests.Sounds.SoundChannels;
public class TestSoundChannelBase
{
    private class SoundChannelBaseDerived : SoundChannelBase
    {
        public SoundChannelBaseDerived(int tempo, SoundFormat format, PanType panType, int capacity) : base(tempo, format, panType, capacity)
        {
        }

        public override short[] GenerateWave()
        {
            return new short[0];
        }
    }

    private class SoundComponent : ISoundComponent
    {
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

    [Fact(DisplayName = "初期化が正しく行われているかをテストする。")]
    public void InitializeTest()
    {
        var tempo = 100;
        var format = new SoundFormat(SamplingFrequencyType.FourtyEightKHz, BitRateType.SixteenBit, ChannelType.Stereo);
        var panType = PanType.Both;
        var capacity = 1;
        var soundChannel = new SoundChannelBaseDerived(tempo, format, panType, capacity);
        Assert.Equal(soundChannel.Tempo, tempo);
        Assert.Equal(soundChannel.Format, format);
        Assert.Equal(soundChannel.PanType, panType);
        Assert.Equal(soundChannel.Capacity, capacity);

        _ = Assert.Throws<ArgumentOutOfRangeException>(() => new SoundChannelBaseDerived(-100, format, panType, capacity));
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => new SoundChannelBaseDerived(0, format, panType, capacity));

        _ = Assert.Throws<ArgumentOutOfRangeException>(() => new SoundChannelBaseDerived(tempo, format, panType, -1));
    }

    [Fact(DisplayName = "サウンドコンポーネントが末尾に追加されるかのテスト。")]
    public void AddTest()
    {
        var soundChannel = GetSoundChannel();

        var countOfBefore = soundChannel.ComponentCount;
        var lastSoundComponent = new SoundComponent();
        soundChannel.Add(new SoundComponent());
        soundChannel.Add(lastSoundComponent);
        Assert.NotEqual(countOfBefore, soundChannel.ComponentCount);
        Assert.Equal(soundChannel[1].GetHashCode(), lastSoundComponent.GetHashCode());
    }

    [Fact(DisplayName = "インデックスによるサウンドコンポーネントを取り除くテスト。例外が正しく投げられるかも調べる。")]
    public void RemoveAtTest()
    {
        var soundChannel = GetSoundChannel();

        var countOfBefore = soundChannel.ComponentCount;
        soundChannel.Add(new SoundComponent());
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => soundChannel.RemoveAt(-1));
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => soundChannel.RemoveAt(1));
        soundChannel.RemoveAt(0);
        Assert.Equal(0, soundChannel.ComponentCount);
    }

    [Fact(DisplayName = "チャンネル内のサウンドコンポーネントが全て削除されるかを調べる。")]
    public void ClearTest()
    {
        var soundChannel = GetSoundChannel();
        soundChannel.Add(new SoundComponent());
        soundChannel.Add(new SoundComponent());
        soundChannel.Add(new SoundComponent());
        soundChannel.Clear();
        Assert.Equal(0, soundChannel.ComponentCount);
    }

    /*
    [Fact(DisplayName = "楽譜のパーサをもとに、サウンドコンポーネントが追加されるかのテスト")]
    public void ImportScoreTest()
    {
        SoundChannelBase soundChannel = GetSoundChannel();

        IScoreParser scoreParser = new ScoreParser();
        soundChannel.ImportScore(scoreParser);
        (Scale scale, int scaleNum)[] scales = { (Scale.A, 2), (Scale.A, 3), (Scale.A, 4) };

        bool isValid = true;
        for (int i = 0; i < scales.Length && i < soundChannel.ComponentCount; i++) 
        {
            if (soundChannel[i] is not Note note || 
                note.Scale != scales[i].scale ||
                note.ScaleNumber != scales[i].scaleNum)
            {
                isValid = false; 
                break;
            }
        }
        Assert.True(isValid);
    }

    private class ScoreParser : IScoreParser
    {
        public IEnumerable<ISoundComponent> Parse()
        {
            return new List<ISoundComponent>()
            {
                new Note(Scale.A, 2, LengthType.Whole),
                new Note(Scale.A, 3, LengthType.Whole),
                new Note(Scale.A, 4, LengthType.Whole),
            };
        }
    }*/

    private SoundChannelBase GetSoundChannel()
    {
        var tempo = 100;
        var format = new SoundFormat(SamplingFrequencyType.FourtyEightKHz, BitRateType.SixteenBit, ChannelType.Stereo);
        var panType = PanType.Both;
        var capacity = 1;
        return new SoundChannelBaseDerived(tempo, format, panType, capacity);
    }
}
