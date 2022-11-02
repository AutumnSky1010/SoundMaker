namespace SoundMaker.WaveFile;
/// <summary>
/// 量子化ビット数の種類を表す列挙型
/// </summary>
public enum BitRateType : ushort
{
    /// <summary>
    /// 16ビット
    /// </summary>
    SixteenBit = 0x0010,
    /// <summary>
    /// 8ビット
    /// </summary>
    EightBit = 0x0008
}
