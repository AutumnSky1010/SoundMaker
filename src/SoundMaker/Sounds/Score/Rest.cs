using SoundMaker.Sounds.WaveTypes;

namespace SoundMaker.Sounds.Score;

/// <summary>
/// The rest. <br/>休符を表すクラス
/// </summary>
/// <param name="length">Length (ex. "quarter" note). <br/>長さ（音楽的な、「四分」音符、「全」休符のような長さを表す。）</param>
/// <param name="isDotted">Is note/rest dotted. <br/>付点かを表す論理型</param>
public class Rest(LengthType length, bool isDotted = false) : BasicSoundComponentBase(length, isDotted)
{
    public override Rest Clone()
    {
        return new(Length, IsDotted);
    }

    public override short[] GenerateWave(SoundFormat format, int tempo, int length, WaveTypeBase waveType)
    {
        return GetWave(format, tempo, length);
    }

    public override short[] GenerateWave(SoundFormat format, int tempo, WaveTypeBase waveType)
    {
        return GetWave(format, tempo);
    }

    /// <summary>
    /// Rest for the duration (returns an array filled with zeroes). <br/>長さの分休む（０埋めの配列を返す）
    /// </summary>
    /// <param name="format">Format of the sound. <br/>音のフォーマット</param>
    /// <param name="tempo">Number of rests per minute. <br/>一分間の休符の数</param>
    /// <returns>Array filled with zeroes. <br/>0埋めされた配列 : short[]</returns>
    private short[] GetWave(SoundFormat format, int tempo)
    {
        var length = GetWaveArrayLength(format, tempo);
        return GetWave(format, tempo, length);
    }

    /// <summary>
    /// Rest for the duration (returns an array filled with zeroes). <br/>長さ分休む（０埋めの配列を返す）
    /// </summary>
    /// <param name="format">Format of the sound. <br/>音のフォーマット</param>
    /// <param name="tempo">Number of rests per minute. <br/>一分間の休符の数</param>
    /// <param name="length">Length of the array. <br/>配列の長さ</param>
    /// <returns>Array filled with zeroes. <br/>0埋めされた配列 : short[]</returns>
    private short[] GetWave(SoundFormat format, int tempo, int length)
    {
        return Enumerable.Repeat<short>(0, length).ToArray();
    }
}
