namespace SoundMaker.WaveFile;

/// <summary>
/// Interface for riff format chunks. <br/>チャンクを表すインターフェイス
/// </summary>
public interface IChunk
{
    /// <summary>
    /// Get bytes from the chunk. <br/>チャンクのバイト列を取得するメソッド。
    /// </summary>
    /// <returns>Byte array of the chunk.<br/>チャンクのバイト列 : byte[]</returns>
    byte[] GetBytes();
}
