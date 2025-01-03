namespace SoundMaker.Sounds.Score;

/// <summary>
/// Type of length. (ex. "quarter" note). <br/>長さのタイプを列挙（音楽的な、「四分」音符、「全」休符のような長さを表す。）
/// </summary>
public enum LengthType
{
    /// <summary>
    /// Whole. <br/>全音符・休符
    /// </summary>
    Whole = 1,

    /// <summary>
    /// Half. <br/>二分音符・休符
    /// </summary>
    Half = 2,

    /// <summary>
    /// Quarter. <br/>四分音符・休符
    /// </summary>
    Quarter = 4,

    /// <summary>
    /// 8th. <br/>八分音符・休符
    /// </summary>
    Eighth = 8,

    /// <summary>
    /// 16th. <br/>十六分音符・休符
    /// </summary>
    Sixteenth = 16,

    /// <summary>
    /// 32nd. <br/>三十二分音符・休符
    /// </summary>
    ThirtySecond = 32,

    /// <summary>
    /// 64th. <br/>六十四分音符・休符
    /// </summary>
    SixtyFourth = 64,
}
