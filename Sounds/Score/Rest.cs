namespace SoundMaker.Sounds.Score;
/// <summary>
/// 休符を表すクラス
/// </summary>
public class Rest : BasicSoundComponentBase
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="length">長さ（音楽的な、「四分」音符、「全」休符のような長さを表す。）</param>
    /// <param name="isDotted">付点かどうかを表す論理型</param>
    public Rest(LengthType length, bool isDotted = false)
        : base(length, isDotted) { }

    public override ushort[] GetSquareWave(SoundFormat format, SquareWaveRatio squareWaveRatio, int tempo)
        => this.GetWave(format, tempo);

    public override ushort[] GetSquareWave(SoundFormat format, SquareWaveRatio squareWaveRatio, int tempo, int length)
        => this.GetWave(format, tempo);

    public override ushort[] GetTriangleWave(SoundFormat format, int tempo)
        => this.GetWave(format, tempo);

    public override ushort[] GetTriangleWave(SoundFormat format, int tempo, int length)
        => this.GetWave(format, tempo, length);

    /// <summary>
    /// 長さ分休む（０埋めの配列を返す）
    /// </summary>
    /// <param name="format">音のフォーマット</param>
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
    /// <param name="format">音のフォーマット</param>
    /// <param name="tempo">一分間の休符の数</param>
    /// <param name="length">配列の長さ</param>
    /// <returns>0埋めされた配列 : unsigned short[]</returns>
    private ushort[] GetWave(SoundFormat format, int tempo, int length)
        => Enumerable.Repeat<ushort>(0, length).ToArray();
}
