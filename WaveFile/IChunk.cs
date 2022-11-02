namespace SoundMaker.WaveFile;

/// <summary>
/// チャンクを表すインターフェイス
/// </summary>
public interface IChunk
{
    /// <summary>
    /// チャンクのバイト列を取得するメソッド。
    /// </summary>
    /// <returns>チャンクのバイト列 : byte[]</returns>
    byte[] GetBytes();
}
