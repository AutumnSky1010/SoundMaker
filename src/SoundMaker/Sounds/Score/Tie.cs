using SoundMaker.Sounds.WaveTypes;

namespace SoundMaker.Sounds.Score;
/// <summary>
/// tie is joined two notes of same scale. タイ（同じ高さの音符同士を繋げて、あたかも一つの音符かのように扱う）を表すクラス
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
        BaseNote = baseNote;
        AdditionalNotes = new List<Note>()
        {
            new Note(baseNote.Scale, baseNote.ScaleNumber, additionalLength, additionalIsDotted)
        };
    }

    /// <summary>
    /// constructor コンストラクタ
    /// </summary>
    /// <param name="baseNote">the note of base. 基本となる音符。二つ目の音符の音の高さはこの音符と同じになる。</param>
    /// <param name="additionalNotes">notes of tie. 追加する音符</param>
    public Tie(Note baseNote, IReadOnlyCollection<Note> additionalNotes)
    {
        BaseNote = baseNote;
        AdditionalNotes = new List<Note>(additionalNotes);
    }

    /// <summary>
    /// count of notes.
    /// </summary>
    public int Count => AdditionalNotes.Count + 1;

    /// <summary>
    /// the base note. 基本の音符
    /// </summary>
    public Note BaseNote { get; }

    /// <summary>
    /// the additional notes. 追加の音符のリスト
    /// </summary>
    public IReadOnlyCollection<Note> AdditionalNotes { get; }

    public ushort[] GenerateWave(SoundFormat format, int tempo, int length, WaveTypeBase waveType)
    {
        return BaseNote.GenerateWave(format, tempo, length, waveType);
    }

    public ushort[] GenerateWave(SoundFormat format, int tempo, WaveTypeBase waveType)
    {
        var length = GetWaveArrayLength(format, tempo);
        return GenerateWave(format, tempo, length, waveType);
    }

    public int GetWaveArrayLength(SoundFormat format, int tempo)
    {
        var length = BaseNote.GetWaveArrayLength(format, tempo);
        foreach (var note in AdditionalNotes)
        {
            length += SoundWaveLengthCaluclator.Caluclate(format, tempo, note.Length, note.IsDotted);
        }
        return length;
    }
}
