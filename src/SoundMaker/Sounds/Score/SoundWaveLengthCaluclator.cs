namespace SoundMaker.Sounds.Score;
/// <summary>
/// 音の配列の長さを計算するクラス
/// </summary>
internal static class SoundWaveLengthCaluclator
{
    /// <summary>
    /// メソッド。
    /// </summary>
    /// <param name="format">SoundFormat in SoundMaker.Sounds of namespace.</param>
    /// <param name="tempo">quarter note/rest per minute. 一分間の四分音符・休符の数</param>
    /// <param name="length">音の長さ</param>
    /// <param name="isDotted">付点の場合はTrueに設定する</param>
    /// <returns>音の配列の長さ : int</returns>
    /// <exception cref="ArgumentOutOfRangeException">Tempo must be non-negative and greater than 0.</exception>
    public static int Caluclate(SoundFormat format, int tempo, LengthType length, bool isDotted)
    {
        if (tempo <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(tempo), "Tempo must be non-negative and greater than 0.");
        }
        // 128分音符・休符の長さを求める（最小単位なので、これを掛け算すれば正確な長さを出せる。）
        // 一個分の長さ＝サンプリング周波数 * 60秒 / (テンポ(一分間に四分音符何個か) * 32(四分音符の長さを32で割ると128分音符))
        int baseNoteLength = (int)((int)format.SamplingFrequency * 60 / (tempo * 32d));
        // リストの長さ = 128分音符一個分の長さ * (128 / 長さ)　
        // 例) 128 / 4 = 32より、四分音符一個分の長さは、128分音符32個分の長さ。
        int listLength = baseNoteLength * (128 / (int)length);
        // 付点の場合1.5倍の長さになる。
        if (isDotted)
        {
            listLength += baseNoteLength * (128 / (int)length / 2);
        }
        return listLength;
    }
}
