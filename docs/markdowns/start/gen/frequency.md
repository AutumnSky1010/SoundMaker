# 周波数を指定し、音声波形を生成する

周波数を直接指定して音波を生成する方法です。以下のプログラムでは、800Hzの三角波を5秒間生成します。

### 概要
指定された周波数の音波を生成するためのコードです。指定秒数分の配列の長さは `サンプリング周波数 * 秒数` で求められます。

### コード例
```cs
private static StereoWave MakeStereoWave(SoundFormat format)
{
    var triangleWave = new TriangleWave();
    int hertz = 800;
    int length = (int)format.SamplingFrequency * 5;
    int volume = 50;

    var waveShorts = triangleWave.GenerateWave(format, length, volume, hertz);
    return new StereoWave(waveShorts, waveShorts);
}
```

### 詳細な説明

- **三角波の作成**: `TriangleWave` クラスのインスタンスを作成します。
- **周波数の設定**: `hertz` は生成する音波の周波数を指定します。ここでは800Hzに設定されています。
- **配列の長さの計算**: `length` は `サンプリング周波数 * 秒数` で計算されます。ここでは5秒間の音波を生成するため、 `format.SamplingFrequency * 5` となります。
- **音量の設定**: `volume` で音量を指定します。ここでは50に設定されています(0 <= volume <= 100)。
- **音波の生成**: `triangleWave.GenerateWave` メソッドを使って、指定された周波数と音量で音波を生成します。
- **ステレオ音波の作成**: 最後に、生成された音波データを使って `StereoWave` のインスタンスを作成します。