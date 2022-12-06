namespace SoundMaker.Sounds;
/// <summary>
/// interface for wave. 波を表すインターフェイス
/// </summary>
public interface IWave
{
    /// <summary>
    /// volume of mixed wave.(0 ~ 100) ミックス済みの波形の音量
    /// </summary>
    int Volume { get; }

    /// <summary>
    /// length of wave at initialization. if stereo wave, return the longer length in the array at initialization.  初期化時の波形データの配列の長さ。ステレオの場合、左右で長い方の長さを返す。
    /// </summary>
    [Obsolete("if you want to get length of bytes, call GetLengthOfBytes()")]
    int Length { get; }

    /// <summary>
    /// get length of bytes of wave data. 波形データのバイト列の長さを取得するメソッド。
    /// </summary>
    /// <param name="bitRate">bit rate. ビットレート</param>
    /// <returns>length of bytes of wave data.</returns>
    int GetLengthOfBytes(BitRateType bitRate);

    /// <summary>
    /// get the array of bytes of wave data. 波形データのバイト列を取得するメソッド。
    /// </summary>
    /// <param name="bitRate">bit rate. ビットレート</param>
    /// <returns>bytes of wave data. 波形データのバイト列 : byte[]</returns>
    byte[] GetBytes(BitRateType bitRate);
}
