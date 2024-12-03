using SoundMaker;
using SoundMaker.Sounds;
using SoundMaker.Sounds.SoundChannels;
using SoundMaker.Sounds.WaveTypes;

namespace SoundMakerTests.UnitTests.Sounds;
public class TrackBaseSoundTest
{
    [Fact(DisplayName = "すべてのトラックを取得できるかをテストする。")]
    public void GetAllTracks()
    {
        var format = FormatBuilder.Create()
            .WithFrequency(44100)
            .WithBitDepth(8)
            .WithChannelCount(1)
            .ToSoundFormat();
        var sound = new TrackBaseSound(format, 100);
        var wave = new SquareWave(SquareWaveRatio.Point25);
        var track1 = sound.CreateTrack(0, wave);
        var track2 = sound.CreateTrack(1000, wave);

        var actual = sound.GetAllTracks().ToArray();

        Assert.Equal(2, actual.Length);
        Assert.Equal(track1, actual[0]);
        Assert.True(track2.Equals(actual[1]));
    }

    [Fact(DisplayName = "指定したミリ秒から開始するトラックをすべて削除できるか")]
    public void RemoveTracksAt()
    {
        var format = FormatBuilder.Create()
            .WithFrequency(44100)
            .WithBitDepth(8)
            .WithChannelCount(1)
            .ToSoundFormat();
        // 0ミリ秒開始のトラックを削除する対象とする
        var targetStartMS = 0;
        var sound = new TrackBaseSound(format, 100);
        var wave = new SquareWave(SquareWaveRatio.Point25);
        _ = sound.CreateTrack(targetStartMS, wave);
        _ = sound.CreateTrack(targetStartMS, wave);
        var track1 = sound.CreateTrack(1000, wave);

        sound.RemoveTracksAt(targetStartMS);

        Assert.False(sound.TryGetTracks(targetStartMS, out _));
        Assert.Contains(track1, sound.GetAllTracks());
    }

    [Fact(DisplayName = "トラックの削除を正しく行えるか")]
    public void RemoveTrack()
    {
        var format = FormatBuilder.Create()
            .WithFrequency(44100)
            .WithBitDepth(8)
            .WithChannelCount(1)
            .ToSoundFormat();
        var sound = new TrackBaseSound(format, 100);
        var wave = new SquareWave(SquareWaveRatio.Point25);
        var track1 = sound.CreateTrack(0, wave);
        var track2 = sound.CreateTrack(1000, wave);

        var maybeOk = sound.RemoveTrack(track1);

        var tracks = sound.GetAllTracks();

        Assert.True(maybeOk);
        Assert.DoesNotContain(track1, tracks);
        Assert.Contains(track2, tracks);

        var maybeFail = sound.RemoveTrack(new Track(new TriangleWave(), format, 100, 10000));
        Assert.False(maybeFail);
    }

    [Fact(DisplayName = "指定したミリ秒から開始するトラックをすべて取得できるか")]
    public void GetTracks()
    {
        var format = FormatBuilder.Create()
            .WithFrequency(44100)
            .WithBitDepth(8)
            .WithChannelCount(1)
            .ToSoundFormat();
        var sound = new TrackBaseSound(format, 100);
        var wave = new SquareWave(SquareWaveRatio.Point25);
        var track1 = sound.CreateTrack(0, wave);

        var tracks = sound.GetTracks(track1.StartMilliSecond);
        Assert.Contains(track1, tracks);
    }

    [Fact(DisplayName = "存在しない開始位置でトラックを取得しようとした場合に空リストが返るか")]
    public void GetTracks_Empty()
    {
        var format = FormatBuilder.Create()
            .WithFrequency(44100)
            .WithBitDepth(8)
            .WithChannelCount(1)
            .ToSoundFormat();
        var sound = new TrackBaseSound(format, 100);

        var maybeEmpty = sound.GetTracks(0);

        Assert.Empty(maybeEmpty);
    }

    [Fact(DisplayName = "トラック取得に成功した場合、失敗した場合")]
    public void TryGetTracks()
    {
        var format = FormatBuilder.Create()
            .WithFrequency(44100)
            .WithBitDepth(8)
            .WithChannelCount(1)
            .ToSoundFormat();
        var sound = new TrackBaseSound(format, 100);
        var wave = new SquareWave(SquareWaveRatio.Point25);
        var track1 = sound.CreateTrack(0, wave);

        Assert.True(sound.TryGetTracks(track1.StartMilliSecond, out var tracks));
        Assert.Contains(track1, tracks);

        Assert.False(sound.TryGetTracks(1000, out _));
    }

    [Fact(DisplayName = "トラックを指定した時間で挿入できる。")]
    public void Insert()
    {
        var format = FormatBuilder.Create()
            .WithFrequency(44100)
            .WithBitDepth(8)
            .WithChannelCount(1)
            .ToSoundFormat();
        var sound = new TrackBaseSound(format, 100);
        var wave = new SquareWave(SquareWaveRatio.Point25);
        var track1 = sound.CreateTrack(0, wave);
        var track2 = sound.CreateTrack(1000, wave);

        Assert.Equal(track1, sound.GetTracks(track1.StartMilliSecond)[0]);
        Assert.Equal(track2, sound.GetTracks(track2.StartMilliSecond)[0]);
    }

    [Fact(DisplayName = "トラックの移動を行えるか")]
    public void MoveTrack()
    {
        var format = FormatBuilder.Create()
            .WithFrequency(44100)
            .WithBitDepth(8)
            .WithChannelCount(1)
            .ToSoundFormat();
        var sound = new TrackBaseSound(format, 100);
        var wave = new SquareWave(SquareWaveRatio.Point25);
        var oldStartMilliSecond = 0;
        var track1 = sound.CreateTrack(oldStartMilliSecond, wave);

        var newStartMilliSecond = 1000;
        sound.MoveTrack(track1, newStartMilliSecond);

        Assert.Equal(track1, sound.GetTracks(newStartMilliSecond)[0]);
        Assert.Empty(sound.GetTracks(oldStartMilliSecond));
    }

    [Fact(DisplayName = "トラックのコピーを行えるか")]
    public void CopyTrack()
    {
        var format = FormatBuilder.Create()
            .WithFrequency(44100)
            .WithBitDepth(8)
            .WithChannelCount(1)
            .ToSoundFormat();
        var sound = new TrackBaseSound(format, 100);
        var wave = new SquareWave(SquareWaveRatio.Point25);
        var oldStartMilliSecond = 0;
        var track1 = sound.CreateTrack(oldStartMilliSecond, wave);

        var newStartMilliSecond = 1000;
        sound.CopyTrack(track1, newStartMilliSecond);

        Assert.Equal(track1, sound.GetTracks(oldStartMilliSecond)[0]);

        var copied = sound.GetTracks(newStartMilliSecond);
        Assert.NotEmpty(copied);
    }

    [Fact(DisplayName = "空にできるか")]
    public void Clear()
    {
        var format = FormatBuilder.Create()
            .WithFrequency(44100)
            .WithBitDepth(8)
            .WithChannelCount(1)
            .ToSoundFormat();
        var sound = new TrackBaseSound(format, 100);
        var wave = new SquareWave(SquareWaveRatio.Point25);
        _ = sound.CreateTrack(0, wave);
        _ = sound.CreateTrack(1000, wave);

        sound.Clear();

        Assert.Empty(sound.GetAllTracks());
    }
}
