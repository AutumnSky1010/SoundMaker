![SoundMakerCover](https://user-images.githubusercontent.com/66455966/206901705-974f5a63-46db-435c-bdb1-1717e8bb7883.png)  

## 🗺️言語(Language)
1) [日本語](#概要)
2) [English](#overview)

## 🎵概要
本ライブラリを用いると、以下の事が可能です。
- チップチューンサウンド？を作成する
- waveファイルにサウンドを書き込む

## 📑ドキュメント
[Wiki](https://github.com/AutumnSky1010/SoundMaker/wiki)

## ⛰️要件
.NET 6

## ⏬インストール方法
### NuGet

[SoundMaker](https://www.nuget.org/packages/SoundMaker/)

## 🎶簡単な使い方
```CSharp
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

## 👀詳細
### 出力形式
**サンプリング周波数**
- 48000Hz
- 44100Hz

**量子化ビット数**
- 16bit
- 8bit

**チャンネル数**
- Stereo 2ch
- Monaural 1ch

## 🍄作った人のツイッター
[Twitter](https://twitter.com/DTB_AutumnSky)  

## ©️ライセンス
MIT License

## 🎵Overview
You can do The following content with this library.
- make the sound of chiptune
- export sound to a file of wave format.

## 📑Documentation
[Wiki](https://github.com/AutumnSky1010/SoundMaker/wiki)

## ⛰️Requirement
.NET 6

## ⏬Installation
### NuGet

[SoundMaker](https://www.nuget.org/packages/SoundMaker/)

## 🎶Usage
```CSharp
namespace YourNamespace;
public static class YourClass
{
    private static void Main()
    {
        // Create a sound format.
        var builder = FormatBuilder.Create()
            .WithFrequency(48000)
            .WithBitDepth(16)
            .WithChannelCount(2);

        var soundFormat = builder.ToSoundFormat();
        StereoWave wave = MakeStereoWave(soundFormat);

        // Write to a file.
        var sound = new SoundWaveChunk(wave.GetBytes(soundFormat.BitRate));
        var waveFileFormat = builder.ToFormatChunk();
        var writer = new WaveWriter(waveFileFormat, sound);
        string filePath = "sample.wav";
        writer.Write(filePath);
    }

    private static StereoWave MakeStereoWave(SoundFormat format)
    {
        // The number of quarter notes per minute
        int tempo = 100;
        // First, you need to create sound channels.
        // Currently, it supports square wave, triangle wave, pseudo-triangle wave, and low-bit noise.
        var rightChannel = new SquareSoundChannel(tempo, format, SquareWaveRatio.Point25, PanType.Right)
        {
            // Add objects of classes that implement ISoundComponent to the channel.
            // Currently, you can use normal notes, rests, ties, and tuplets.
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
        // Mixing is done by the 'StereoMixer' class. 
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

## 👀Features
### Output format
**Sampling frequency**
- 48000Hz
- 44100Hz

**Quantization bit rate**
- 16bit
- 8bit

**Number of Channels**
- Stereo 2ch
- Monaural 1ch

## 🍄Author
[Twitter](https://twitter.com/DTB_AutumnSky)

## ©️License
SoundMaker is licensed under the MIT License.

