using SoundMaker.Sounds.Score;

namespace SoundMaker.Sounds.SoundChannels;
public interface ISoundChannel : IEnumerable<ISoundComponent>
{
    /// <summary>
    /// get sound component at index. index番目のサウンドコンポーネントを取得する
    /// </summary>
    /// <param name="index">何番目かを表す整数</param>
    /// <returns>sound component.サウンドコンポーネント</returns>
    ISoundComponent this[int index] { get; }

    /// <summary>
    /// the total number of sound components the internal data structure can hold without resizing. 内部リストがサイズを変えないで保持できるサウンドコンポーネントの個数
    /// </summary>
    public int Capacity { get; }

    /// <summary>
    /// count of sound components. サウンドコンポーネントの個数
    /// </summary>
    public int ComponentCount { get; }

    /// <summary>
    /// length of wave data. 音の波形データを表す配列の長さ
    /// </summary>
    public int WaveArrayLength { get; }

    /// <summary>
    /// quarter note/rest per minute. 一分間の四分音符・休符の数
    /// </summary>
    public int Tempo { get; }

    /// <summary>
    /// format of the sound. 音のフォーマット
    /// </summary>
    SoundFormat Format { get; }

    /// <summary>
    /// direction of hearing. ステレオサウンドの場合、左右どちらから音が出るか
    /// </summary>
    PanType PanType { get; }

    /// <summary>
    /// generate wave data. 音の波形データを生成するメソッド。
    /// </summary>
    /// <returns>the array of wave data.音の波形データの配列 :  short[]</returns>
    short[] GenerateWave();

    /// <summary>
    /// add sound component to this. サウンドコンポーネントを追加するメソッド。
    /// </summary>
    /// <param name="components">the sound component to be added to this. 追加するサウンドコンポーネント</param>
    void Add(ISoundComponent component);

    /// <summary>
    /// remove the sound component at index. index番目のサウンドコンポーネントを削除するメソッド。
    /// </summary>
    /// <param name="index">the index of the sound component to remove. 削除するサウンドコンポーネントのインデックス</param>
    void RemoveAt(int index);

    /// <summary>
    /// clear sound components in this. チャンネル内のサウンドコンポーネントを空にするメソッド。
    /// </summary>
    void Clear();
}
