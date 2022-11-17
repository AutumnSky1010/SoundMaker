namespace SoundMaker.WaveFile;

/// <summary>
/// interface of chunks. チャンクを表すインターフェイス
/// </summary>
public interface IChunk
{
    /// <summary>
    /// get bytes from the chunk. チャンクのバイト列を取得するメソッド。
    /// </summary>
    /// <returns>byte array of the chunk.チャンクのバイト列 : byte[]</returns>
    byte[] GetBytes();
}
