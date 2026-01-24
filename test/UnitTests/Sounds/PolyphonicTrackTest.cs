using SoundMaker;
using SoundMaker.Sounds;
using SoundMaker.Sounds.Score;
using SoundMaker.Sounds.WaveTypes;

namespace SoundMakerTests.UnitTests.Sounds;

file class SoundComponentStub : ISoundComponent
{
    private readonly int _length;
    private readonly short _value;

    public SoundComponentStub(int length = 2, short value = 1)
    {
        _length = length;
        _value = value;
    }

    public ISoundComponent Clone()
    {
        return new SoundComponentStub(_length, _value);
    }

    public short[] GenerateWave(SoundFormat format, int tempo, int length, WaveTypeBase waveType)
    {
        return [];
    }

    public short[] GenerateWave(SoundFormat format, int tempo, WaveTypeBase waveType)
    {
        var result = new short[_length];
        for (int i = 0; i < _length; i++)
        {
            result[i] = _value;
        }
        return result;
    }

    public int GetWaveArrayLength(SoundFormat format, int tempo)
    {
        return _length;
    }
}

public class PolyphonicTrackTest
{
    private static readonly int _samplingFrequency = 48000;

    [Fact(DisplayName = "同じ位置にサウンドコンポーネントを追加できるか")]
    public void AddAt_SamePosition()
    {
        var track = CreatePolyphonicTrack();
        var component1 = new SoundComponentStub();
        var component2 = new SoundComponentStub();

        track.AddAt(0, component1);
        track.AddAt(0, component2);

        var components = track.GetComponentsAt(0);
        Assert.Equal(2, components.Count);
        Assert.Contains(component1, components);
        Assert.Contains(component2, components);
    }

    [Fact(DisplayName = "異なる位置にサウンドコンポーネントを追加できるか")]
    public void AddAt_DifferentPositions()
    {
        var track = CreatePolyphonicTrack();
        var component1 = new SoundComponentStub();
        var component2 = new SoundComponentStub();

        track.AddAt(0, component1);
        track.AddAt(10, component2);

        Assert.Single(track.GetComponentsAt(0));
        Assert.Single(track.GetComponentsAt(10));
        Assert.Equal(2, track.Count);
    }

    [Fact(DisplayName = "負の位置に追加すると例外がスローされるか")]
    public void AddAt_NegativePosition_ThrowsException()
    {
        var track = CreatePolyphonicTrack();
        var component = new SoundComponentStub();

        Assert.Throws<ArgumentOutOfRangeException>(() => track.AddAt(-1, component));
    }

    [Fact(DisplayName = "サウンドコンポーネントを削除できるか")]
    public void RemoveAt()
    {
        var track = CreatePolyphonicTrack();
        var component = new SoundComponentStub();
        track.AddAt(0, component);

        var removed = track.RemoveAt(0, component);

        Assert.True(removed);
        Assert.Empty(track.GetComponentsAt(0));
        Assert.Equal(0, track.Count);
    }

    [Fact(DisplayName = "存在しないコンポーネントの削除はfalseを返すか")]
    public void RemoveAt_NotFound()
    {
        var track = CreatePolyphonicTrack();
        var component = new SoundComponentStub();

        var removed = track.RemoveAt(0, component);

        Assert.False(removed);
    }

    [Fact(DisplayName = "全てのコンポーネントをクリアできるか")]
    public void Clear()
    {
        var track = CreatePolyphonicTrack();
        track.AddAt(0, new SoundComponentStub());
        track.AddAt(10, new SoundComponentStub());

        track.Clear();

        Assert.Equal(0, track.Count);
        Assert.Empty(track.GetPositions());
    }

    [Fact(DisplayName = "WaveArrayLengthが正しく計算されるか")]
    public void WaveArrayLength()
    {
        var track = CreatePolyphonicTrack();
        var component1 = new SoundComponentStub(length: 10);
        var component2 = new SoundComponentStub(length: 5);

        track.AddAt(0, component1);
        track.AddAt(5, component2);

        // component1: position 0, length 10 -> ends at 10
        // component2: position 5, length 5 -> ends at 10
        Assert.Equal(10, track.WaveArrayLength);
    }

    [Fact(DisplayName = "WaveArrayLengthが最も遠い終了位置を返すか")]
    public void WaveArrayLength_ReturnsMaxEndPosition()
    {
        var track = CreatePolyphonicTrack();
        var component1 = new SoundComponentStub(length: 5);
        var component2 = new SoundComponentStub(length: 5);

        track.AddAt(0, component1);
        track.AddAt(10, component2);

        // component1: ends at 5
        // component2: ends at 15
        Assert.Equal(15, track.WaveArrayLength);
    }

    [Fact(DisplayName = "EndIndexが正しく計算されるか")]
    public void EndIndex()
    {
        var track = CreatePolyphonicTrack(startIndex: 100);
        var component = new SoundComponentStub(length: 10);
        track.AddAt(0, component);

        // StartIndex=100, WaveArrayLength=10 -> EndIndex=109
        Assert.Equal(109, track.EndIndex);
    }

    [Fact(DisplayName = "空のトラックのEndIndexはStartIndexと同じか")]
    public void EndIndex_EmptyTrack()
    {
        var track = CreatePolyphonicTrack(startIndex: 100);

        Assert.Equal(100, track.EndIndex);
    }

    [Fact(DisplayName = "波形を正しく生成できるか")]
    public void GenerateWave()
    {
        var track = CreatePolyphonicTrack();
        var component = new SoundComponentStub(length: 4, value: 100);
        track.AddAt(0, component);

        var wave = track.GenerateWave();

        Assert.Equal(4, wave.Length);
        // Only one component, so max concurrent is 1, value should be 100/1 = 100
        Assert.All(wave, v => Assert.Equal(100, v));
    }

    [Fact(DisplayName = "同時発音で波形がミックスされるか")]
    public void GenerateWave_MixesConcurrentSounds()
    {
        var track = CreatePolyphonicTrack();
        var component1 = new SoundComponentStub(length: 4, value: 100);
        var component2 = new SoundComponentStub(length: 4, value: 100);

        track.AddAt(0, component1);
        track.AddAt(0, component2);

        var wave = track.GenerateWave();

        Assert.Equal(4, wave.Length);
        // 2 concurrent sounds, so each contributes 100/2 = 50, total = 100
        Assert.All(wave, v => Assert.Equal(100, v));
    }

    [Fact(DisplayName = "異なる位置の波形が正しく配置されるか")]
    public void GenerateWave_DifferentPositions()
    {
        var track = CreatePolyphonicTrack();
        var component1 = new SoundComponentStub(length: 2, value: 100);
        var component2 = new SoundComponentStub(length: 2, value: 100);

        track.AddAt(0, component1);
        track.AddAt(2, component2);

        var wave = track.GenerateWave();

        Assert.Equal(4, wave.Length);
        // No overlap, each sound plays sequentially, max concurrent is 1
        Assert.All(wave, v => Assert.Equal(100, v));
    }

    [Fact(DisplayName = "部分波形を正しく生成できるか")]
    public void GeneratePartialWave()
    {
        var track = CreatePolyphonicTrack();
        var component = new SoundComponentStub(length: 10, value: 100);
        track.AddAt(0, component);

        var wave = track.GeneratePartialWave(2, 5);

        Assert.Equal(4, wave.Length);
        Assert.All(wave, v => Assert.Equal(100, v));
    }

    [Fact(DisplayName = "クローンが正しく作成されるか")]
    public void Clone()
    {
        var track = CreatePolyphonicTrack(startIndex: 50);
        track.Pan = 0.5;
        track.AddAt(0, new SoundComponentStub());
        track.AddAt(10, new SoundComponentStub());

        var clone = track.Clone();

        Assert.NotSame(track, clone);
        Assert.Equal(track.StartIndex, clone.StartIndex);
        Assert.Equal(track.Pan, clone.Pan);
        Assert.Equal(track.Count, clone.Count);
        Assert.Equal(track.WaveArrayLength, clone.WaveArrayLength);
    }

    [Fact(DisplayName = "TrackBaseSoundからPolyphonicTrackを作成できるか")]
    public void CreateFromTrackBaseSound()
    {
        var format = CreateFormat();
        var sound = new TrackBaseSound(format, 100);

        var poly = sound.CreatePolyphonicTrack(0, new TriangleWave());

        Assert.NotNull(poly);
        Assert.Equal(0, poly.StartIndex);
    }

    [Fact(DisplayName = "TrackBaseSoundでPolyphonicTrackを使用して波形を生成できるか")]
    public void TrackBaseSound_GenerateWaveWithPolyphonicTrack()
    {
        var format = CreateFormat();
        var sound = new TrackBaseSound(format, 100);

        var poly = sound.CreatePolyphonicTrack(0, new TriangleWave());
        poly.AddAt(0, new SoundComponentStub(length: 4, value: 100));

        var wave = sound.GenerateMonauralWave();

        Assert.NotNull(wave);
        Assert.True(wave.GetWave().Length > 0);
    }

    [Fact(DisplayName = "GetPositionsが全ての位置を返すか")]
    public void GetPositions()
    {
        var track = CreatePolyphonicTrack();
        track.AddAt(0, new SoundComponentStub());
        track.AddAt(5, new SoundComponentStub());
        track.AddAt(10, new SoundComponentStub());

        var positions = track.GetPositions().ToList();

        Assert.Equal(3, positions.Count);
        Assert.Contains(0, positions);
        Assert.Contains(5, positions);
        Assert.Contains(10, positions);
    }

    private PolyphonicTrack CreatePolyphonicTrack(int startIndex = 0)
    {
        var format = CreateFormat();
        var sound = new TrackBaseSound(format, 100);
        return sound.CreatePolyphonicTrack(startIndex, new TriangleWave());
    }

    private SoundFormat CreateFormat()
    {
        return FormatBuilder.Create()
            .WithFrequency(_samplingFrequency)
            .WithBitDepth(16)
            .WithChannelCount(2)
            .ToSoundFormat();
    }
}
