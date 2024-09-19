namespace SoundMaker.Sounds;
/// <summary>
/// interface for wave. 波を表すインターフェイス
/// </summary>
public interface IWave
{
    /// <summary>
    /// get length of bytes of wave data. 波形データのバイト列の長さを取得するメソッド。
    /// </summary>
    /// <param name="bitRate">quantization bit rate. 量子化ビット数</param>
    /// <returns>length of bytes of wave data.</returns>
    int GetLengthOfBytes(BitRateType bitRate);

    /// <summary>
    /// get the array of bytes of wave data. 波形データのバイト列を取得するメソッド。
    /// </summary>
    /// <param name="bitRate">quantization bit rate. 量子化ビット数</param>
    /// <returns>bytes of wave data. 波形データのバイト列 : byte[]</returns>
    byte[] GetBytes(BitRateType bitRate);
}
