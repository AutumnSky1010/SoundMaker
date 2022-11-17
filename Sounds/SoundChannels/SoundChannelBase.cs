
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
    /// <param name="panType">pan of the sound. 左右どちらから音が出るか</param>
    /// <param name="componentsCount">count of sound components. サウンドコンポーネントの個数</param>
    public SoundChannelBase(int tempo, SoundFormat format, PanType panType, int componentsCount)
    {
        this.SoundComponents = new List<ISoundComponent>(componentsCount);
        this.Format = format;
        this.PanType = panType;
        this.Tempo = tempo;
    }

    /// <summary>
    /// constructor. コンストラクタ。
    /// </summary>
    /// <param name="tempo">quarter note/rest per minute. 一分間の四分音符・休符の数</param>
    /// <param name="format">format of the sound.音のフォーマット</param>
    /// <param name="panType">pan of the sound. 左右どちらから音が出るか</param>
    public SoundChannelBase(int tempo, SoundFormat format, PanType panType)
    {
        this.PanType = panType;
        this.Format = format;
        this.Tempo = tempo;
    }

    /// <summary>
    /// サウンドコンポーネントのリスト
    /// </summary>
    protected List<ISoundComponent> SoundComponents { get; } = new List<ISoundComponent>();

    public SoundFormat Format { get; }

    public PanType PanType { get; }

    public int ComponentCount => this.SoundComponents.Count;

    public int Tempo { get; }

    public int WaveArrayLength { get; private set; }

    public ISoundComponent this[int index] => this.SoundComponents[index];

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

    public void RemoveAt(int index)
    {
        if (this.SoundComponents.Count <= index)
        {
            return;
        }
        var component = this.SoundComponents[index];
        this.WaveArrayLength -= component.GetWaveArrayLength(this.Format, this.Tempo);
        this.SoundComponents.Remove(component);
    }

    public abstract ushort[] CreateWave();
}
