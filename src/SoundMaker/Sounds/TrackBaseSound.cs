using SoundMaker.Sounds.WaveTypes;

namespace SoundMaker.Sounds;

/// <summary>
/// Initializes a new instance of the TrackBaseSound class with the specified format and tempo. <br/>
/// 指定されたフォーマットとテンポでTrackBaseSoundクラスの新しいインスタンスを初期化するメソッド。
/// </summary>
/// <param name="format">The sound format to be used. <br/>
/// 使用するサウンドフォーマット。
/// </param>
/// <param name="tempo">The tempo of the track. <br/>
/// トラックのテンポ。
/// </param>
public class TrackBaseSound(SoundFormat format, int tempo)
{
    /// <summary>
    /// Gets the tempo value. <br/>
    /// テンポの値を取得するプロパティ。
    /// </summary>
    public int Tempo { get; } = tempo;

    /// <summary>
    /// Gets the sound format. <br/>
    /// サウンドフォーマットを取得するプロパティ。
    /// </summary>
    public SoundFormat Format { get; } = format;

    /// <summary>
    /// トラックを管理する辞書<br/>
    /// 開始時間(ミリ秒)とトラック(複数)のペア
    /// </summary>
    private Dictionary<int, List<Track>> _tracksTimeMap = [];

    /// <summary>
    /// Creates a new track with the specified wave type and start time. <br/>
    /// 指定された波の種類と開始時間で新しいトラックを作成するメソッド。
    /// </summary>
    /// <param name="startIndex">The start time in index. <br/> 開始時間（インデクス）。</param>
    /// <param name="waveType">The type of wave. <br/> 波の種類。</param>
    /// <returns>A new instance of the track. <br/> 新しいトラックのインスタンス。</returns>
    public Track CreateTrack(int startIndex, WaveTypeBase waveType)
    {
        var track = new Track(waveType, Format, Tempo, startIndex);
        InsertTrack(startIndex, track);
        return track;
    }

    /// <summary>
    /// Removes all tracks at the specified start time. <br/>
    /// 指定された開始時間のすべてのトラックを削除するメソッド。
    /// </summary>
    /// <param name="startIndex">The start time in index. <br/> 開始時間（インデクス）。</param>
    /// <returns>True if tracks were removed; otherwise, false. <br/> トラックが削除された場合は true、それ以外の場合は false。</returns>
    public bool RemoveTracksAt(int startIndex)
    {
        return _tracksTimeMap.Remove(startIndex);
    }

    /// <summary>
    /// Removes the specified track. <br/>
    /// 指定されたトラックを削除するメソッド。
    /// </summary>
    /// <param name="track">The track to remove. <br/> 削除するトラック。</param>
    /// <returns>True if the track was removed; otherwise, false. <br/> トラックが削除された場合は true、それ以外の場合は false。</returns>
    public bool RemoveTrack(Track track)
    {
        if (_tracksTimeMap.TryGetValue(track.StartIndex, out var tracks))
        {
            var ok = tracks.Remove(track);
            if (!ok)
            {
                return false;
            }

            if (tracks.Count == 0)
            {
                return _tracksTimeMap.Remove(track.StartIndex);
            }

            return true;
        }

        return false;
    }

    /// <summary>
    /// Gets the list of tracks at the specified start time. <br/>
    /// 指定された開始時間のトラックのリストを取得するメソッド。
    /// </summary>
    /// <param name="startIndex">The start time in index. <br/> 開始時間（インデクス）。</param>
    /// <returns>
    /// A list of tracks. <br/>
    /// トラックのリスト。
    /// If the operation fails, an empty list is returned. <br/>
    /// 失敗時は空リスト。
    /// </returns>

    public List<Track> GetTracks(int startIndex)
    {
        if (_tracksTimeMap.TryGetValue(startIndex, out var tracks))
        {
            return tracks;
        }
        else
        {
            return [];
        }
    }

    /// <summary>
    /// Retrieves all tracks. <br/>
    /// すべてのトラックを取得するメソッド。
    /// </summary>
    /// <returns>An enumerable collection of tracks. <br/>
    /// トラックの列挙可能なコレクションを返します。
    /// </returns>
    public IEnumerable<Track> GetAllTracks()
    {
        return _tracksTimeMap.SelectMany(pair => pair.Value);
    }

    /// <summary>
    /// Tries to get the list of tracks at the specified start time. <br/>
    /// 指定された開始時間のトラックのリストを取得しようとするメソッド。
    /// </summary>
    /// <param name="startIndex">The start time in index. <br/> 開始時間（インデクス）。</param>
    /// <param name="tracks">The list of tracks. <br/> トラックのリスト。</param>
    /// <returns>True if tracks were found; otherwise, false. <br/> トラックが見つかった場合は true、それ以外の場合は false。</returns>
    public bool TryGetTracks(int startIndex, out List<Track> tracks)
    {
        if (_tracksTimeMap.TryGetValue(startIndex, out var foundTracks))
        {
            tracks = foundTracks;
            return true;
        }
        else
        {
            tracks = [];
            return false;
        }
    }

    /// <summary>
    /// Inserts a track at the specified start time. <br/>
    /// 指定された開始時間にトラックを挿入するメソッド。
    /// </summary>
    /// <param name="startIndex">The start time in index. <br/> 開始時間（インデクス）。</param>
    /// <param name="track">The track to insert. <br/> 挿入するトラック。</param>
    private void InsertTrack(int startIndex, Track track)
    {
        if (_tracksTimeMap.TryGetValue(startIndex, out var tracks))
        {
            tracks.Add(track);
        }
        else
        {
            _tracksTimeMap[startIndex] = [track];
        }
    }

    /// <summary>
    /// Moves a track to a new start time. <br/>
    /// トラックを新しい開始時間に移動するメソッド。
    /// </summary>
    /// <param name="track">The track to move. <br/> 移動するトラック。</param>
    /// <param name="newStartIndex">The new start time in index. <br/> 新しい開始時間（インデクス）。</param>
    /// <returns>True if the track was moved; otherwise, false. <br/> トラックが移動された場合は true、それ以外の場合は false。</returns>
    public bool MoveTrack(Track track, int newStartIndex)
    {
        if (RemoveTrack(track))
        {
            InsertTrack(newStartIndex, track);
            track.StartIndex = newStartIndex;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Creates a copy of the specified track at a new start time. <br/>
    /// 指定されたトラックのコピーを新しい開始時間に作成するメソッド。
    /// </summary>
    /// <param name="sourceTrack">The track to copy. <br/> コピーするトラック。</param>
    /// <param name="newStartIndex">The new start time in index. <br/> 新しい開始時間（インデクス）。</param>
    /// <returns>A new instance of the copied track. <br/> コピーされたトラックの新しいインスタンス。</returns>
    public Track CopyTrack(Track sourceTrack, int newStartIndex)
    {
        var newTrack = sourceTrack.Clone();
        newTrack.StartIndex = newStartIndex;
        InsertTrack(newStartIndex, newTrack);
        return newTrack;
    }

    /// <summary>
    /// Clears all tracks. <br/>
    /// すべてのトラックをクリアするメソッド。
    /// </summary>
    public void Clear()
    {
        _tracksTimeMap.Clear();
    }

    /// <summary>
    /// Generates a monaural wave from the tracks. <br/>
    /// トラックからモノラル波を生成するメソッド。
    /// </summary>
    /// <returns>A monaural wave. <br/> モノラル波。</returns>
    public MonauralWave GenerateMonauralWave()
    {
        if (_tracksTimeMap.Count == 0)
        {
            return new([]);
        }

        // 最大の終了時インデクスを取得する
        var maxEndIndex = _tracksTimeMap
            .SelectMany(pair => pair.Value)
            .Max(track => track.EndIndex);

        var wave = new double[maxEndIndex + 1];

        foreach (var (_, tracks) in _tracksTimeMap)
        {
            foreach (var track in tracks)
            {
                if (track.Count == 0)
                {
                    continue;
                }

                var trackWave = track.GenerateWave();
                for (int i = track.StartIndex; i <= track.EndIndex; i++)
                {
                    wave[i] += trackWave[i - track.StartIndex];
                }
            }
        }

        var normalizedRight = NormalizeAndClamp(wave);
        return new(normalizedRight);
    }

    /// <summary>
    /// Generates a stereo wave from the tracks. <br/>
    /// トラックからステレオ波を生成するメソッド。
    /// </summary>
    /// <returns>A stereo wave. <br/> ステレオ波。</returns>
    public StereoWave GenerateStereoWave()
    {
        if (_tracksTimeMap.Count == 0)
        {
            return new([], []);
        }

        // 最大の終了時インデクスを取得する
        var maxEndIndex = _tracksTimeMap
            .SelectMany(pair => pair.Value)
            .Max(track => track.EndIndex);

        var right = new double[maxEndIndex + 1];
        var left = new double[maxEndIndex + 1];

        foreach (var (_, tracks) in _tracksTimeMap)
        {
            foreach (var track in tracks)
            {
                if (track.Count == 0)
                {
                    continue;
                }

                var trackWave = track.GenerateWave();
                var pan = (track.Pan + 1) / 2.0f;
                for (int i = track.StartIndex; i <= track.EndIndex; i++)
                {
                    left[i] += trackWave[i - track.StartIndex] * pan;
                    right[i] += trackWave[i - track.StartIndex] * (1 - pan);
                }
            }
        }

        var normalizedRight = NormalizeAndClamp(right);
        var normalizedLeft = NormalizeAndClamp(left);
        return new(normalizedRight, normalizedLeft);
    }

    /// <summary>
    /// Normalizes and clamps the wave data. <br/>
    /// 波形データを正規化してクランプするメソッド。
    /// </summary>
    /// <param name="wave">The wave data. <br/> 波形データ。</param>
    /// <returns>The normalized and clamped wave data. <br/> 正規化およびクランプされた波形データ。</returns>
    private static short[] NormalizeAndClamp(double[] wave)
    {
        const int MaxValue = short.MaxValue;
        const int MinValue = short.MinValue;

        var maxAmplitude = wave.Max(Math.Abs);
        var scaleFactor = 1.0;

        if (maxAmplitude > MaxValue)
        {
            scaleFactor = MaxValue / maxAmplitude;
        }

        return wave.Select(sample =>
        {
            var scaledSample = sample * scaleFactor;
            return (short)Math.Clamp(scaledSample, MinValue, MaxValue);
        }).ToArray();
    }

    /// <summary> 
    /// Imports tracks into the internal map based on their start times. <br/> 
    /// トラックを開始時間に基づいて内部のマップにインポートするメソッド。 
    /// </summary> 
    /// <param name="from">The tracks to import. <br/> インポートするトラック。</param>
    public void Import(IEnumerable<Track> from)
    {
        var map = new Dictionary<int, List<Track>>();
        foreach (var track in from)
        {
            if (map.TryGetValue(track.StartIndex, out var tracks))
            {
                tracks.Add(track);
            }
            else
            {
                map.Add(track.StartIndex, [track]);
            }
        }

        _tracksTimeMap = map;
    }
}
