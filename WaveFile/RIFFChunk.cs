namespace SoundMaker.WaveFile;
/// <summary>
/// chunk of riff. RIFFチャンクを表す構造体
/// </summary>
public struct RIFFChunk : IChunk
{
    /// <summary>
    /// constructor. コンストラクタ
    /// </summary>
    /// <param name="Size">[file size] - 8byte ファイル全体サイズからRIFFとWAVEのバイト数(8B)を引いた数。</param>
    public RIFFChunk(uint Size)
    {
        this.Size = Size;
    }

    private uint Size { get; }
    private uint Format { get; } = 0x45564157;
    public byte[] GetBytes()
    {
        var result = BitConverter.GetBytes(0x46464952);
        result = result.Concat(BitConverter.GetBytes(this.Size)).ToArray();
        return result.Concat(BitConverter.GetBytes(this.Format)).ToArray();
    }
}
