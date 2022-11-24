using SoundMaker.Sounds.WaveTypes;

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
    /// <param name="isDotted">note/rest is dotted. 付点かを表す論理型</param>
    public Tuplet(IReadOnlyList<ISoundComponent> tupletComponents, LengthType length, bool isDotted = false)
    {
        this.TupletComponents = new List<ISoundComponent>(tupletComponents);
        this.Length = length;
        this.IsDotted = isDotted;
    }

    private IReadOnlyList<ISoundComponent> TupletComponents { get; }

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
    public int Count => this.TupletComponents.Count;

    public int GetWaveArrayLength(SoundFormat format, int tempo)
    {
        return SoundWaveLengthCaluclator.Caluclate(format, tempo, this.Length, this.IsDotted);
    }

    public ushort[] GenerateWave(SoundFormat format, int tempo, int length, WaveTypeBase waveType)
    {
        var result = new List<ushort>(length);
        // 一個あたりの配列の長さを算出
        int count = this.GetLengthPerOneComponent();
        int i;
        var componentLengthBase = length / count;
        for (i = 0; i < this.Count - 1; i++)
        {
            int componentLength;
            if (this.TupletComponents[i] is Tie tie)
            {
                componentLength = componentLengthBase * tie.Count;
            }
            else { componentLength = componentLengthBase; }
            result.AddRange(this.TupletComponents[i].GenerateWave(format, tempo, componentLength, waveType));
        }

        int lastComponentLength;
        if (this.TupletComponents[i] is Tie lastTie)
        {
            lastComponentLength = componentLengthBase * lastTie.Count + length % count;
        }
        else { lastComponentLength = componentLengthBase + length % count; }
        result.AddRange(this.TupletComponents[i].GenerateWave(format, tempo, lastComponentLength, waveType));
        return result.ToArray();
    }

    public ushort[] GenerateWave(SoundFormat format, int tempo, WaveTypeBase waveType)
    {
        var length = this.GetWaveArrayLength(format, tempo);
        return this.GenerateWave(format, tempo, length, waveType);
    }

    private int GetLengthPerOneComponent()
    {
        int count = 0;
        for (int i = 0; i < this.Count; i++)
        {
            var component = this.TupletComponents[i];
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
