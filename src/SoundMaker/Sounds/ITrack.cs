using SoundMaker.Sounds.WaveTypes;

namespace SoundMaker.Sounds;

/// <summary>
/// Interface for track types that can generate waveforms. <br/>
/// 波形を生成できるトラック型のインターフェース。
/// </summary>
public interface ITrack
{
    /// <summary>
    /// Gets or sets the wave type. <br/>
    /// 波形タイプを取得または設定するプロパティ。
    /// </summary>
    WaveTypeBase WaveType { get; set; }

    /// <summary>
    /// Gets or sets the left-right audio balance. <br/>
    /// Takes values from -1.0 (left) to 1.0 (right). <br/>
    /// 左右の音量バランスを取得または設定するプロパティ。<br/>
    /// -1.0が左、1.0が右側。
    /// </summary>
    double Pan { get; set; }

    /// <summary>
    /// Gets the start time in index. <br/>
    /// 開始時間（インデクス）を取得するプロパティ。
    /// </summary>
    int StartIndex { get; }

    /// <summary>
    /// Gets the end time in index. <br/>
    /// 終了時間（インデクス）を取得するプロパティ。
    /// </summary>
    int EndIndex { get; }

    /// <summary>
    /// Gets the length of the wave array. <br/>
    /// 波形配列の長さを取得するプロパティ。
    /// </summary>
    int WaveArrayLength { get; }

    /// <summary>
    /// Gets the count of sound components. <br/>
    /// サウンドコンポーネントの数を取得するプロパティ。
    /// </summary>
    int Count { get; }

    /// <summary>
    /// Generates a wave based on the sound components. <br/>
    /// サウンドコンポーネントに基づいて波形を生成するメソッド。
    /// </summary>
    /// <returns>
    /// An array of shorts representing the generated wave. <br/>
    /// 生成された波形を表すショート型の配列。
    /// </returns>
    short[] GenerateWave();

    /// <summary>
    /// Generates the waveform data for the specified range. <br/>
    /// 指定した範囲の波形データを生成するメソッド。
    /// </summary>
    /// <param name="startIndex">The starting index of the range. <br/> 範囲の開始インデックス。</param>
    /// <param name="endIndex">The ending index of the range. <br/> 範囲の終了インデックス。</param>
    /// <returns>The waveform data for the specified range. <br/> 指定範囲の波形データ。</returns>
    short[] GeneratePartialWave(int startIndex, int endIndex);

    /// <summary>
    /// Creates a clone of the track. <br/>
    /// トラックのクローンを作成するメソッド。
    /// </summary>
    /// <returns>A new instance of the track with the same properties. <br/> 同じプロパティを持つトラックの新しいインスタンス。</returns>
    ITrack Clone();
}
