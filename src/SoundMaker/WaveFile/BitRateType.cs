namespace SoundMaker.WaveFile;
/// <summary>
/// the type which is expressed bit rate of the sound. 量子化ビット数の種類を表す列挙型
/// </summary>
public enum BitRateType : ushort
{
    /// <summary>
    /// 16bit. 16ビット
    /// </summary>
    SixteenBit = 0x0010,
    /// <summary>
    /// 8bit. 8ビット
    /// </summary>
    EightBit = 0x0008
}
