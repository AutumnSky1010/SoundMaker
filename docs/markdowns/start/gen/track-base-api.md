# トラックベースAPIでの音声生成 (ver 3.0.0以降)

チャンネルベースAPIと異なり、開始時間をミリ秒で指定でき、パンも数値の範囲で指定できるため、細かな調整が可能です。

### 概要
この方法では、複数のトラックを作成し、それらをミキシングしてステレオ音声を生成します。各トラックの開始時間をミリ秒単位で指定でき、パン（左右の音の配置）も調整可能です。

### コード例
```cs
private static StereoWave MakeStereoWave(SoundFormat format)
{
    // 一分間の四分音符の個数
    int tempo = 100;
    var trackBaseSound = new TrackBaseSound(format, tempo);

    var track1Components = new ISoundComponent[]
    {
        // ISoundComponentを実装したクラスのオブジェクトの配列を定義する。上から順に演奏される。
        // 現段階では普通の音符、休符、タイ、連符を使うことができる。
        new Note(Scale.C, 5, LengthType.Eighth, isDotted: true),
        new Tie(new Note(Scale.D, 5, LengthType.Eighth), LengthType.Eighth),
        new Tuplet(GetComponents(), LengthType.Quarter)
    };

    var track2Components = new ISoundComponent[]
    {
        new Note(Scale.C, 4, LengthType.Eighth, isDotted: true),
        new Note(Scale.D, 4, LengthType.Quarter),
        new Rest(LengthType.Quarter)
    };
    var track3Components = new ISoundComponent[]
    {
        new Note(Scale.C, 3, LengthType.Eighth, isDotted: true),
        new Note(Scale.D, 3, LengthType.Quarter),
        new Rest(LengthType.Quarter)
    };

    // トラックを作成する
    var track1 = trackBaseSound.CreateTrack(startMilliSecond: 0, new SquareWave(SquareWaveRatio.Point25));
    track1.Import(track1Components);
    // パンを-1から1の範囲で指定できます。1は左、-1は右です。
    // 上限、下限値をオーバーした場合は上限・下限値に丸められることに注意してください。
    track1.Pan = -1;

    var track2 = trackBaseSound.CreateTrack(0, new SquareWave(SquareWaveRatio.Point125));
    track2.Import(track2Components);
    track2.Pan = -1;

    var track3 = trackBaseSound.CreateTrack(0, new TriangleWave());
    track3.Import(track3Components);
    track3.Pan = 1;

    // ステレオ波を生成する
    return trackBaseSound.GenerateStereoWave();
}
```

### 詳細な説明

- **テンポ設定**: `tempo` は1分間に演奏される四分音符の数を示しています。ここでは100に設定されています。
- **トラックの作成**:
    - `track1`: 矩形波のトラック。パンは右（`Pan = -1`）に設定されています。
        - コンポーネント: ドット付き八分音符、タイ、連符。
    - `track2`: 矩形波の別トラック。パンは右（`Pan = -1`）に設定されています。
        - コンポーネント: ドット付き八分音符、四分音符、休符。
    - `track3`: 三角波のトラック。パンは左（`Pan = 1`）に設定されています。
        - コンポーネント: ドット付き八分音符、四分音符、休符。
- **開始時間の指定**: 各トラックの開始時間をミリ秒単位で指定できます。例えば、`track1` の開始時間は `startMilliSecond: 0` です。
- **パンの設定**: パンを -1 から 1 の範囲で指定でき、1 は左、-1 は右を示します。上限、下限値をオーバーした場合は自動的に丸められます。
- **音波の生成**: `trackBaseSound.GenerateStereoWave` メソッドを使って、すべてのトラックをミックスし、ステレオ音波を生成します。