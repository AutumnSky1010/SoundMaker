using SoundMaker.Sounds.Score;
using SoundMaker.Sounds.WaveTypes;

namespace SoundMaker.Sounds;
/// <summary>
/// Represents a track with a specific wave type. <br/>
/// 特定の波形タイプを持つトラックを表すクラス。
/// </summary>
/// <typeparam name="T">The type of wave. <br/>
/// 波形のタイプ。
/// </typeparam>
public class Track<T> where T : WaveTypeBase
{
    private List<ISoundComponent> _soundComponents = [];

    private readonly SoundFormat _format;

    private readonly int _tempo;

    /// <summary>
    /// Initializes a new instance of the Track class. <br/>
    /// Track クラスの新しいインスタンスを初期化するコンストラクタ。
    /// </summary>
    /// <param name="waveType">The wave type. <br/> 波形タイプ。</param>
    /// <param name="format">The sound format. <br/> サウンドフォーマット。</param>
    /// <param name="tempo">The tempo. <br/> テンポ。</param>
    /// <param name="startMilliSecond">The start time in milliseconds. <br/> 開始時間（ミリ秒）。</param>
    internal Track(T waveType, SoundFormat format, int tempo, int startMilliSecond)
    {
        WaveType = waveType;
        _format = format;
        _tempo = tempo;
        StartMilliSecond = startMilliSecond;
    }

    /// <summary>
    /// Gets or sets the start time in milliseconds. <br/>
    /// 開始時間（ミリ秒）を取得または設定するプロパティ。
    /// </summary>
    internal int StartMilliSecond { get; set; }

    /// <summary>
    /// Gets the length of the wave array. <br/>
    /// 波形配列の長さを取得するプロパティ。
    /// </summary>
    public int WaveArrayLength { get; private set; }

    /// <summary>
    /// Gets or sets the wave type. <br/>
    /// 波形タイプを取得または設定するプロパティ。
    /// </summary>
    public T WaveType { get; set; }

    /// <summary>
    /// Generates a wave based on the sound components. <br/>
    /// サウンドコンポーネントに基づいて波形を生成するメソッド。
    /// </summary>
    /// <returns>
    /// An array of shorts representing the generated wave. <br/>
    /// 生成された波形を表すショート型の配列。
    /// </returns>
    public short[] GenerateWave()
    {
        var result = new List<short>();
        foreach (var soundComponent in _soundComponents)
        {
            var wave = soundComponent.GenerateWave(_format, _tempo, WaveType);
            result.AddRange(wave);
        }
        return [.. result];
    }


    /// <summary>
    /// Adds a sound component to the track. <br/>
    /// トラックにサウンドコンポーネントを追加するメソッド。
    /// </summary>
    /// <param name="component">The sound component to add. <br/> 追加するサウンドコンポーネント。</param>
    public void Add(ISoundComponent component)
    {
        WaveArrayLength += component.GetWaveArrayLength(_format, _tempo);
        _soundComponents.Add(component);
    }

    /// <summary>
    /// Removes a sound component at the specified index. <br/>
    /// 指定されたインデックスのサウンドコンポーネントを削除するメソッド。
    /// </summary>
    /// <param name="index">The index of the sound component to remove. <br/> 削除するサウンドコンポーネントのインデックス。</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the index is out of range. <br/> インデックスが範囲外の場合にスローされる例外。</exception>
    public void RemoveAt(int index)
    {
        if (index < 0 || index >= _soundComponents.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }
        var targetComponent = _soundComponents[index];
        _soundComponents.Remove(targetComponent);
        WaveArrayLength -= targetComponent.GetWaveArrayLength(_format, _tempo);
    }

    /// <summary>
    /// Removes a specified sound component. <br/>
    /// 指定されたサウンドコンポーネントを削除するメソッド。
    /// </summary>
    /// <param name="component">The sound component to remove. <br/> 削除するサウンドコンポーネント。</param>
    /// <returns>True if the component was removed; otherwise, false. <br/> コンポーネントが削除された場合は true、それ以外の場合は false。</returns>
    public bool Remove(ISoundComponent component)
    {
        var ok = _soundComponents.Remove(component);
        if (ok)
        {
            WaveArrayLength -= component.GetWaveArrayLength(_format, _tempo);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Clears all sound components from the track. <br/>
    /// トラックからすべてのサウンドコンポーネントをクリアするメソッド。
    /// </summary>
    public void Clear()
    {
        WaveArrayLength = 0;
        _soundComponents.Clear();
    }

    /// <summary>
    /// Imports a collection of sound components into the track. <br/>
    /// トラックにサウンドコンポーネントのコレクションをインポートするメソッド。
    /// </summary>
    /// <param name="components">The collection of sound components to import. <br/> インポートするサウンドコンポーネントのコレクション。</param>
    public void Import(IEnumerable<ISoundComponent> components)
    {
        _soundComponents = new List<ISoundComponent>(components);
        WaveArrayLength = components.Sum(component => component.GetWaveArrayLength(_format, _tempo));
    }

    /// <summary>
    /// Creates a clone of the track. <br/>
    /// トラックのクローンを作成するメソッド。
    /// </summary>
    /// <returns>A new instance of the track with the same properties. <br/> 同じプロパティを持つトラックの新しいインスタンス。</returns>
    internal Track<T> Clone()
    {
        var copy = new Track<T>(WaveType, _format, _tempo, StartMilliSecond)
        {
            WaveArrayLength = WaveArrayLength,
            _soundComponents = _soundComponents.Select(component => component.Clone()).ToList()
        };
        return copy;
    }
}
