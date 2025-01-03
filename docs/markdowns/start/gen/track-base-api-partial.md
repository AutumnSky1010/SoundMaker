# バッファリング可能な音声生成(トラックベースAPI)

この機能は音声波形を部分的に生成し、リアルタイムでの音声再生をサポートします。特に大規模な音声データを処理する場合に、効率的な再生を実現します。

### 概要
この機能は、指定されたバッファサイズごとに音声波形を部分的に生成します。これにより、音声データのリアルタイム再生が可能になります。

### コード例
以下は、バッファサイズ1024でステレオ波形を生成するコード例です：
```cs
// GenerateBufferedMonauralWave
var stereoWaves = trackBaseSound.GenerateBufferedStereoWave(startIndex: 0, bufferSize: 1024);

foreach (var wave in stereoWaves)
{
    // 波形を利用した処理
}
```

開始インデクス以降の波形を繰り返し生成します。

### 詳細な説明
- **バッファサイズ**: `bufferSize` は一度に生成する波形データのサイズを示しています。この例では1024に設定されています。
- **開始インデックス**: `startIndex` は波形生成の開始位置を示しています。ここでは0に設定されています。