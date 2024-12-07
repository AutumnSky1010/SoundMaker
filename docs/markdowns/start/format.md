# 利用できるフォーマットについて

SoundMakerでは、以下の形式のWAV出力に対応しています。

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

## FormatBuilderの使い方

SoundMakerでは、簡単にフォーマットデータを扱うためのビルダクラスを提供しています。

### コード例
```cs
var builder = FormatBuilder.Create()
            .WithFrequency(48000)
            .WithBitDepth(16)
            .WithChannelCount(2);

// 音声波形生成用フォーマット
var soundFormat = builder.ToSoundFormat();
// WAV出力用フォーマット
var waveFileFormat = builder.ToFormatChunk();
```

### 詳細な説明

- **サンプリング周波数の設定**: `.WithFrequency(48000)` で、サンプリング周波数を48000Hzに設定します。
- **量子化ビット数の設定**: `.WithBitDepth(16)` で、量子化ビット数を16bitに設定します。
- **チャンネル数の設定**: `.WithChannelCount(2)` で、ステレオ（2ch）に設定します。
- **フォーマットの取得**:
    - `soundFormat`: `.ToSoundFormat()` メソッドを使用して、音声波形生成用のフォーマットを取得します。
    - `waveFileFormat`: `.ToFormatChunk()` メソッドを使用して、WAV出力用のフォーマットを取得します。