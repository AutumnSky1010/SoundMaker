namespace SoundMaker.Sounds.Score;
/// <summary>
/// type of length.(ex. "quarter" note) 長さのタイプを列挙（音楽的な、「四分」音符、「全」休符のような長さを表す。）
/// </summary>
public enum LengthType
{
    /// <summary>
    /// whole. 全音符・休符
    /// </summary>
    Whole = 1,
    /// <summary>
    /// half. 二分音符・休符
    /// </summary>
    Half = 2,
    /// <summary>
    /// quarter. 四分音符・休符
    /// </summary>
    Quarter = 4,
    /// <summary>
    /// 8. 八分音符・休符
    /// </summary>
    Eighth = 8,
    /// <summary>
    /// 16. 十六分音符・休符
    /// </summary>
    Sixteenth = 16,
    /// <summary>
    /// 32. 三十二分音符・休符
    /// </summary>
    ThirtySecond = 32,
    /// <summary>
    /// 64. 六十四分音符・休符
    /// </summary>
    SixtyFourth = 64,
}
