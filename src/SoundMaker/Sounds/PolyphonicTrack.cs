using SoundMaker.Sounds.Score;
using SoundMaker.Sounds.WaveTypes;

namespace SoundMaker.Sounds;

/// <summary>
/// Represents a polyphonic track that can play multiple sounds simultaneously. <br/>
/// 複数の音を同時に再生できるポリフォニックトラックを表すクラス。
/// </summary>
public class PolyphonicTrack : ITrack
{
    private readonly SortedDictionary<int, List<ISoundComponent>> _componentsByPosition = new();

    private readonly Dictionary<ISoundComponent, int> _waveArrayLengthCache = new();

    private readonly SoundFormat _format;

    private readonly int _tempo;

    /// <summary>
    /// Initializes a new instance of the PolyphonicTrack class. <br/>
    /// PolyphonicTrack クラスの新しいインスタンスを初期化するコンストラクタ。
    /// </summary>
    /// <param name="waveType">The wave type. <br/> 波形タイプ。</param>
    /// <param name="format">The sound format. <br/> サウンドフォーマット。</param>
    /// <param name="tempo">The tempo. <br/> テンポ。</param>
    /// <param name="startIndex">The start time in index. <br/> 開始時間（インデクス）。</param>
    internal PolyphonicTrack(WaveTypeBase waveType, SoundFormat format, int tempo, int startIndex)
    {
        WaveType = waveType;
        _format = format;
        _tempo = tempo;
        StartIndex = startIndex;
    }

    /// <inheritdoc/>
    public WaveTypeBase WaveType { get; set; }

    private double _pan = 0;
    /// <inheritdoc/>
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

    private int _startIndex;
    /// <summary>
    /// Gets the start time in index. <br/>
    /// 開始時間（インデクス）を取得するプロパティ。
    /// </summary>
    public int StartIndex
    {
        get => _startIndex;
        internal set
        {
            if (value < 0)
            {
                value = 0;
            }
            _startIndex = value;
        }
    }

    /// <inheritdoc/>
    public int EndIndex
    {
        get
        {
            var length = WaveArrayLength;
            return length == 0 ? StartIndex : StartIndex + length - 1;
        }
    }

    /// <inheritdoc/>
    public int WaveArrayLength
    {
        get
        {
            if (_componentsByPosition.Count == 0)
            {
                return 0;
            }

            int maxEndPosition = 0;
            foreach (var (position, components) in _componentsByPosition)
            {
                foreach (var component in components)
                {
                    int componentLength = GetComponentLength(component);
                    int endPosition = position + componentLength;
                    if (endPosition > maxEndPosition)
                    {
                        maxEndPosition = endPosition;
                    }
                }
            }
            return maxEndPosition;
        }
    }

    /// <inheritdoc/>
    public int Count => _componentsByPosition.Values.Sum(list => list.Count);

    /// <summary>
    /// Adds a sound component at the specified relative position. <br/>
    /// 指定した相対位置にサウンドコンポーネントを追加するメソッド。
    /// </summary>
    /// <param name="relativePosition">The relative position from the track start. Position 0 is the track start. <br/>
    /// トラック開始からの相対位置。位置0はトラック開始位置。</param>
    /// <param name="component">The sound component to add. <br/> 追加するサウンドコンポーネント。</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when position is negative. <br/>
    /// 位置が負の値の場合にスローされる例外。</exception>
    public void AddAt(int relativePosition, ISoundComponent component)
    {
        if (relativePosition < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(relativePosition), "Position cannot be negative.");
        }

        if (_componentsByPosition.TryGetValue(relativePosition, out var components))
        {
            components.Add(component);
        }
        else
        {
            _componentsByPosition[relativePosition] = [component];
        }

        var componentLength = component.GetWaveArrayLength(_format, _tempo);
        _waveArrayLengthCache.TryAdd(component, componentLength);
    }

    /// <summary>
    /// Removes a sound component at the specified relative position. <br/>
    /// 指定した相対位置のサウンドコンポーネントを削除するメソッド。
    /// </summary>
    /// <param name="relativePosition">The relative position. <br/> 相対位置。</param>
    /// <param name="component">The sound component to remove. <br/> 削除するサウンドコンポーネント。</param>
    /// <returns>True if the component was removed; otherwise, false. <br/>
    /// コンポーネントが削除された場合は true、それ以外の場合は false。</returns>
    public bool RemoveAt(int relativePosition, ISoundComponent component)
    {
        if (!_componentsByPosition.TryGetValue(relativePosition, out var components))
        {
            return false;
        }

        var removed = components.Remove(component);
        if (removed)
        {
            _waveArrayLengthCache.Remove(component);
            if (components.Count == 0)
            {
                _componentsByPosition.Remove(relativePosition);
            }
        }
        return removed;
    }

    /// <summary>
    /// Gets all sound components at the specified relative position. <br/>
    /// 指定した相対位置のすべてのサウンドコンポーネントを取得するメソッド。
    /// </summary>
    /// <param name="relativePosition">The relative position. <br/> 相対位置。</param>
    /// <returns>A read-only list of sound components at the position. <br/>
    /// 指定位置のサウンドコンポーネントの読み取り専用リスト。</returns>
    public IReadOnlyList<ISoundComponent> GetComponentsAt(int relativePosition)
    {
        if (_componentsByPosition.TryGetValue(relativePosition, out var components))
        {
            return components;
        }
        return [];
    }

    /// <summary>
    /// Gets all position keys that have sound components. <br/>
    /// サウンドコンポーネントを持つすべての位置キーを取得するメソッド。
    /// </summary>
    /// <returns>An enumerable of position keys. <br/> 位置キーの列挙。</returns>
    public IEnumerable<int> GetPositions() => _componentsByPosition.Keys;

    /// <summary>
    /// Clears all sound components from the track. <br/>
    /// トラックからすべてのサウンドコンポーネントをクリアするメソッド。
    /// </summary>
    public void Clear()
    {
        _componentsByPosition.Clear();
        _waveArrayLengthCache.Clear();
    }

    /// <inheritdoc/>
    public short[] GenerateWave()
    {
        if (_componentsByPosition.Count == 0)
        {
            return [];
        }

        int totalLength = WaveArrayLength;
        var result = new short[totalLength];
        var maxConcurrentSounds = GetMaxConcurrentSounds();

        if (maxConcurrentSounds == 0)
        {
            return result;
        }

        foreach (var (position, components) in _componentsByPosition)
        {
            foreach (var component in components)
            {
                var wave = component.GenerateWave(_format, _tempo, WaveType);

                for (int i = 0; i < wave.Length && (position + i) < totalLength; i++)
                {
                    result[position + i] += (short)(wave[i] / maxConcurrentSounds);
                }
            }
        }

        return result;
    }

    /// <inheritdoc/>
    public short[] GeneratePartialWave(int startIndex, int endIndex)
    {
        if (startIndex < 0 || startIndex > endIndex)
        {
            return [];
        }

        if (_componentsByPosition.Count == 0)
        {
            return [];
        }

        var expectedLength = endIndex - startIndex + 1;
        var result = new short[expectedLength];
        var maxConcurrentSounds = GetMaxConcurrentSounds();

        if (maxConcurrentSounds == 0)
        {
            return result;
        }

        foreach (var (position, components) in _componentsByPosition)
        {
            // 相対座標を絶対座標に変換
            int absolutePosition = StartIndex + position;

            foreach (var component in components)
            {
                int componentLength = GetComponentLength(component);
                int absoluteEndPosition = absolutePosition + componentLength - 1;

                // Skip if component is entirely before or after the requested range
                // 絶対座標同士で比較
                if (absoluteEndPosition < startIndex || absolutePosition > endIndex)
                {
                    continue;
                }

                var wave = component.GenerateWave(_format, _tempo, WaveType);

                // Calculate the overlap range（絶対座標を使用）
                int waveStartOffset = Math.Max(0, startIndex - absolutePosition);
                int resultStartOffset = Math.Max(0, absolutePosition - startIndex);

                for (int i = waveStartOffset; i < wave.Length && (resultStartOffset + i - waveStartOffset) < expectedLength; i++)
                {
                    int resultIndex = resultStartOffset + i - waveStartOffset;
                    result[resultIndex] += (short)(wave[i] / maxConcurrentSounds);
                }
            }
        }

        return result;
    }

    /// <summary>
    /// Creates a clone of the polyphonic track. <br/>
    /// ポリフォニックトラックのクローンを作成するメソッド。
    /// </summary>
    /// <returns>A new instance of the track with the same properties. <br/>
    /// 同じプロパティを持つトラックの新しいインスタンス。</returns>
    public PolyphonicTrack Clone()
    {
        var copy = new PolyphonicTrack(WaveType.Clone(), _format, _tempo, StartIndex)
        {
            Pan = Pan
        };

        foreach (var (position, components) in _componentsByPosition)
        {
            foreach (var component in components)
            {
                copy.AddAt(position, component.Clone());
            }
        }

        return copy;
    }

    /// <inheritdoc/>
    ITrack ITrack.Clone() => Clone();

    /// <summary>
    /// Calculates the maximum number of overlapping sounds at any time. <br/>
    /// 任意の時点で同時に再生されているサウンド数の最大値を計算するメソッド。
    /// </summary>
    /// <returns>The maximum number of overlapping sounds. <br/> 同時に再生されているサウンド数の最大値。</returns>
    private int GetMaxConcurrentSounds()
    {
        if (_componentsByPosition.Count == 0)
        {
            return 0;
        }

        var events = new List<(int time, int type)>();

        foreach (var (position, components) in _componentsByPosition)
        {
            foreach (var component in components)
            {
                int componentLength = GetComponentLength(component);
                events.Add((position, 1));
                events.Add((position + componentLength, -1));
            }
        }

        events.Sort((a, b) => a.time == b.time ? a.type.CompareTo(b.type) : a.time.CompareTo(b.time));

        int maxConcurrentSounds = 0;
        int currentSounds = 0;

        foreach (var (_, type) in events)
        {
            currentSounds += type;
            maxConcurrentSounds = Math.Max(maxConcurrentSounds, currentSounds);
        }

        return maxConcurrentSounds;
    }

    private int GetComponentLength(ISoundComponent component)
    {
        if (_waveArrayLengthCache.TryGetValue(component, out var cachedLength))
        {
            return cachedLength;
        }
        return component.GetWaveArrayLength(_format, _tempo);
    }
}
