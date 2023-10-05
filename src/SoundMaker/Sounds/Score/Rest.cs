using SoundMaker.Sounds.WaveTypes;

namespace SoundMaker.Sounds.Score;
/// <summary>
/// the rest. 休符を表すクラス
/// </summary>
public class Rest : BasicSoundComponentBase
{
    /// <summary>
    /// constructor コンストラクタ
    /// </summary>
    /// <param name="length">length (ex. "quarter" note) 長さ（音楽的な、「四分」音符、「全」休符のような長さを表す。）</param>
    /// <param name="isDotted">is note/rest dotted. 付点かを表す論理型</param>
    public Rest(LengthType length, bool isDotted = false)
        : base(length, isDotted) { }

    public override ushort[] GenerateWave(SoundFormat format, int tempo, int length, WaveTypeBase waveType)
        => this.GetWave(format, tempo, length);

    public override ushort[] GenerateWave(SoundFormat format, int tempo, WaveTypeBase waveType)
        => this.GetWave(format, tempo);

    /// <summary>
    /// 長さの分休む（０埋めの配列を返す）
    /// </summary>
    /// <param name="format">format of the sound.音のフォーマット</param>
    /// <param name="tempo">一分間の休符の数</param>
    /// <returns>0埋めされた配列 : unsigned short[]</returns>
    private ushort[] GetWave(SoundFormat format, int tempo)
    {
        int length = this.GetWaveArrayLength(format, tempo);
        return this.GetWave(format, tempo, length);
    }

    /// <summary>
    /// 長さ分休む（０埋めの配列を返す）
    /// </summary>
    /// <param name="format">format of the sound.音のフォーマット</param>
    /// <param name="tempo">一分間の休符の数</param>
    /// <param name="length">length of the array. 配列の長さ</param>
    /// <returns>0埋めされた配列 : unsigned short[]</returns>
    private ushort[] GetWave(SoundFormat format, int tempo, int length)
        => Enumerable.Repeat<ushort>(0, length).ToArray();
}
