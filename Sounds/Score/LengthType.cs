namespace SoundMaker.Sounds.Score;
/// <summary>
/// 長さのタイプを列挙（音楽的な、「四分」音符、「全」休符のような長さを表す。）
/// </summary>
public enum LengthType
{
    /// <summary>
    /// 全音符・休符
    /// </summary>
    Whole = 1,
    /// <summary>
    /// 二分音符・休符
    /// </summary>
    Half = 2,
    /// <summary>
    /// 四分音符・休符
    /// </summary>
    Quarter = 4,
    /// <summary>
    /// 八分音符・休符
    /// </summary>
    Eighth = 8,
    /// <summary>
    /// 十六分音符・休符
    /// </summary>
    Sixteenth = 16,
    /// <summary>
    /// 三十二分音符・休符
    /// </summary>
    ThrthirtySecond = 32,
    /// <summary>
    /// 六十四分音符・休符
    /// </summary>
    SixtyFourth = 64,
}
