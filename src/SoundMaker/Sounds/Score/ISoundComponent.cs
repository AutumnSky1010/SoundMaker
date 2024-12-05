using SoundMaker.Sounds.WaveTypes;

namespace SoundMaker.Sounds.Score;

/// <summary>
/// interface for sound components. 音の部品を表すインターフェイス
/// </summary>
public interface ISoundComponent
{
    /// <summary>
    /// return length of the sound array. 音の配列の長さを取得するメソッド。
    /// </summary>
    /// <param name="format">format of the sound.音のフォーマット</param>
    /// <param name="tempo">quarter note/rest per minute. 一分間の四分音符・休符の数</param>
    /// <returns>length of array. 配列の長さ : int</returns>
    /// <exception cref="ArgumentOutOfRangeException">Tempo must be non-negative and greater than 0.</exception>
    int GetWaveArrayLength(SoundFormat format, int tempo);

    /// <summary>
    /// generate the wave of wave type. 波形の種類に基づいて波形データの配列を生成するメソッド。
    /// </summary>
    /// <param name="format">format of the sound.音のフォーマット</param>
    /// <param name="tempo">quarter note/rest per minute. 一分間の四分音符・休符の数</param>
    /// <param name="length">length of the array. 配列の長さ</param>
    /// <param name="waveType">type of wave.波形の種類</param>
    /// <returns>data of wave. 波形データ :  short[]</returns>
    /// <exception cref="ArgumentOutOfRangeException">Tempo must be non-negative and greater than 0.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Length must be non-negative.</exception>
    short[] GenerateWave(SoundFormat format, int tempo, int length, WaveTypeBase waveType);

    /// <summary>
    /// generate the wave of wave type. 波形の種類に基づいて波形データの配列を生成するメソッド。
    /// </summary>
    /// <param name="format">format of the sound.音のフォーマット</param>
    /// <param name="tempo">quarter note/rest per minute. 一分間の四分音符・休符の数</param>
    /// <param name="waveType">type of wave.波形の種類</param>
    /// <returns>data of wave. 波形データ :  short[]</returns>
    /// <exception cref="ArgumentOutOfRangeException">Tempo must be non-negative and greater than 0.</exception>
    short[] GenerateWave(SoundFormat format, int tempo, WaveTypeBase waveType);

    /// <summary>
    /// Creates a clone of the sound component. <br/>
    /// サウンドコンポーネントのクローンを作成するメソッド。
    /// </summary>
    /// <returns>
    /// A new instance of the sound component with the same properties. <br/>
    /// 同じプロパティを持つサウンドコンポーネントの新しいインスタンス
    /// </returns>
    ISoundComponent Clone();
}
