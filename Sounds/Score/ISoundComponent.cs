using SoundMaker.Sounds.WaveTypes;

namespace SoundMaker.Sounds.Score;

/// <summary>
/// 音の部品を表すインターフェイス
/// </summary>
public interface ISoundComponent
{
    /// <summary>
    /// return length of the sound array. 音の配列の長さを取得するメソッド。
    /// </summary>
    /// <param name="format">format of the sound.音のフォーマット</param>
    /// <param name="tempo">quarter note/rest per minute. 一分間の四分音符・休符の数</param>
    /// <returns>length of array. 配列の長さ : int</returns>
    int GetWaveArrayLength(SoundFormat format, int tempo);

    /// <summary>
    /// 矩形波の波形データの配列を生成するメソッド。
    /// </summary>
    /// <param name="format">format of the sound.音のフォーマット</param>
    /// <param name="tempo">quarter note/rest per minute. 一分間の四分音符・休符の数</param>
    /// <param name="length">length of the array. 配列の長さ</param>
    /// <param name="waveType">type of wave.波形の種類</param>
    /// <returns>data of wave. 波形データ : unsigned short[]</returns>
    ushort[] GenerateWave(SoundFormat format, int tempo, int length, WaveTypeBase waveType);

    /// <summary>
    /// 矩形波の波形データの配列を生成するメソッド。
    /// </summary>
    /// <param name="format">format of the sound.音のフォーマット</param>
    /// <param name="tempo">quarter note/rest per minute. 一分間の四分音符・休符の数</param>
    /// <param name="waveType">type of wave.波形の種類</param>
    /// <returns>data of wave. 波形データ : unsigned short[]</returns>
    ushort[] GenerateWave(SoundFormat format, int tempo, WaveTypeBase waveType);
}
