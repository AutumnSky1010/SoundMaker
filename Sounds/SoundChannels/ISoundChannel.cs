using SoundMaker.Sounds.Score;

namespace SoundMaker.Sounds.SoundChannels;
public interface ISoundChannel
{
    /// <summary>
    /// index番目のサウンドコンポーネントを取得する
    /// </summary>
    /// <param name="index">何番目かを表す整数</param>
    /// <returns>サウンドコンポーネント</returns>
    ISoundComponent this[int index] { get; }

    /// <summary>
    /// サウンドコンポーネントの個数
    /// </summary>
    public int ComponentCount { get; }

    /// <summary>
    /// 音の波形データを表す配列の長さ
    /// </summary>
    public int WaveArrayLength { get; }

    /// <summary>
    /// 一分間の四分音符・休符の数
    /// </summary>
    public int Tempo { get; }

    /// <summary>
    /// 音のフォーマット
    /// </summary>
    SoundFormat Format { get; }

    /// <summary>
    /// ステレオサウンドの場合、左右どちらから音が出るか
    /// </summary>
    PanType PanType { get; }

    /// <summary>
    /// 音の波形データを生成するメソッド。
    /// </summary>
    /// <returns>音の波形データの配列 : unsigned short[]</returns>
    ushort[] CreateWave();

    /// <summary>
    /// サウンドコンポーネントを追加するメソッド。
    /// </summary>
    /// <param name="components">追加するサウンドコンポーネント</param>
    void Add(ISoundComponent components);

    /// <summary>
    /// index番目のサウンドコンポーネントを削除するメソッド。
    /// </summary>
    /// <param name="index">削除するサウンドコンポーネントのインデックス</param>
    void RemoveAt(int index);

    /// <summary>
    /// チャンネル内のサウンドコンポーネントを空にするメソッド。
    /// </summary>
    void Clear();
}
