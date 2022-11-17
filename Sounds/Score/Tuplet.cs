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
    public Tuplet(IReadOnlyList<BasicSoundComponentBase> tupletComponents, LengthType length, bool isDotted = false)
    {
        this.TupletComponents = new List<BasicSoundComponentBase>(tupletComponents);
        this.Length = length;
        this.IsDotted = isDotted;
    }

    private IReadOnlyList<BasicSoundComponentBase> TupletComponents { get; }

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
        // コンポーネントの数で割って、一個あたりの配列の長さを算出
        var componentLength = length / this.Count;
        int i;
        for (i = 0; i < this.Count - 1; i++)
        {
            result.AddRange(this.TupletComponents[i].GenerateWave(format, tempo, componentLength, waveType));
        }
        var lastComponentLength = componentLength + length % this.Count;
        result.AddRange(this.TupletComponents[i].GenerateWave(format, tempo, lastComponentLength, waveType));
        return result.ToArray();
    }

    public ushort[] GenerateWave(SoundFormat format, int tempo, WaveTypeBase waveType)
    {
        var length = this.GetWaveArrayLength(format, tempo);
        return this.GenerateWave(format, tempo, length, waveType);
    }
}
