# SMSCシリアライズ・デシリアライズ

SMSCデータをシリアライズおよびデシリアライズする方法を示します。

### シリアライズ

以下のコード例では、サウンドコンポーネントをSMSC形式にシリアライズする方法を示しています。

#### コード例
```cs
// using SoundMaker.ScoreData.SMSC;
// using SoundMaker.Sounds.Score;
var components = new List<ISoundComponent>()
{
    new Note(Scale.E, 5, LengthType.Eighth),
    new Note(Scale.F, 5, LengthType.Eighth),
    new Note(Scale.G, 5, LengthType.Eighth),
};
var smsc = SMSCFormat.Serialize(components);
Console.WriteLine(smsc);
```

#### 詳細な説明

1. **サウンドコンポーネントの作成**: `ISoundComponent` を実装したクラス（例：`Note`）のオブジェクトをリストに追加します。
    - `new Note(Scale.E, 5, LengthType.Eighth)`: 音階E、5オクターブ、長さは八分音符の音を作成。
    - 同様に、音階FおよびGの音符も追加します。
2. **シリアライズ**: `SMSCFormat.Serialize` メソッドを使用して、サウンドコンポーネントのリストをSMSC形式にシリアライズします。
3. **出力**: シリアライズされたSMSCデータをコンソールに出力します。

### デシリアライズ

以下のコード例では、SMSC形式のデータをサウンドコンポーネントにデシリアライズする方法を示しています。

#### コード例
```cs
// using SoundMaker.ScoreData.SMSC;
// using SoundMaker.Sounds.Score;
var smsc = "C4, 4";
var result = SMSCFormat.Read(smsc);
if (result.TryGetValue(out IReadOnlyList<ISoundComponent> components))
{
}
else
{
}
```

#### 詳細な説明

1. **SMSCデータの読み込み**: シリアライズされたSMSC形式の文字列を用意します。例：`"C4, 4"`。
2. **デシリアライズ**: `SMSCFormat.Read` メソッドを使用して、SMSC形式のデータをサウンドコンポーネントにデシリアライズします。
3. **結果の確認**:
    - `result.TryGetValue(out IReadOnlyList<ISoundComponent> components)`: デシリアライズに成功した場合、サウンドコンポーネントのリストを取得します。
    - デシリアライズに失敗した場合の処理も含めます。
