using SoundMaker.Sounds.Score;
using SoundMaker.Sounds.WaveTypes;

namespace SoundMaker.Sounds;

/// <summary>
/// Represents a track with a specific wave type. <br/>
/// 特定の波形タイプを持つトラックを表すクラス。
/// </summary>
public class Track
{
    private List<ISoundComponent> _soundComponents = [];

    private Dictionary<ISoundComponent, int> _waveArrayLengthPair = [];

    private readonly SoundFormat _format;

    private readonly int _tempo;

    /// <summary>
    /// Initializes a new instance of the Track class. <br/>
    /// Track クラスの新しいインスタンスを初期化するコンストラクタ。
    /// </summary>
    /// <param name="waveType">The wave type. <br/> 波形タイプ。</param>
    /// <param name="format">The sound format. <br/> サウンドフォーマット。</param>
    /// <param name="tempo">The tempo. <br/> テンポ。</param>
    /// <param name="startIndex">The start time in index. <br/> 開始時間（インデクス）。</param>
    internal Track(WaveTypeBase waveType, SoundFormat format, int tempo, int startIndex)
    {
        WaveType = waveType;
        _format = format;
        _tempo = tempo;
        StartIndex = startIndex;
    }


    /// <summary>
    /// Sound components<br/>
    /// サウンドコンポーネント
    /// </summary>
    public IReadOnlyList<ISoundComponent> SoundComponents => _soundComponents;

    private double _pan = 0;
    /// <summary>
    /// 左右の音量バランスを取得または設定するプロパティ。<br/>
    /// -1.0が左、1.0が右側。<br/>
    /// Gets or sets the left-right audio balance. <br/>
    /// Takes values from -1.0 (left) to 1.0 (right). 
    /// </summary>
    public double Pan
    {
        get => _pan;
        set
        {
            value = value > 1.0 ? 1.0 : value;
            value = value < -1.0 ? -1.0 : value;

            _pan = value;
        }
    }

    internal int EndIndex { get; private set; }
    private int _startIndex;
    /// <summary>
    /// Gets or sets the start time in index. <br/>
    /// 開始時間（インデクス）を取得または設定するプロパティ。
    /// </summary>
    internal int StartIndex
    {
        get => _startIndex;
        set
        {
            // 負の数は許可しない
            if (value < 0)
            {
                value = 0;
            }

            // 開始インデクスが変わると終了時のインデクスも変わる
            _startIndex = value;
            if (WaveArrayLength == 0)
            {
                EndIndex = StartIndex;
            }
            else
            {
                EndIndex = StartIndex + WaveArrayLength - 1;
            }
        }
    }

    private int _waveArrayLength;
    /// <summary>
    /// Gets the length of the wave array. <br/>
    /// 波形配列の長さを取得するプロパティ。
    /// </summary>
    public int WaveArrayLength
    {
        get => _waveArrayLength;
        set
        {
            _waveArrayLength = value;

            // 配列の長さが変わると終了時インデクスが変わる
            if (WaveArrayLength == 0)
            {
                EndIndex = StartIndex;
            }
            else
            {
                EndIndex = StartIndex + WaveArrayLength - 1;
            }
        }
    }

    /// <summary>
    /// Count of sound components. <br/>
    /// サウンドコンポーネントの数
    /// </summary>
    public int Count => _soundComponents.Count;

    /// <summary>
    /// Gets or sets the wave type. <br/>
    /// 波形タイプを取得または設定するプロパティ。
    /// </summary>
    public WaveTypeBase WaveType { get; set; }

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
    /// 指定した範囲の波形を生成する。
    /// </summary>
    /// <param name="startIndex">開始インデックス</param>
    /// <param name="endIndex">終了インデックス</param>
    /// <returns>指定範囲の波形データ</returns>
    public short[] GeneratePartialWave(int startIndex, int endIndex)
    {
        // 入力検証
        if (startIndex < 0 || startIndex > endIndex)
        {
            return [];
        }

        // 生成する波形の長さを計算する
        var expectedLength = endIndex - startIndex + 1;

        // 現在探索中のコンポーネント開始インデクス
        int currentComponentIndex = StartIndex;

        // 生成した波形データを格納するリスト
        var result = new List<short>();

        // 余分に生成した波形の長さ(データの先頭側)
        var unnecessaryLengthFirst = 0;
        foreach (var soundComponent in _soundComponents)
        {
            // コンポーネントごとの波形長を取得
            int waveArrayLength = _waveArrayLengthPair.TryGetValue(soundComponent, out var value)
                ? value
                : soundComponent.GetWaveArrayLength(_format, _tempo);

            int nextComponentIndex = currentComponentIndex + waveArrayLength;

            // コンポーネントが必要な範囲に含まれない場合の処理
            if (startIndex >= nextComponentIndex)
            {
                // `startIndex` が現在のコンポーネントの終了位置より後の場合、次のコンポーネントへ進む
                currentComponentIndex = nextComponentIndex;
                continue;
            }

            if (endIndex < currentComponentIndex)
            {
                // `endIndex`が現在のコンポーネントの開始位置より前の場合、ブレークする
                break;
            }

            // 以下、コンポーネントが必要な範囲に含まれる場合の処理
            var wave = soundComponent.GenerateWave(_format, _tempo, WaveType);
            if (result.Count == 0)
            {
                unnecessaryLengthFirst = Math.Max(startIndex - currentComponentIndex, 0);

                // 開始インデクスより先に波形がある場合は０で埋めて、波形の開始位置をあわせる
                if (currentComponentIndex > startIndex)
                {
                    result.AddRange(Enumerable.Repeat<short>(0, currentComponentIndex - startIndex));
                }
            }
            // 生成した波形を追加
            result.AddRange(wave);

            // 次のコンポーネントへ進む
            currentComponentIndex = nextComponentIndex;

            // 必要な範囲の波形をすべて取得したら終了
            if (result.Count - unnecessaryLengthFirst >= expectedLength)
            {
                break;
            }
        }

        // 最初の不要な長さをスキップする
        var skipped = result.Skip(unnecessaryLengthFirst).ToArray();
        if (skipped.Length <= expectedLength)
        {
            return skipped;
        }

        // 波形後ろ側の余分な長さをスキップする
        return skipped.Take(expectedLength).ToArray();
    }

    /// <summary>
    /// Adds a sound component to the track. <br/>
    /// トラックにサウンドコンポーネントを追加するメソッド。
    /// </summary>
    /// <param name="component">The sound component to add. <br/> 追加するサウンドコンポーネント。</param>
    public void Add(ISoundComponent component)
    {
        var componentLength = component.GetWaveArrayLength(_format, _tempo);
        WaveArrayLength += componentLength;
        _soundComponents.Add(component);
        _waveArrayLengthPair.TryAdd(component, componentLength);
    }

    /// <summary>
    /// Inserts a sound component at the specified index. <br/>
    /// 指定されたインデックスにサウンドコンポーネントを挿入するメソッド。
    /// </summary>
    /// <param name="index">The zero-based index at which the component should be inserted. <br/>
    /// コンポーネントを挿入するゼロベースのインデックス。</param>
    /// <param name="component">The sound component to insert. <br/>
    /// 挿入するサウンドコンポーネント。</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the index is out of range. <br/>
    /// インデックスが範囲外の場合にスローされる例外。</exception>

    public void Insert(int index, ISoundComponent component)
    {
        if (IsOutOfRange(index))
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        _soundComponents.Insert(index, component);
        var componentLength = component.GetWaveArrayLength(_format, _tempo);
        WaveArrayLength += componentLength;
        _waveArrayLengthPair.TryAdd(component, componentLength);
    }

    /// <summary>
    /// Removes a sound component at the specified index. <br/>
    /// 指定されたインデックスのサウンドコンポーネントを削除するメソッド。
    /// </summary>
    /// <param name="index">The index of the sound component to remove. <br/> 削除するサウンドコンポーネントのインデックス。</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the index is out of range. <br/> インデックスが範囲外の場合にスローされる例外。</exception>
    public void RemoveAt(int index)
    {
        if (IsOutOfRange(index))
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        var targetComponent = _soundComponents[index];
        _soundComponents.Remove(targetComponent);
        if (_waveArrayLengthPair.TryGetValue(targetComponent, out var componentLength))
        {
            _waveArrayLengthPair.Remove(targetComponent);
            WaveArrayLength -= componentLength;
        }
        else
        {
            WaveArrayLength -= targetComponent.GetWaveArrayLength(_format, _tempo);
        }
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
            if (_waveArrayLengthPair.TryGetValue(component, out var componentLength))
            {
                _waveArrayLengthPair.Remove(component);
                WaveArrayLength -= componentLength;
            }
            else
            {
                WaveArrayLength -= component.GetWaveArrayLength(_format, _tempo);
            }
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
        _waveArrayLengthPair.Clear();
    }

    /// <summary>
    /// Imports a collection of sound components into the track. <br/>
    /// トラックにサウンドコンポーネントのコレクションをインポートするメソッド。
    /// </summary>
    /// <param name="components">The collection of sound components to import. <br/> インポートするサウンドコンポーネントのコレクション。</param>
    public void Import(IEnumerable<ISoundComponent> components)
    {
        _soundComponents = new List<ISoundComponent>(components);
        var sum = 0;
        foreach (var component in components)
        {
            var componentLength = component.GetWaveArrayLength(_format, _tempo);
            _waveArrayLengthPair.TryAdd(component, componentLength);
            sum += componentLength;
        }
        WaveArrayLength = sum;
    }

    /// <summary>
    /// Creates a clone of the track. <br/>
    /// トラックのクローンを作成するメソッド。
    /// </summary>
    /// <returns>A new instance of the track with the same properties. <br/> 同じプロパティを持つトラックの新しいインスタンス。</returns>
    internal Track Clone()
    {
        var copy = new Track(WaveType.Clone(), _format, _tempo, StartIndex);
        copy.Import(_soundComponents.Select(component => component.Clone()));
        return copy;
    }

    private bool IsOutOfRange(int index)
    {
        return index < 0 || index >= _soundComponents.Count;
    }
}
