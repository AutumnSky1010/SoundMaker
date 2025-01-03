using SoundMaker.Sounds.WaveTypes;

namespace SoundMaker.Sounds.Score;

/// <summary>
/// Tuplet. <br/>連符を表すクラス
/// </summary>
public class Tuplet : ISoundComponent
{
    /// <summary>
    /// Constructor. <br/>コンストラクタ
    /// </summary>
    /// <param name="tupletComponents">Components to be tuplet. <br/>連符にする基本の音のリスト</param>
    /// <param name="length">Length (ex. "quarter" note). <br/>長さ（音楽的な、「四分」音符、「全」休符のような長さを表す。）</param>
    /// <param name="isDotted">Tuplet is dotted. <br/>付点かを表す論理型</param>
    public Tuplet(IReadOnlyList<ISoundComponent> tupletComponents, LengthType length, bool isDotted = false)
    {
        TupletComponents = new List<ISoundComponent>(tupletComponents);
        Length = length;
        IsDotted = isDotted;
    }

    /// <summary>
    /// Components to be tuplet. <br/>連符にする基本の音のリスト
    /// </summary>
    internal IReadOnlyList<ISoundComponent> TupletComponents { get; }

    /// <summary>
    /// Get the component at index. <br/>index番目の連符の音を取得する。
    /// </summary>
    /// <param name="index">Index. <br/>何番目かを表す整数</param>
    /// <returns>Sound component. <br/>サウンドコンポーネント</returns>
    /// <exception cref="IndexOutOfRangeException">Index is less than 0 or index is equal to or greater than Count.</exception>
    public ISoundComponent this[int index] => index < 0 || index >= TupletComponents.Count
                ? throw new IndexOutOfRangeException("Index is less than 0 or index is equal to or greater than Count.")
                : TupletComponents[index];

    /// <summary>
    /// Length (ex. "quarter" note). <br/>長さ（音楽的な、「四分」音符、「全」休符のような長さを表す。）
    /// </summary>
    public LengthType Length { get; }

    /// <summary>
    /// Note/rest is dotted. <br/>付点かを表す論理型
    /// </summary>
    public bool IsDotted { get; }

    /// <summary>
    /// Count of components in this. <br/>コンポーネントの個数
    /// </summary>
    public int Count => TupletComponents.Count;

    public int GetWaveArrayLength(SoundFormat format, int tempo)
    {
        return SoundWaveLengthCalculator.Calculate(format, tempo, Length, IsDotted);
    }

    public short[] GenerateWave(SoundFormat format, int tempo, int length, WaveTypeBase waveType)
    {
        var result = new List<short>(length);
        // Calculate the length of each component
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

    public short[] GenerateWave(SoundFormat format, int tempo, WaveTypeBase waveType)
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

    /// <summary>
    /// Creates a clone of the tuplet. <br/>連符のクローンを作成するメソッド。
    /// </summary>
    /// <returns>A new instance of the tuplet with the same properties. <br/>同じプロパティを持つ連符の新しいインスタンス</returns>
    public Tuplet Clone()
    {
        var cloned = new Tuplet(TupletComponents.Select(component => component.Clone()).ToArray(), Length, IsDotted);
        return cloned;
    }

    ISoundComponent ISoundComponent.Clone()
    {
        return Clone();
    }
}
