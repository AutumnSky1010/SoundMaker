namespace SoundMaker.WaveFile;
/// <summary>
/// Chunk of riff. <br/>RIFFチャンクを表す構造体
/// </summary>
/// <param name="Size">The number of 8 byte subtracted from the overall file size. <br/>ファイル全体サイズからRIFFとWAVEのバイト数(8B)を引いた数。</param>
public readonly struct RIFFChunk(uint Size) : IChunk
{
    private uint Size { get; } = Size;
    // 0x45564157 は WAVEの意味
    private uint Format { get; } = 0x45564157;
    public byte[] GetBytes()
    {
        // 0x46464952 は RIFF
        var result = BitConverter.GetBytes(0x46464952);
        result = result.Concat(BitConverter.GetBytes(Size)).ToArray();
        return result.Concat(BitConverter.GetBytes(Format)).ToArray();
    }
}
