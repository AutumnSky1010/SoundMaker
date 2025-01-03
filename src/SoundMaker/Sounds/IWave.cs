namespace SoundMaker.Sounds;

/// <summary>
/// Interface for wave. 波を表すインターフェイス
/// </summary>
public interface IWave
{
    /// <summary>
    /// Get the length of bytes of wave data. <br/>波形データのバイト列の長さを取得するメソッド。
    /// </summary>
    /// <param name="bitRate">Quantization bit rate. <br/>量子化ビット数</param>
    /// <returns>Length of bytes of wave data.</returns>
    int GetLengthOfBytes(BitRateType bitRate);

    /// <summary>
    /// Get the array of bytes of wave data. <br/>波形データのバイト列を取得するメソッド。
    /// </summary>
    /// <param name="bitRate">Quantization bit rate. <br/>量子化ビット数</param>
    /// <returns>Array of bytes of wave data. <br/>波形データのバイト列 : byte[]</returns>
    byte[] GetBytes(BitRateType bitRate);
}
