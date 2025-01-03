using SoundMaker.Sounds.WaveTypes;

namespace SoundMaker.Sounds.Score;

/// <summary>
/// Interface for sound components. <br/>音の部品を表すインターフェイス
/// </summary>
public interface ISoundComponent
{
    /// <summary>
    /// Return length of the sound array. <br/>音の配列の長さを取得するメソッド。
    /// </summary>
    /// <param name="format">Format of the sound. <br/>音のフォーマット</param>
    /// <param name="tempo">Quarter note/rest per minute. <br/>一分間の四分音符・休符の数</param>
    /// <returns>Length of array. <br/>配列の長さ : int</returns>
    /// <exception cref="ArgumentOutOfRangeException">Tempo must be non-negative and greater than 0.</exception>
    int GetWaveArrayLength(SoundFormat format, int tempo);

    /// <summary>
    /// Generate the wave of wave type. <br/>波形の種類に基づいて波形データの配列を生成するメソッド。
    /// </summary>
    /// <param name="format">Format of the sound. <br/>音のフォーマット</param>
    /// <param name="tempo">Quarter note/rest per minute. <br/>一分間の四分音符・休符の数</param>
    /// <param name="length">Length of the array. <br/>配列の長さ</param>
    /// <param name="waveType">Type of wave. <br/>波形の種類</param>
    /// <returns>Data of wave. <br/>波形データ : short[]</returns>
    /// <exception cref="ArgumentOutOfRangeException">Tempo must be non-negative and greater than 0.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Length must be non-negative.</exception>
    short[] GenerateWave(SoundFormat format, int tempo, int length, WaveTypeBase waveType);

    /// <summary>
    /// Generate the wave of wave type. <br/>波形の種類に基づいて波形データの配列を生成するメソッド。
    /// </summary>
    /// <param name="format">Format of the sound. <br/>音のフォーマット</param>
    /// <param name="tempo">Quarter note/rest per minute. <br/>一分間の四分音符・休符の数</param>
    /// <param name="waveType">Type of wave. <br/>波形の種類</param>
    /// <returns>Data of wave. <br/>波形データ : short[]</returns>
    /// <exception cref="ArgumentOutOfRangeException">Tempo must be non-negative and greater than 0.</exception>
    short[] GenerateWave(SoundFormat format, int tempo, WaveTypeBase waveType);

    /// <summary>
    /// Creates a clone of the sound component. <br/>サウンドコンポーネントのクローンを作成するメソッド。
    /// </summary>
    /// <returns>
    /// A new instance of the sound component with the same properties. <br/>同じプロパティを持つサウンドコンポーネントの新しいインスタンス
    /// </returns>
    ISoundComponent Clone();
}
