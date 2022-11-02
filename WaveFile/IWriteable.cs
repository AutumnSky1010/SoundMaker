namespace SoundMaker.WaveFile;
/// <summary>
/// 書き込みが可能なオブジェクトのインターフェイス
/// </summary>
public interface IWriteable
{
    /// <summary>
    /// 書き込むメソッド。
    /// </summary>
    /// <param name="path">書き込み先のファイルのパス</param>
    void Write(string path);
}
