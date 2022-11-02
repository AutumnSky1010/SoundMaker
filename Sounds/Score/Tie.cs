namespace SoundMaker.Sounds.Score;
/// <summary>
/// タイ（同じ高さの音符同士を繋げて、あたかも一つの音符かのように扱う）を表すクラス
/// </summary>
public class Tie : ISoundComponent
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="baseNote">基本となる音符。二つ目の音符の音の高さはこの音符と同じになる。</param>
    /// <param name="additionalLength">二つ目の音符の長さ（音楽的な、「四分」音符、「全」休符のような長さを表す。）</param>
    /// <param name="additionalIsDotted">二つ目の音符が付点かを表す論理型</param>
    public Tie(Note baseNote, LengthType additionalLength, bool additionalIsDotted = false)
    {
        this.BaseNote = baseNote;
        this.AdditionalIsDotted = additionalIsDotted;
        this.AdditionalLength = additionalLength;
    }

    /// <summary>
    /// 基本の音符
    /// </summary>
    public Note BaseNote { get; }

    /// <summary>
    /// 二つ目の音符の長さ（音楽的な、「四分」音符、「全」休符のような長さを表す。）
    /// </summary>
    public LengthType AdditionalLength { get; }

    /// <summary>
    /// 二つ目の音符が付点かを表す論理型
    /// </summary>
    public bool AdditionalIsDotted { get; }

    public ushort[] GetSquareWave(SoundFormat format, SquareWaveRatio squareWaveRatio, int tempo)
    {
        int length = this.GetWaveArrayLength(format, tempo);
        return this.GetSquareWave(format, squareWaveRatio, tempo, length);
    }

    public ushort[] GetSquareWave(SoundFormat format, SquareWaveRatio squareWaveRatio, int tempo, int length)
    {
        return this.BaseNote.GetSquareWave(format, squareWaveRatio, tempo, length);
    }

    public ushort[] GetTriangleWave(SoundFormat format, int tempo)
    {
        int length = this.GetWaveArrayLength(format, tempo);
        return this.GetTriangleWave(format, tempo, length);
    }

    public ushort[] GetTriangleWave(SoundFormat format, int tempo, int length)
    {
        return this.BaseNote.GetTriangleWave(format, tempo, length);
    }

    public int GetWaveArrayLength(SoundFormat format, int tempo)
    {
        int length = this.BaseNote.GetWaveArrayLength(format, tempo);
        length += SoundWaveLengthCaluclator.Caluclate(format, tempo, this.AdditionalLength, this.AdditionalIsDotted);
        return length;
    }
}
