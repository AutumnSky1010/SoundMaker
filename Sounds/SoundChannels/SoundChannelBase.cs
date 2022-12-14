
using SoundMaker.Sounds.Score;

namespace SoundMaker.Sounds.SoundChannels;
/// <summary>
/// sound channel base. サウンドチャンネルの抽象基底クラス。
/// </summary>
public abstract class SoundChannelBase : ISoundChannel
{

    /// <summary>
    /// constructor. コンストラクタ。
    /// </summary>
    /// <param name="tempo">quarter note/rest per minute. 一分間の四分音符・休符の数</param>
    /// <param name="format">format of the sound.音のフォーマット</param>
    /// <param name="panType">direction of hearing. 左右どちらから音が出るか</param>
    /// <param name="capacity">the total number of sound components the internal data structure can hold without resizing. 内部データ構造がリサイズされずに保持できるサウンドコンポーネントの総数。</param>
    /// <exception cref="ArgumentOutOfRangeException">Tempo must be non-negative and greater than 0.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Capacity must be non-negative.</exception>
    public SoundChannelBase(int tempo, SoundFormat format, PanType panType, int capacity)
    {
        if (capacity < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(capacity), "'capacity' must be non-negative.");
        }
        this.SoundComponents = new List<ISoundComponent>(capacity);
        this.Format = format;
        this.PanType = panType;
        if (tempo <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(tempo), "'tempo' must be non-negative and greater than 0.");
        }
        this.Tempo = tempo;
    }

    /// <summary>
    /// constructor. コンストラクタ。
    /// </summary>
    /// <param name="tempo">quarter note/rest per minute. 一分間の四分音符・休符の数</param>
    /// <param name="format">format of the sound.音のフォーマット</param>
    /// <param name="panType">pan of the sound. 左右どちらから音が出るか</param>
    /// <exception cref="ArgumentOutOfRangeException">Tempo must be non-negative and greater than 0.</exception>
    public SoundChannelBase(int tempo, SoundFormat format, PanType panType)
    {
        this.PanType = panType;
        this.Format = format;
        if (tempo <= 0)
        {
            throw new ArgumentOutOfRangeException("'tempo' must be non-negative and greater than 0.");
        }
        this.Tempo = tempo;
    }

    /// <summary>
    /// サウンドコンポーネントのリスト
    /// </summary>
    protected List<ISoundComponent> SoundComponents { get; } = new List<ISoundComponent>();

    public SoundFormat Format { get; }

    public PanType PanType { get; }

    public int Capacity => this.SoundComponents.Capacity;

    public int ComponentCount => this.SoundComponents.Count;

    public int Tempo { get; }

    public int WaveArrayLength { get; private set; }

    /// <summary>
    /// get sound component at index. index番目のサウンドコンポーネントを取得する
    /// </summary>
    /// <param name="index">index. 何番目かを表す整数</param>
    /// <returns>sound component.サウンドコンポーネント</returns>
    /// <exception cref="IndexOutOfRangeException">index is less than 0 or index is equal to or greater than ComponentCount.</exception>
    public ISoundComponent this[int index]
    {
        get
        {
            if (index < 0 || index >= this.SoundComponents.Count)
            {
                throw new IndexOutOfRangeException("index is less than 0 or index is equal to or greater than ComponentCount.");
            }
            return this.SoundComponents[index];
        }
    }

    public void Add(ISoundComponent component)
    {
        this.SoundComponents.Add(component);
        this.WaveArrayLength += component.GetWaveArrayLength(this.Format, this.Tempo);
    }

    public void Clear()
    {
        this.SoundComponents.Clear();
        this.WaveArrayLength = 0;
    }

    /// <summary>
    /// remove the sound component at index. index番目のサウンドコンポーネントを削除するメソッド。
    /// </summary>
    /// <param name="index">the index of the sound component to remove. 削除するサウンドコンポーネントのインデックス</param>
    /// <exception cref="ArgumentOutOfRangeException">index is less than 0 or index is equal to or greater than ComponentCount.</exception>
    public void RemoveAt(int index)
    {
        if (this.SoundComponents.Count <= index || index < 0)
        {
            throw new ArgumentOutOfRangeException("index is less than 0 or index is equal to or greater than ComponentCount.");
        }
        var component = this.SoundComponents[index];
        this.WaveArrayLength -= component.GetWaveArrayLength(this.Format, this.Tempo);
        this.SoundComponents.Remove(component);
    }

    public abstract ushort[] GenerateWave();
}
