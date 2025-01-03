using SoundMaker.Sounds.Score;
using System.Collections;

namespace SoundMaker.Sounds.SoundChannels;

/// <summary>
/// Sound channel base. <br/>サウンドチャンネルの抽象基底クラス。
/// </summary>
public abstract class SoundChannelBase : ISoundChannel
{
    /// <summary>
    /// Constructor. <br/>コンストラクタ。
    /// </summary>
    /// <param name="tempo">Quarter note/rest per minute. <br/>一分間の四分音符・休符の数</param>
    /// <param name="format">Format of the sound. <br/>音のフォーマット</param>
    /// <param name="panType">Direction of hearing. <br/>左右どちらから音が出るか</param>
    /// <param name="capacity">The total number of sound components the internal data structure can hold without resizing. <br/>内部データ構造がリサイズされずに保持できるサウンドコンポーネントの総数。</param>
    /// <exception cref="ArgumentOutOfRangeException">Tempo must be non-negative and greater than 0.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Capacity must be non-negative.</exception>
    public SoundChannelBase(int tempo, SoundFormat format, PanType panType, int capacity)
    {
        if (capacity < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(capacity), "'capacity' must be non-negative.");
        }
        SoundComponents = new List<ISoundComponent>(capacity);
        Format = format;
        PanType = panType;
        if (tempo <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(tempo), "'tempo' must be non-negative and greater than 0.");
        }
        Tempo = tempo;
    }

    /// <summary>
    /// Constructor. <br/>コンストラクタ。
    /// </summary>
    /// <param name="tempo">Quarter note/rest per minute. <br/>一分間の四分音符・休符の数</param>
    /// <param name="format">Format of the sound. <br/>音のフォーマット</param>
    /// <param name="panType">Pan of the sound. <br/>左右どちらから音が出るか</param>
    /// <exception cref="ArgumentOutOfRangeException">Tempo must be non-negative and greater than 0.</exception>
    public SoundChannelBase(int tempo, SoundFormat format, PanType panType)
    {
        PanType = panType;
        Format = format;
        if (tempo <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(tempo), "'tempo' must be non-negative and greater than 0.");
        }
        Tempo = tempo;
    }

    /// <summary>
    /// List of sound components. <br/>サウンドコンポーネントのリスト
    /// </summary>
    protected List<ISoundComponent> SoundComponents { get; private set; } = [];

    public SoundFormat Format { get; }

    public PanType PanType { get; }

    public int Capacity => SoundComponents.Capacity;

    public int ComponentCount => SoundComponents.Count;

    public int Tempo { get; }

    public int WaveArrayLength { get; private set; }

    /// <summary>
    /// Get sound component at index. <br/>index番目のサウンドコンポーネントを取得する
    /// </summary>
    /// <param name="index">Index. <br/>何番目かを表す整数</param>
    /// <returns>Sound component. <br/>サウンドコンポーネント</returns>
    /// <exception cref="IndexOutOfRangeException">Index is less than 0 or index is equal to or greater than ComponentCount.</exception>
    public ISoundComponent this[int index] => index < 0 || index >= SoundComponents.Count
                ? throw new IndexOutOfRangeException("Index is less than 0 or index is equal to or greater than ComponentCount.")
                : SoundComponents[index];

    public void Add(ISoundComponent component)
    {
        SoundComponents.Add(component);
        WaveArrayLength += component.GetWaveArrayLength(Format, Tempo);
    }

    public void Clear()
    {
        SoundComponents.Clear();
        WaveArrayLength = 0;
    }

    /// <summary>
    /// Remove the sound component at index. <br/>index番目のサウンドコンポーネントを削除するメソッド。
    /// </summary>
    /// <param name="index">The index of the sound component to remove. <br/>削除するサウンドコンポーネントのインデックス</param>
    /// <exception cref="ArgumentOutOfRangeException">Index is less than 0 or index is equal to or greater than ComponentCount.</exception>
    public void RemoveAt(int index)
    {
        if (SoundComponents.Count <= index || index < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Index is less than 0 or index is equal to or greater than ComponentCount.");
        }
        var component = SoundComponents[index];
        WaveArrayLength -= component.GetWaveArrayLength(Format, Tempo);
        _ = SoundComponents.Remove(component);
    }

    /// <summary>
    /// Import sound components. <br/>サウンドコンポーネントをインポートする。
    /// </summary>
    /// <param name="components">Sound components. <br/>サウンドコンポーネント</param>
    public void Import(IEnumerable<ISoundComponent> components)
    {
        SoundComponents = new List<ISoundComponent>(components);
        WaveArrayLength = components.Sum(component => component.GetWaveArrayLength(Format, Tempo));
    }

    public abstract short[] GenerateWave();

    protected void FadeInOut(short[] wave)
    {
        var tenMSCount = (int)((double)Format.SamplingFrequency * 0.01);
        if (wave.Length > tenMSCount << 1)
        {
            for (int i = 0; i < tenMSCount; i++)
            {
                double fadeMagnification = (1.0 / tenMSCount) * (i + 1);

                wave[i] = (short)(fadeMagnification * wave[i]);
                wave[wave.Length - i - 1] = (short)(fadeMagnification * wave[wave.Length - i - 1]);
            }
        }
    }

    public IEnumerator<ISoundComponent> GetEnumerator()
    {
        return SoundComponents.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return SoundComponents.GetEnumerator();
    }
}
