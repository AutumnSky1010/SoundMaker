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
    private readonly Dictionary<int, List<Track>> _tracksTimeMap = [];

    /// <summary>
    /// Creates a new track with the specified wave type and start time. <br/>
    /// 指定された波の種類と開始時間で新しいトラックを作成するメソッド。
    /// </summary>
    /// <param name="startMilliSecond">The start time in milliseconds. <br/> 開始時間（ミリ秒）。</param>
    /// <param name="waveType">The type of wave. <br/> 波の種類。</param>
    /// <returns>A new instance of the track. <br/> 新しいトラックのインスタンス。</returns>
    public Track CreateTrack(int startMilliSecond, WaveTypeBase waveType)
    {
        var track = new Track(waveType, Format, Tempo, startMilliSecond);
        InsertTrack(startMilliSecond, track);
        return track;
    }

    /// <summary>
    /// Removes all tracks at the specified start time. <br/>
    /// 指定された開始時間のすべてのトラックを削除するメソッド。
    /// </summary>
    /// <param name="startMilliSecond">The start time in milliseconds. <br/> 開始時間（ミリ秒）。</param>
    /// <returns>True if tracks were removed; otherwise, false. <br/> トラックが削除された場合は true、それ以外の場合は false。</returns>
    public bool RemoveTracksAt(int startMilliSecond)
    {
        return _tracksTimeMap.Remove(startMilliSecond);
    }

    /// <summary>
    /// Removes the specified track. <br/>
    /// 指定されたトラックを削除するメソッド。
    /// </summary>
    /// <param name="track">The track to remove. <br/> 削除するトラック。</param>
    /// <returns>True if the track was removed; otherwise, false. <br/> トラックが削除された場合は true、それ以外の場合は false。</returns>
    public bool RemoveTrack(Track track)
    {
        if (_tracksTimeMap.TryGetValue(track.StartMilliSecond, out var tracks))
        {
            var ok = tracks.Remove(track);
            if (!ok)
            {
                return false;
            }

            if (tracks.Count == 0)
            {
                return _tracksTimeMap.Remove(track.StartMilliSecond);
            }

            return true;
        }

        return false;
    }

    /// <summary>
    /// Gets the list of tracks at the specified start time. <br/>
    /// 指定された開始時間のトラックのリストを取得するメソッド。
    /// </summary>
    /// <param name="startMilliSecond">The start time in milliseconds. <br/> 開始時間（ミリ秒）。</param>
    /// <returns>
    /// A list of tracks. <br/>
    /// トラックのリスト。
    /// If the operation fails, an empty list is returned. <br/>
    /// 失敗時は空リスト。
    /// </returns>

    public List<Track> GetTracks(int startMilliSecond)
    {
        if (_tracksTimeMap.TryGetValue(startMilliSecond, out var tracks))
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
    /// <param name="startMilliSecond">The start time in milliseconds. <br/> 開始時間（ミリ秒）。</param>
    /// <param name="tracks">The list of tracks. <br/> トラックのリスト。</param>
    /// <returns>True if tracks were found; otherwise, false. <br/> トラックが見つかった場合は true、それ以外の場合は false。</returns>
    public bool TryGetTracks(int startMilliSecond, out List<Track> tracks)
    {
        if (_tracksTimeMap.TryGetValue(startMilliSecond, out var foundTracks))
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
    /// <param name="startMilliSecond">The start time in milliseconds. <br/> 開始時間（ミリ秒）。</param>
    /// <param name="track">The track to insert. <br/> 挿入するトラック。</param>
    private void InsertTrack(int startMilliSecond, Track track)
    {
        if (_tracksTimeMap.TryGetValue(startMilliSecond, out var tracks))
        {
            tracks.Add(track);
        }
        else
        {
            _tracksTimeMap[startMilliSecond] = [track];
        }
    }

    /// <summary>
    /// Moves a track to a new start time. <br/>
    /// トラックを新しい開始時間に移動するメソッド。
    /// </summary>
    /// <param name="track">The track to move. <br/> 移動するトラック。</param>
    /// <param name="newStartMilliSecond">The new start time in milliseconds. <br/> 新しい開始時間（ミリ秒）。</param>
    /// <returns>True if the track was moved; otherwise, false. <br/> トラックが移動された場合は true、それ以外の場合は false。</returns>
    public bool MoveTrack(Track track, int newStartMilliSecond)
    {
        if (RemoveTrack(track))
        {
            InsertTrack(newStartMilliSecond, track);
            track.StartMilliSecond = newStartMilliSecond;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Creates a copy of the specified track at a new start time. <br/>
    /// 指定されたトラックのコピーを新しい開始時間に作成するメソッド。
    /// </summary>
    /// <param name="sourceTrack">The track to copy. <br/> コピーするトラック。</param>
    /// <param name="newStartMilliSecond">The new start time in milliseconds. <br/> 新しい開始時間（ミリ秒）。</param>
    /// <returns>A new instance of the copied track. <br/> コピーされたトラックの新しいインスタンス。</returns>
    public Track CopyTrack(Track sourceTrack, int newStartMilliSecond)
    {
        var newTrack = sourceTrack.Clone();
        newTrack.StartMilliSecond = newStartMilliSecond;
        InsertTrack(newStartMilliSecond, newTrack);
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
    /// <param name="samplingFrequency">The sampling frequency. <br/> サンプリング周波数。</param>
    /// <returns>A monaural wave. <br/> モノラル波。</returns>
    public MonauralWave GenerateMonauralWave(SamplingFrequencyType samplingFrequency)
    {
        if (_tracksTimeMap.Count == 0)
        {
            return new([]);
        }

        int maxStartTime = 0;
        int maxDuration = 0;

        foreach (var pair in _tracksTimeMap)
        {
            maxStartTime = pair.Key > maxStartTime ? pair.Key : maxStartTime;
            foreach (var track in pair.Value)
            {
                int duration = track.WaveArrayLength;
                if (duration > maxDuration)
                {
                    maxDuration = duration;
                }
            }
        }

        var totalDuration = maxStartTime + maxDuration;

        var result = new int[totalDuration * (int)samplingFrequency / 1000];

        foreach (var (startMilliSecond, tracks) in _tracksTimeMap)
        {
            int startSample = startMilliSecond * (int)samplingFrequency / 1000;

            foreach (var track in tracks)
            {
                var trackWave = track.GenerateWave();
                for (int i = 0; i < trackWave.Length && (startSample + i) < result.Length; i++)
                {
                    result[startSample + i] += trackWave[i];
                }
            }
        }

        var normalized = NormalizeAndClamp(result);
        return new(normalized);
    }

    /// <summary>
    /// Normalizes and clamps the wave data. <br/>
    /// 波形データを正規化してクランプするメソッド。
    /// </summary>
    /// <param name="wave">The wave data. <br/> 波形データ。</param>
    /// <returns>The normalized and clamped wave data. <br/> 正規化およびクランプされた波形データ。</returns>
    private short[] NormalizeAndClamp(int[] wave)
    {
        const int MaxValue = short.MaxValue;
        const int MinValue = short.MinValue;

        int maxAmplitude = wave.Max(Math.Abs);
        float scaleFactor = 1.0f;

        if (maxAmplitude > MaxValue)
        {
            scaleFactor = (float)MaxValue / maxAmplitude;
        }

        return wave.Select(sample =>
        {
            int scaledSample = (int)(sample * scaleFactor);
            return (short)Math.Clamp(scaledSample, MinValue, MaxValue);
        }).ToArray();
    }
}
