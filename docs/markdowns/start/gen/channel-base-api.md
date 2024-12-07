# チャンネルベースAPIでの音声生成

サウンドチャンネルに音符や休符を挿入することで音声を生成する方法を説明します。

### 概要
この方法では、複数の音のチャンネルを作成し、それらをミキシングしてステレオ音声を生成します。

### コード例
```cs
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
```

### 詳細な説明

- **テンポ設定**: `tempo` は1分間に演奏される四分音符の数を示しています。ここでは100に設定されています。
- **チャンネルの作成**:
    - `rightChannel`: 矩形波のサウンドチャンネル。右パンに設定されています。ドット付き八分音符、タイ、連符が含まれています。
    - `rightChannel2`: 矩形波の別チャンネル。異なる周波数（`SquareWaveRatio.Point125`）で設定されています。
    - `leftChannel`: 三角波のサウンドチャンネル。左パンに設定されています。
- **ISoundComponent** を実装したクラスのオブジェクト（`Note`、`Tie`、`Tuplet`、`Rest`など）をチャンネルに追加しています。
- **ミキシング**: 最後に、これらのチャンネルを `StereoMixer` クラスでミックスし、ステレオ音声を生成します。
