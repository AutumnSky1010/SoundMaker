using SoundMaker.Sounds.WaveTypes;

namespace SoundMaker.Sounds.Score;

/// <summary>
/// Tie is joined two notes of same scale. <br/>タイ（同じ高さの音符同士を繋げて、あたかも一つの音符かのように扱う）を表すクラス
/// </summary>
public class Tie : ISoundComponent
{
    /// <summary>
    /// Constructor. <br/>コンストラクタ
    /// </summary>
    /// <param name="baseNote">The note of base. <br/>基本となる音符。二つ目の音符の音の高さはこの音符と同じになる。</param>
    /// <param name="additionalLength">Length of the second note. (ex. "quarter" note). <br/>二つ目の音符の長さ（音楽的な、「四分」音符、「全」休符のような長さを表す。）</param>
    /// <param name="additionalIsDotted">The second note/rest is dotted. <br/>二つ目の音符が付点かを表す論理型</param>
    public Tie(Note baseNote, LengthType additionalLength, bool additionalIsDotted = false)
    {
        BaseNote = baseNote;
        AdditionalNotes = new List<Note>()
        {
            new(baseNote.Scale, baseNote.ScaleNumber, additionalLength, additionalIsDotted)
        };
    }

    /// <summary>
    /// Constructor. <br/>コンストラクタ
    /// </summary>
    /// <param name="baseNote">The note of base. <br/>基本となる音符。二つ目の音符の音の高さはこの音符と同じになる。</param>
    /// <param name="additionalNotes">Notes of tie. <br/>追加する音符</param>
    public Tie(Note baseNote, IReadOnlyCollection<Note> additionalNotes)
    {
        BaseNote = baseNote;
        AdditionalNotes = new List<Note>(additionalNotes);
    }

    /// <summary>
    /// Count of notes.
    /// </summary>
    public int Count => AdditionalNotes.Count + 1;

    /// <summary>
    /// The base note. <br/>基本の音符
    /// </summary>
    public Note BaseNote { get; }

    /// <summary>
    /// The additional notes. <br/>追加の音符のリスト
    /// </summary>
    public IReadOnlyCollection<Note> AdditionalNotes { get; }

    public short[] GenerateWave(SoundFormat format, int tempo, int length, WaveTypeBase waveType)
    {
        return BaseNote.GenerateWave(format, tempo, length, waveType);
    }

    public short[] GenerateWave(SoundFormat format, int tempo, WaveTypeBase waveType)
    {
        var length = GetWaveArrayLength(format, tempo);
        return GenerateWave(format, tempo, length, waveType);
    }

    public int GetWaveArrayLength(SoundFormat format, int tempo)
    {
        var length = BaseNote.GetWaveArrayLength(format, tempo);
        foreach (var note in AdditionalNotes)
        {
            length += SoundWaveLengthCalculator.Calculate(format, tempo, note.Length, note.IsDotted);
        }
        return length;
    }

    /// <summary>
    /// Creates a clone of the tie. <br/>タイのクローンを作成するメソッド。
    /// </summary>
    /// <returns>A new instance of the tie with the same properties. <br/>同じプロパティを持つタイの新しいインスタンス</returns>
    public Tie Clone()
    {
        var newTie = new Tie(BaseNote.Clone(), AdditionalNotes.Select(note => note.Clone()).ToArray());
        return newTie;
    }

    ISoundComponent ISoundComponent.Clone()
    {
        return Clone();
    }
}
