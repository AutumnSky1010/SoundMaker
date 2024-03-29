﻿namespace SoundMaker.WaveFile;
/// <summary>
/// Chunk of format for the .wav file. フォーマットチャンクを表す構造体
/// </summary>
public readonly struct FormatChunk : IChunk
{
    /// <summary>
    /// constructor. コンストラクタ
    /// </summary>
    /// <param name="samplingFrequency">sampling frequency. サンプリング周波数</param>
    /// <param name="bitRate">bit rate. 量子化ビット数</param>
    /// <param name="channel">type of channels count. チャンネル数</param>
    public FormatChunk(SamplingFrequencyType samplingFrequency, BitRateType bitRate, ChannelType channel)
    {
        Channel = (ushort)channel;
        BitRate = (ushort)bitRate;
        SamplingFrequency = (uint)samplingFrequency;
        BlockSize = (ushort)(BitRate * Channel / 8);
        ByteSizePerSecond = BlockSize * SamplingFrequency;
    }

    // チャンクサイズは16byte
    private uint ChankSize { get; } = 0x00000010;

    private ushort SoundFormat { get; } = 0x0001;

    private ushort Channel { get; }

    /// <summary>
    /// sampling frequency. サンプリング周波数
    /// </summary>
    public uint SamplingFrequency { get; }

    private uint ByteSizePerSecond { get; }

    private ushort BlockSize { get; }

    /// <summary>
    /// bit rate. 量子化ビット数
    /// </summary>
    public ushort BitRate { get; }

    /// <summary>
    /// get array of this chunk. フォーマットチャンクのバイト列を取得するメソッド。
    /// </summary>
    /// <returns>bytes of this chunk. フォーマットチャンクのバイト列 : byte[]</returns>
    public byte[] GetBytes()
    {
        var result = BitConverter.GetBytes(0x20746D66);
        result = result.Concat(BitConverter.GetBytes(ChankSize)).ToArray();
        result = result.Concat(BitConverter.GetBytes(SoundFormat)).ToArray();
        result = result.Concat(BitConverter.GetBytes(Channel)).ToArray();
        result = result.Concat(BitConverter.GetBytes(SamplingFrequency)).ToArray();
        result = result.Concat(BitConverter.GetBytes(ByteSizePerSecond)).ToArray();
        result = result.Concat(BitConverter.GetBytes(BlockSize)).ToArray();
        return result.Concat(BitConverter.GetBytes(BitRate)).ToArray();
    }
}
