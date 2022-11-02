namespace SoundMaker.Sounds.Score;
/// <summary>
/// 音符・休符など音の基本部品を表す抽象基底クラス
/// </summary>
public abstract class BasicSoundComponentBase : ISoundComponent
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="length">長さ（音楽的な、「四分」音符、「全」休符のような長さを表す。）</param>
    /// <param name="isDotted">付点かを表す論理型</param>
    public BasicSoundComponentBase(LengthType length, bool isDotted)
    {
        this.Length = length;
        this.IsDotted = isDotted;
    }

    /// <summary>
    /// 長さ（音楽的な、「四分」音符、「全」休符のような長さを表す。）
    /// </summary>
    public LengthType Length { get; }

    /// <summary>
    /// 付点かを表す論理型
    /// </summary>
    public bool IsDotted { get; }

    public abstract ushort[] GetSquareWave(SoundFormat format, SquareWaveRatio squareWaveRatio, int tempo);

    public abstract ushort[] GetSquareWave(SoundFormat format, SquareWaveRatio squareWaveRatio, int tempo, int length);

    public abstract ushort[] GetTriangleWave(SoundFormat format, int tempo);

    public abstract ushort[] GetTriangleWave(SoundFormat format, int tempo, int length);

    public int GetWaveArrayLength(SoundFormat format, int tempo)
    {
        return SoundWaveLengthCaluclator.Caluclate(format, tempo, this.Length, this.IsDotted);
    }
}
