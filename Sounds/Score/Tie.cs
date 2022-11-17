using SoundMaker.Sounds.WaveTypes;

namespace SoundMaker.Sounds.Score;
/// <summary>
/// tie. タイ（同じ高さの音符同士を繋げて、あたかも一つの音符かのように扱う）を表すクラス
/// </summary>
public class Tie : ISoundComponent
{
    /// <summary>
    /// constructor コンストラクタ
    /// </summary>
    /// <param name="baseNote">the note of base. 基本となる音符。二つ目の音符の音の高さはこの音符と同じになる。</param>
    /// <param name="additionalLength">length of the second note.(ex. "quarter" note) 二つ目の音符の長さ（音楽的な、「四分」音符、「全」休符のような長さを表す。）</param>
    /// <param name="additionalIsDotted">the second note/rest is dotted. 二つ目の音符が付点かを表す論理型</param>
    public Tie(Note baseNote, LengthType additionalLength, bool additionalIsDotted = false)
    {
        this.BaseNote = baseNote;
        this.AdditionalIsDotted = additionalIsDotted;
        this.AdditionalLength = additionalLength;
    }

    /// <summary>
    /// the base note. 基本の音符
    /// </summary>
    public Note BaseNote { get; }

    /// <summary>
    /// length of the second note.(ex. "quarter" note) 二つ目の音符の長さ（音楽的な、「四分」音符、「全」休符のような長さを表す。）
    /// </summary>
    public LengthType AdditionalLength { get; }

    /// <summary>
    /// the second note/rest is dotted.二つ目の音符が付点かを表す論理型
    /// </summary>
    public bool AdditionalIsDotted { get; }

    public ushort[] GenerateWave(SoundFormat format, int tempo, int length, WaveTypeBase waveType)
    {
        return this.BaseNote.GenerateWave(format, tempo, length, waveType);
    }

    public ushort[] GenerateWave(SoundFormat format, int tempo, WaveTypeBase waveType)
    {
        int length = this.GetWaveArrayLength(format, tempo);
        return this.GenerateWave(format, tempo, length, waveType);
    }

    public int GetWaveArrayLength(SoundFormat format, int tempo)
    {
        int length = this.BaseNote.GetWaveArrayLength(format, tempo);
        length += SoundWaveLengthCaluclator.Caluclate(format, tempo, this.AdditionalLength, this.AdditionalIsDotted);
        return length;
    }
}
