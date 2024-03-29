﻿namespace SoundMaker.Sounds;
/// <summary>
/// format of the sound. 音のフォーマットを表す構造体
/// </summary>
public readonly struct SoundFormat
{
    /// <summary>
    /// constructor. コンストラクタ
    /// </summary>
    /// <param name="samplingFrequency">sampling frequency. サンプリング周波数</param>
    /// <param name="bitRate">bit rate.量子化ビット数</param>
    /// <param name="channel">type of channels count. チャンネル数</param>
    public SoundFormat(SamplingFrequencyType samplingFrequency, BitRateType bitRate, ChannelType channel)
    {
        Channel = channel;
        SamplingFrequency = samplingFrequency;
        BitRate = bitRate;
    }

    /// <summary>
    /// type of channels count. チャンネル数
    /// </summary>
    public ChannelType Channel { get; }

    /// <summary>
    /// sampling frequency. サンプリング周波数
    /// </summary>
    public SamplingFrequencyType SamplingFrequency { get; }

    /// <summary>
    /// bit rate. 量子化ビット数
    /// </summary>
    public BitRateType BitRate { get; }
}
