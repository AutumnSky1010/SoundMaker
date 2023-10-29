﻿using SoundMaker.Sounds.WaveTypes;

namespace SoundMaker.Sounds.Score;
/// <summary>
/// tuplet. 連符を表すクラス
/// </summary>
public class Tuplet : ISoundComponent
{
    /// <summary>
    /// constructor コンストラクタ
    /// </summary>
    /// <param name="tupletComponents">components to be tuplet. 連符にする基本の音のリスト</param>
    /// <param name="length">length (ex. "quarter" note) 長さ（音楽的な、「四分」音符、「全」休符のような長さを表す。）</param>
    /// <param name="isDotted">tuplet is dotted. 付点かを表す論理型</param>
    public Tuplet(IReadOnlyList<ISoundComponent> tupletComponents, LengthType length, bool isDotted = false)
    {
        TupletComponents = new List<ISoundComponent>(tupletComponents);
        Length = length;
        IsDotted = isDotted;
    }

    /// <summary>
    /// components to be tuplet. 連符にする基本の音のリスト
    /// </summary>
    private IReadOnlyList<ISoundComponent> TupletComponents { get; }

    /// <summary>
    /// get the component at index. index番目の連符の音を取得する。
    /// </summary>
    /// <param name="index">index. 何番目かを表す整数</param>
    /// <returns>sound component.サウンドコンポーネント</returns>
    /// <exception cref="IndexOutOfRangeException">index is less than 0 or index is equal to or greater than Count.</exception>
    public ISoundComponent this[int index] => index < 0 || index >= TupletComponents.Count
                ? throw new IndexOutOfRangeException("index is less than 0 or index is equal to or greater than Count.")
                : TupletComponents[index];

    /// <summary>
    /// length (ex. "quarter" note) 長さ（音楽的な、「四分」音符、「全」休符のような長さを表す。
    /// </summary>
    public LengthType Length { get; }

    /// <summary>
    /// note/rest is dotted. 付点かを表す論理型
    /// </summary>
    public bool IsDotted { get; }

    /// <summary>
    /// count of component in this. コンポーネントの個数
    /// </summary>
    public int Count => TupletComponents.Count;

    public int GetWaveArrayLength(SoundFormat format, int tempo)
    {
        return SoundWaveLengthCaluclator.Caluclate(format, tempo, Length, IsDotted);
    }

    public ushort[] GenerateWave(SoundFormat format, int tempo, int length, WaveTypeBase waveType)
    {
        var result = new List<ushort>(length);
        // 一個あたりの配列の長さを算出
        var count = GetLengthPerOneComponent();
        int i;
        var componentLengthBase = length / count;
        for (i = 0; i < Count - 1; i++)
        {
            var componentLength = TupletComponents[i] is Tie tie ? componentLengthBase * tie.Count : componentLengthBase;
            result.AddRange(TupletComponents[i].GenerateWave(format, tempo, componentLength, waveType));
        }

        var lastComponentLength = TupletComponents[i] is Tie lastTie
            ? (componentLengthBase * lastTie.Count) + (length % count)
            : componentLengthBase + (length % count);
        result.AddRange(TupletComponents[i].GenerateWave(format, tempo, lastComponentLength, waveType));
        return result.ToArray();
    }

    public ushort[] GenerateWave(SoundFormat format, int tempo, WaveTypeBase waveType)
    {
        var length = GetWaveArrayLength(format, tempo);
        return GenerateWave(format, tempo, length, waveType);
    }

    private int GetLengthPerOneComponent()
    {
        var count = 0;
        for (var i = 0; i < Count; i++)
        {
            var component = TupletComponents[i];
            if (component is Tie tie)
            {
                count += tie.Count;
            }
            else
            {
                count++;
            }
        }
        return count;
    }
}
