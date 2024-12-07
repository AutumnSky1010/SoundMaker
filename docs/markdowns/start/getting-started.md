# 最初のコード
```CSharp
using SoundMaker;
using SoundMaker.Sounds;
using SoundMaker.Sounds.Score;
using SoundMaker.Sounds.SoundChannels;
using SoundMaker.WaveFile;

namespace YourNamespace;
public static class YourClass
{
    private static void Main()
    {
        // サウンドの形式を作成する。
        var builder = FormatBuilder.Create()
            .WithFrequency(48000)
            .WithBitDepth(16)
            .WithChannelCount(2);

        var soundFormat = builder.ToSoundFormat();
        StereoWave wave = MakeStereoWave(soundFormat);

        // ファイルに書き込む。
        var sound = new SoundWaveChunk(wave.GetBytes(soundFormat.BitRate));
        var waveFileFormat = builder.ToFormatChunk();
        var writer = new WaveWriter(waveFileFormat, sound);
        string filePath = "sample.wav";
        writer.Write(filePath);
    }

    private static StereoWave MakeStereoWave(SoundFormat format)
    {
        // 一分間の四分音符の個数
        int tempo = 100;
        // まず、音のチャンネルを作成する必要がある。
        // 現段階では矩形波、三角波、疑似三角波、ロービットノイズに対応している。
        var rightChannel = new SquareSoundChannel(tempo, format, SquareWaveRatio.Point25, PanType.Right)
        {
            // ISoundComponentを実装したクラスのオブジェクトをチャンネルに追加していく。
            // 現段階では普通の音符、休符、タイ、連符を使うことができる。
            new Note(Scale.C, 5, LengthType.Eighth, isDotted: true),
            new Tie(new Note(Scale.D, 5, LengthType.Eighth), LengthType.Eighth),
            new Tuplet(GetComponents(), LengthType.Quarter)
        };
        var rightChannel2 = new SquareSoundChannel(tempo, format, SquareWaveRatio.Point125, PanType.Right)
        {
            new Note(Scale.C, 4, LengthType.Eighth, isDotted: true),
            new Note(Scale.D, 4, LengthType.Quarter),
            new Rest(LengthType.Quarter)
        };
        var leftChannel = new TriangleSoundChannel(tempo, format, PanType.Left)
        {
            new Note(Scale.C, 3, LengthType.Eighth, isDotted: true),
            new Note(Scale.D, 3, LengthType.Quarter),
            new Rest(LengthType.Quarter)
        };

        var channels = new List<ISoundChannel>() { rightChannel, rightChannel2, leftChannel };
        // ミックスは'StereoMixer'クラスで行う。 
        return new StereoMixer(channels).Mix();
    }

    private static IReadOnlyList<BasicSoundComponentBase> GetComponents()
    {
        return new List<BasicSoundComponentBase>()
        {
            new Note(Scale.E, 5, LengthType.Eighth),
            new Note(Scale.F, 5, LengthType.Eighth),
            new Note(Scale.G, 5, LengthType.Eighth),
        };
    }
}
```

今後の説明でもこのサンプルコードを利用します。