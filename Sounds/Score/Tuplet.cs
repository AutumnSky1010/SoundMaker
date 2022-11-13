namespace SoundMaker.Sounds.Score;
/// <summary>
/// 連符を表すクラス
/// </summary>
public class Tuplet : ISoundComponent
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="tupletComponents">連符にする基本の音のリスト</param>
    /// <param name="length">長さ（音楽的な、「四分」音符、「全」休符のような長さを表す。）</param>
    /// <param name="isDotted">付点かを表す論理型</param>
    public Tuplet(IReadOnlyList<BasicSoundComponentBase> tupletComponents, LengthType length, bool isDotted = false)
    {
        this.TupletComponents = new List<BasicSoundComponentBase>(tupletComponents);
        this.Length = length;
        this.IsDotted = isDotted;
    }

    private IReadOnlyList<BasicSoundComponentBase> TupletComponents { get; }

    /// <summary>
    /// 長さ（音楽的な、「四分」音符、「全」休符のような長さを表す。）
    /// </summary>
    public LengthType Length { get; }

    /// <summary>
    /// 付点かを表す論理型
    /// </summary>
    public bool IsDotted { get; }

    /// <summary>
    /// コンポーネントの個数
    /// </summary>
    public int Count => this.TupletComponents.Count;

    public ushort[] GetSquareWave(SoundFormat format, SquareWaveRatio squareWaveRatio, int tempo)
    {
        var length = this.GetWaveArrayLength(format, tempo);
        return this.GetSquareWave(format, squareWaveRatio, tempo, length);
    }

    public ushort[] GetSquareWave(SoundFormat format, SquareWaveRatio squareWaveRatio, int tempo, int length)
    {
        var result = new List<ushort>(length);
        // コンポーネントの数で割って、一個あたりの配列の長さを算出
        var componentLength = length / this.Count;
        int i;
        for (i = 0; i < this.Count - 1; i++)
        {
            result.AddRange(this.TupletComponents[i].GetSquareWave(format, squareWaveRatio, tempo, componentLength));
        }
        var lastComponentLength = componentLength + length % this.Count;
        result.AddRange(this.TupletComponents[i].GetSquareWave(format, squareWaveRatio, tempo, lastComponentLength));
        return result.ToArray();
    }

    public ushort[] GetTriangleWave(SoundFormat format, int tempo)
    {
        var length = this.GetWaveArrayLength(format, tempo);
        return this.GetTriangleWave(format, tempo, length);
    }

    public ushort[] GetTriangleWave(SoundFormat format, int tempo, int length)
    {
        var result = new List<ushort>(length);
        var componentLength = length / this.Count;
        int i;
        for (i = 0; i < length - 1; i++)
        {
            result.AddRange(this.TupletComponents[i].GetTriangleWave(format, tempo, componentLength));
        }
        var lastComponentLength = componentLength + length % this.Count;
        result.AddRange(this.TupletComponents[i].GetTriangleWave(format, tempo, lastComponentLength));
        return result.ToArray();
    }

    public int GetWaveArrayLength(SoundFormat format, int tempo)
    {
        return SoundWaveLengthCaluclator.Caluclate(format, tempo, this.Length, this.IsDotted);
    }

    public ushort[] GetNoiseWave(SoundFormat format, int tempo)
    {
        var length = this.GetWaveArrayLength(format, tempo);
        return this.GetNoiseWave(format, tempo, length);
    }

    public ushort[] GetNoiseWave(SoundFormat format, int tempo, int length)
    {
        var result = new List<ushort>(length);
        // コンポーネントの数で割って、一個あたりの配列の長さを算出
        var componentLength = length / this.Count;
        int i;
        for (i = 0; i < this.Count - 1; i++)
        {
            result.AddRange(this.TupletComponents[i].GetNoiseWave(format, tempo, componentLength));
        }
        var lastComponentLength = componentLength + length % this.Count;
        result.AddRange(this.TupletComponents[i].GetNoiseWave(format, tempo, lastComponentLength));
        return result.ToArray();
    }
}
