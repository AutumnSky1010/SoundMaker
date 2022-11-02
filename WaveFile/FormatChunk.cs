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
        this._channel = (ushort)channel;
        this.BitRate = (ushort)bitRate;
        this.SamplingFrequency = (uint)samplingFrequency;
        this._blockSize = (ushort)(this.BitRate * this._channel / 8);
        this._byteSizePerSecond = this._blockSize * this.SamplingFrequency;
    }

    private uint _chankSize { get; } = 0x00000010;

    private ushort _soundFormat { get; } = 0x0001;

    private ushort _channel { get; }

    public uint SamplingFrequency { get; }

    private uint _byteSizePerSecond { get; }

    private ushort _blockSize { get; }

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
        result = result.Concat(BitConverter.GetBytes(this._chankSize)).ToArray();
        result = result.Concat(BitConverter.GetBytes(this._soundFormat)).ToArray();
        result = result.Concat(BitConverter.GetBytes(this._channel)).ToArray();
        result = result.Concat(BitConverter.GetBytes(this.SamplingFrequency)).ToArray();
        result = result.Concat(BitConverter.GetBytes(this._byteSizePerSecond)).ToArray();
        result = result.Concat(BitConverter.GetBytes(this._blockSize)).ToArray();
        return result.Concat(BitConverter.GetBytes(this.BitRate)).ToArray();
    }
}
