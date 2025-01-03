namespace SoundMaker.Sounds;

/// <summary>
/// Format of the sound. <br/>音のフォーマットを表す構造体
/// </summary>
public readonly struct SoundFormat
{
    /// <summary>
    /// Constructor. <br/>コンストラクタ
    /// </summary>
    /// <param name="samplingFrequency">Sampling frequency. <br/>サンプリング周波数</param>
    /// <param name="bitRate">Bit rate. <br/>量子化ビット数</param>
    /// <param name="channel">Type of channels count. <br/>チャンネル数</param>
    public SoundFormat(SamplingFrequencyType samplingFrequency, BitRateType bitRate, ChannelType channel)
    {
        Channel = channel;
        SamplingFrequency = samplingFrequency;
        BitRate = bitRate;
    }

    /// <summary>
    /// Type of channels count. <br/>チャンネル数
    /// </summary>
    public ChannelType Channel { get; }

    /// <summary>
    /// Sampling frequency. <br/>サンプリング周波数
    /// </summary>
    public SamplingFrequencyType SamplingFrequency { get; }

    /// <summary>
    /// Bit rate. <br/>量子化ビット数
    /// </summary>
    public BitRateType BitRate { get; }
}
