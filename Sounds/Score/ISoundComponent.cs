namespace SoundMaker.Sounds.Score;

/// <summary>
/// 音の部品を表すインターフェイス
/// </summary>
public interface ISoundComponent
{
    /// <summary>
    /// 音の配列の長さを取得するメソッド。
    /// </summary>
    /// <param name="format">音のフォーマット</param>
    /// <param name="tempo">一分間の四分音符・休符の数</param>
    /// <returns>配列の長さ : int</returns>
    int GetWaveArrayLength(SoundFormat format, int tempo);

    /// <summary>
    /// 矩形波の波形データの配列を生成するメソッド。
    /// </summary>
    /// <param name="format">音のフォーマット</param>
    /// <param name="squareWaveRatio">デューティ比</param>
    /// <param name="tempo">一分間の四分音符・休符の数</param>
    /// <returns>波形データ : unsigned short[]</returns>
    ushort[] GetSquareWave(SoundFormat format, SquareWaveRatio squareWaveRatio, int tempo);

    /// <summary>
    /// 三角波の波形データの配列を生成するメソッド。
    /// </summary>
    /// <param name="format">音のフォーマット</param>
    /// <param name="tempo">一分間の四分音符・休符の数</param>
    /// <returns>波形データ : unsigned short[]</returns>
    ushort[] GetTriangleWave(SoundFormat format, int tempo);

    /// <summary>
    /// 配列の長さを強制的に指定して、矩形波の波形データの配列を生成するメソッド。
    /// </summary>
    /// <param name="format">音のフォーマット</param>
    /// <param name="squareWaveRatio">デューティ比</param>
    /// <param name="tempo">一分間の四分音符・休符の数</param>
    /// <param name="length">戻り値の配列の長さ</param>
    /// <returns>波形データ : unsigned short[]</returns>
    ushort[] GetSquareWave(SoundFormat format, SquareWaveRatio squareWaveRatio, int tempo, int length);

    /// <summary>
    /// 配列の長さを強制的に指定して、三角波の波形データの配列を生成するメソッド。
    /// </summary>
    /// <param name="format">音のフォーマット</param>
    /// <param name="tempo">一分間の四分音符・休符の数</param>
    /// <param name="length">戻り値の配列の長さ</param>
    /// <returns>波形データ : unsigned short[]</returns>
    ushort[] GetTriangleWave(SoundFormat format, int tempo, int length);

    ushort[] GetNoiseWave(SoundFormat format, int tempo);

    ushort[] GetNoiseWave(SoundFormat format, int tempo, int length);
}
