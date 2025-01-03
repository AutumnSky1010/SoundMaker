namespace SoundMaker.WaveFile;
/// <summary>
/// The type which is expressed quantization bit rate of the sound. <br/>量子化ビット数の種類を表す列挙型
/// </summary>
public enum BitRateType : ushort
{
    /// <summary>
    /// 16bit. <br/>16ビット
    /// </summary>
    SixteenBit = 0x0010,
    /// <summary>
    /// 8bit. <br/>8ビット
    /// </summary>
    EightBit = 0x0008
}
