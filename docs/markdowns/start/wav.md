# WAV出力

SoundMakerではWAV形式(リニアPCM)で音声を出力できます。

### ファイルに出力する

以下のコード例では、サウンドの形式を作成し、波形をファイルに出力する方法を示しています。

#### コード例
```cs
// サウンドの形式を作成する。
var builder = FormatBuilder.Create()
    .WithFrequency(48000)
    .WithBitDepth(16)
    .WithChannelCount(2);
var waveFileFormat = builder.ToFormatChunk();

// 波形のバイト列
var wave = new byte[10];

var sound = new SoundWaveChunk(wave);
var writer = new WaveWriter(waveFileFormat, sound);
var filePath = "sample.wav";
writer.Write(filePath);
```

#### 詳細な説明

1. **サウンド形式の作成**: `FormatBuilder` を使用してサウンドの形式を設定します。
    - `.WithFrequency(48000)`: サンプリング周波数を48000Hzに設定。
    - `.WithBitDepth(16)`: 量子化ビット数を16bitに設定。
    - `.WithChannelCount(2)`: チャンネル数をステレオ（2ch）に設定。
2. **フォーマットの取得**: `ToFormatChunk` メソッドを使って、WAV出力用のフォーマットを取得します。
3. **波形の作成**: `SoundWaveChunk` クラスのインスタンスを作成し、波形データ（バイト列）を渡します。
4. **ファイルへの書き込み**: `WaveWriter` クラスを使用して、フォーマットと波形データを指定し、ファイルに出力します。

### ストリームに出力する

以下のコード例では、サウンドの形式を作成し、波形をメモリストリームに出力する方法を示しています。

#### コード例
```cs
// サウンドの形式を作成する。
var builder = FormatBuilder.Create()
    .WithFrequency(48000)
    .WithBitDepth(16)
    .WithChannelCount(2);
var waveFileFormat = builder.ToFormatChunk();

// 波形のバイト列
var wave = new byte[10];

var sound = new SoundWaveChunk(wave);
var writer = new WaveWriter(waveFileFormat, sound);
var memoryStream = new MemoryStream();
writer.Write(memoryStream);
```

#### 詳細な説明

1. **サウンド形式の作成**: `FormatBuilder` を使用してサウンドの形式を設定します。
    - `.WithFrequency(48000)`: サンプリング周波数を48000Hzに設定。
    - `.WithBitDepth(16)`: 量子化ビット数を16bitに設定。
    - `.WithChannelCount(2)`: チャンネル数をステレオ（2ch）に設定。
2. **フォーマットの取得**: `ToFormatChunk` メソッドを使って、WAV出力用のフォーマットを取得します。
3. **波形の作成**: `SoundWaveChunk` クラスのインスタンスを作成し、波形データ（バイト列）を渡します。
4. **ストリームへの書き込み**: `WaveWriter` クラスを使用して、フォーマットと波形データを指定し、メモリストリームに出力します。