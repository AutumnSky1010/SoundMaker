namespace SoundMaker.Sounds;
/// <summary>
/// interface for wave. 波を表すインターフェイス
/// </summary>
public interface IWave
{
    /// <summary>
    /// volume. ミックス済みの波形の音量
    /// </summary>
    int Volume { get; }

    /// <summary>
    /// length of wave. if stereo wave, return 1/2 of GetWaves().Length.  波形データの配列の長さ。ステレオの場合、GetBytes()の波形データの配列は、二倍の長さになる。
    /// </summary>
    int Length { get; }

    /// <summary>
    /// get array of wave data. 波形データのバイト列を取得するメソッド。
    /// </summary>
    /// <param name="bitRate">bit rate. ビットレート</param>
    /// <returns>bytes of wave data. 波形データのバイト列 : byte[]</returns>
    byte[] GetBytes(BitRateType bitRate);
}
