namespace SoundMaker.Sounds;
/// <summary>
/// 波を表すインターフェイス
/// </summary>
public interface IWave
{
    /// <summary>
    /// ミックス済みの波形の音量
    /// </summary>
    int Volume { get; }

    /// <summary>
    /// 波形データの配列の長さ。ステレオの場合、GetBytes()の波形データの配列は、二倍の長さになる。
    /// </summary>
    int Length { get; }

    /// <summary>
    /// 波形データのバイト列を取得するメソッド。
    /// </summary>
    /// <param name="bitRate">ビットレート</param>
    /// <returns>波形データのバイト列 : byte[]</returns>
    byte[] GetBytes(BitRateType bitRate);
}
