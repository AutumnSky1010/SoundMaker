# SMSCフォーマットとは？
## 概要
簡単にSoundMaker用の楽譜を記述するためのフォーマットです。詳細な情報は設定できませんが、簡単に記述できます。  
チャンネルやトラックの完全なデータを永続化する目的では利用できないことに注意してください。

## 例
```
// コメントアウト
// 付点四分音符(音階はC#4)
C#4, 4.
// 休符
rest, 4.
// タイ と 三連符(4分音符を三等分している)
tie(C4, 4, 4, 4); tup(4, C4, C4, C4)
```

## 仕様
[BNF(バッカス・ナウア記法)](https://github.com/AutumnSky1010/SoundMaker/blob/master/src/SoundMaker/ScoreData/SMSC/SMSC.bnf)

## 今後の展望
音量やエフェクトなどを指定できるように仕様を拡張する可能性があります。  