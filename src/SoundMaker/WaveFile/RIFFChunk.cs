namespace SoundMaker.WaveFile;
/// <summary>
/// chunk of riff. RIFFチャンクを表す構造体
/// </summary>
public struct RIFFChunk : IChunk
{
    /// <summary>
    /// constructor. コンストラクタ
    /// </summary>
    /// <param name="Size">the number of 8 byte subtracted from the overall file size. ファイル全体サイズからRIFFとWAVEのバイト数(8B)を引いた数。</param>
    public RIFFChunk(uint Size)
    {
        this.Size = Size;
    }

    private uint Size { get; }
    // 0x45564157 は WAVEの意味
    private uint Format { get; } = 0x45564157;
    public byte[] GetBytes()
    {
        // 0x46464952 は RIFF
        var result = BitConverter.GetBytes(0x46464952);
        result = result.Concat(BitConverter.GetBytes(this.Size)).ToArray();
        return result.Concat(BitConverter.GetBytes(this.Format)).ToArray();
    }
}
