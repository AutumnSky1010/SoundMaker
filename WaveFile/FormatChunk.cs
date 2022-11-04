namespace SoundMaker.WaveFile;
/// <summary>
/// フォーマットチャンクを表す構造体
/// </summary>
public struct FormatChunk : IChunk
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="samplingFrequency">サンプリング周波数</param>
    /// <param name="bitRate">量子化ビット数</param>
    /// <param name="channel">チャンネル数</param>
    public FormatChunk(SamplingFrequencyType samplingFrequency, BitRateType bitRate, ChannelType channel)
    {
        this.Channel = (ushort)channel;
        this.BitRate = (ushort)bitRate;
        this.SamplingFrequency = (uint)samplingFrequency;
        this.BlockSize = (ushort)(this.BitRate * this.Channel / 8);
        this.ByteSizePerSecond = this.BlockSize * this.SamplingFrequency;
    }

    private uint ChankSize { get; } = 0x00000010;

    private ushort SoundFormat { get; } = 0x0001;

    private ushort Channel { get; }

    public uint SamplingFrequency { get; }

    private uint ByteSizePerSecond { get; }

    private ushort BlockSize { get; }

    /// <summary>
    /// 量子化ビット数
    /// </summary>
    public ushort BitRate { get; }

    /// <summary>
    /// フォーマットチャンクのバイト列を取得するメソッド。
    /// </summary>
    /// <returns>フォーマットチャンクのバイト列 : byte[]</returns>
    public byte[] GetBytes()
    {
        var result = BitConverter.GetBytes(0x20746D66);
        result = result.Concat(BitConverter.GetBytes(this.ChankSize)).ToArray();
        result = result.Concat(BitConverter.GetBytes(this.SoundFormat)).ToArray();
        result = result.Concat(BitConverter.GetBytes(this.Channel)).ToArray();
        result = result.Concat(BitConverter.GetBytes(this.SamplingFrequency)).ToArray();
        result = result.Concat(BitConverter.GetBytes(this.ByteSizePerSecond)).ToArray();
        result = result.Concat(BitConverter.GetBytes(this.BlockSize)).ToArray();
        return result.Concat(BitConverter.GetBytes(this.BitRate)).ToArray();
    }
}
