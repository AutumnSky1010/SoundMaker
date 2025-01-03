using SoundMaker.Sounds.Score;

namespace SoundMaker.Sounds.SoundChannels;
public interface ISoundChannel : IEnumerable<ISoundComponent>
{
    /// <summary>
    /// Get sound component at index. <br/>index番目のサウンドコンポーネントを取得する
    /// </summary>
    /// <param name="index">Index. <br/>何番目かを表す整数</param>
    /// <returns>Sound component. <br/>サウンドコンポーネント</returns>
    ISoundComponent this[int index] { get; }

    /// <summary>
    /// The total number of sound components the internal data structure can hold without resizing. <br/>内部リストがサイズを変えないで保持できるサウンドコンポーネントの個数
    /// </summary>
    public int Capacity { get; }

    /// <summary>
    /// Count of sound components. <br/>サウンドコンポーネントの個数
    /// </summary>
    public int ComponentCount { get; }

    /// <summary>
    /// Length of wave data. <br/>音の波形データを表す配列の長さ
    /// </summary>
    public int WaveArrayLength { get; }

    /// <summary>
    /// Quarter note/rest per minute. <br/>一分間の四分音符・休符の数
    /// </summary>
    public int Tempo { get; }

    /// <summary>
    /// Format of the sound. <br/>音のフォーマット
    /// </summary>
    SoundFormat Format { get; }

    /// <summary>
    /// Direction of hearing. <br/>ステレオサウンドの場合、左右どちらから音が出るか
    /// </summary>
    PanType PanType { get; }

    /// <summary>
    /// Generate wave data. <br/>音の波形データを生成するメソッド。
    /// </summary>
    /// <returns>The array of wave data. <br/>音の波形データの配列 : short[]</returns>
    short[] GenerateWave();

    /// <summary>
    /// Add sound component to this. <br/>サウンドコンポーネントを追加するメソッド。
    /// </summary>
    /// <param name="component">The sound component to be added to this. <br/>追加するサウンドコンポーネント</param>
    void Add(ISoundComponent component);

    /// <summary>
    /// Remove the sound component at index. <br/>index番目のサウンドコンポーネントを削除するメソッド。
    /// </summary>
    /// <param name="index">The index of the sound component to remove. <br/>削除するサウンドコンポーネントのインデックス</param>
    void RemoveAt(int index);

    /// <summary>
    /// Clear sound components in this. <br/>チャンネル内のサウンドコンポーネントを空にするメソッド。
    /// </summary>
    void Clear();
}
