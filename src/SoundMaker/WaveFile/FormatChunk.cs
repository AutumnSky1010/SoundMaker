namespace SoundMaker.WaveFile;

/// <summary>
/// Chunk of format for the .wav file. <br/>フォーマットチャンクを表す構造体
/// </summary>
public readonly struct FormatChunk : IChunk
{
    /// <summary>
    /// Constructor. <br/>コンストラクタ
    /// </summary>
    /// <param name="samplingFrequency">Sampling frequency. <br/>サンプリング周波数</param>
    /// <param name="bitRate">Bit rate. <br/>量子化ビット数</param>
    /// <param name="channel">Type of channels count. <br/>チャンネル数</param>
    public FormatChunk(SamplingFrequencyType samplingFrequency, BitRateType bitRate, ChannelType channel)
    {
        Channel = (ushort)channel;
        BitRate = (ushort)bitRate;
        SamplingFrequency = (uint)samplingFrequency;
        BlockSize = (ushort)(BitRate * Channel / 8);
        ByteSizePerSecond = BlockSize * SamplingFrequency;
    }

    // Chunk size is 16 bytes
    // チャンクサイズは16byte
    private uint ChankSize { get; } = 0x00000010;

    private ushort SoundFormat { get; } = 0x0001;

    private ushort Channel { get; }

    /// <summary>
    /// Sampling frequency. <br/>サンプリング周波数
    /// </summary>
    public uint SamplingFrequency { get; }

    private uint ByteSizePerSecond { get; }

    private ushort BlockSize { get; }

    /// <summary>
    /// Bit rate. <br/>量子化ビット数
    /// </summary>
    public ushort BitRate { get; }

    /// <summary>
    /// Get array of this chunk. <br/>フォーマットチャンクのバイト列を取得するメソッド。
    /// </summary>
    /// <returns>Bytes of this chunk. <br/>フォーマットチャンクのバイト列 : byte[]</returns>
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
