using SoundMaker;
using SoundMaker.Sounds;
using SoundMaker.Sounds.Score;
using SoundMaker.Sounds.WaveTypes;

namespace SoundMakerTests.UnitTests.Sounds;

file class SoundComponentDouble : ISoundComponent
{
    public ISoundComponent Clone()
    {
        return new SoundComponentDouble();
    }

    public short[] GenerateWave(SoundFormat format, int tempo, int length, WaveTypeBase waveType)
    {
        return [];
    }

    public short[] GenerateWave(SoundFormat format, int tempo, WaveTypeBase waveType)
    {
        return [0];
    }

    public int GetWaveArrayLength(SoundFormat format, int tempo)
    {
        return 1;
    }
}

public class TrackTest
{
    [Fact(DisplayName = "サウンドコンポーネントをインポートできるか")]
    public void Import()
    {
        var track = CreateTrack();

        var additionalComponent0 = new SoundComponentDouble();
        var additionalComponent1 = new SoundComponentDouble();

        track.Import([additionalComponent0, additionalComponent1]);

        Assert.Equal(additionalComponent0, track.SoundComponents[0]);
        Assert.Equal(additionalComponent1, track.SoundComponents[1]);
    }

    [Fact(DisplayName = "生成した波形の長さがWaveArrayLengthと同じ長さか")]
    public void GenerateWave()
    {
        var track = CreateTrack();
        var dummyComponent = new SoundComponentDouble();
        track.Import([dummyComponent, dummyComponent, dummyComponent]);

        var wave = track.GenerateWave();

        Assert.Equal(track.WaveArrayLength, wave.Length);
    }

    [Fact(DisplayName = "コンポーネントを追加できるか")]
    public void Add()
    {
        var track = CreateTrack();
        var additionalComponent = new SoundComponentDouble();
        track.Add(additionalComponent);

        Assert.Equal(additionalComponent, track.SoundComponents[0]);
    }

    [Fact(DisplayName = "コンポーネントを挿入できるか。例外が正しく投げられるか")]
    public void Insert()
    {
        var track = CreateTrack();

        var dummyComponent = new SoundComponentDouble();
        var insertTargetComponent = new SoundComponentDouble();
        // トラックにダミーのコンポーネントを追加
        track.Import([dummyComponent, dummyComponent]);
        var insertMaybeOkIndex = 1;
        track.Insert(1, insertTargetComponent);

        Assert.Equal(insertTargetComponent, track.SoundComponents[insertMaybeOkIndex]);

        Assert.Throws<ArgumentOutOfRangeException>(() => track.Insert(-1, dummyComponent));
        Assert.Throws<ArgumentOutOfRangeException>(() => track.Insert(4, dummyComponent));
    }

    [Fact(DisplayName = "コンポーネントをインデクスで削除できるか。例外が正しく投げられるか")]
    public void RemoveAt()
    {
        var track = CreateTrack();

        var dummyComponent = new SoundComponentDouble();

        track.Add(dummyComponent);

        track.RemoveAt(0);

        Assert.Empty(track.SoundComponents);

        track.Add(dummyComponent);
        Assert.Throws<ArgumentOutOfRangeException>(() => track.RemoveAt(-1));
        Assert.Throws<ArgumentOutOfRangeException>(() => track.RemoveAt(1));
    }

    [Fact(DisplayName = "サウンドコンポーネントを削除できるか")]
    public void Remove()
    {
        var track = CreateTrack();
        var dummyComponent = new SoundComponentDouble();
        track.Add(dummyComponent);
        track.Remove(dummyComponent);

        Assert.Empty(track.SoundComponents);
    }

    [Fact(DisplayName = "サウンドコンポーネントをクリアできるか")]
    public void Clear()
    {
        var track = CreateTrack();
        var dummyComponent = new SoundComponentDouble();

        track.Import([dummyComponent, dummyComponent, dummyComponent]);

        track.Clear();

        Assert.Empty(track.SoundComponents);
    }



    private Track CreateTrack()
    {
        var format = FormatBuilder.Create()
            .WithFrequency(48000)
            .WithBitDepth(16)
            .WithChannelCount(2)
            .ToSoundFormat();
        return new Track(new TriangleWave(), format, 100, 0);
    }
}
