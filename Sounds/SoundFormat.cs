namespace SoundMaker.Sounds;
/// <summary>
/// 音のフォーマットを表す構造体
/// </summary>
public struct SoundFormat
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="samplingFrequency">サンプリング周波数</param>
    /// <param name="bitRate">量子化ビット数</param>
    /// <param name="channel">チャンネル数</param>
    public SoundFormat(SamplingFrequencyType samplingFrequency, BitRateType bitRate, ChannelType channel)
    {
        this.Channel = channel;
        this.SamplingFrequency = samplingFrequency;
        this.BitRate = bitRate;
    }

    /// <summary>
    /// チャンネル数
    /// </summary>
    public ChannelType Channel { get; }

    /// <summary>
    /// サンプリング周波数
    /// </summary>
    public SamplingFrequencyType SamplingFrequency { get; }

    /// <summary>
    /// 量子化ビット数
    /// </summary>
    public BitRateType BitRate { get; }
}
