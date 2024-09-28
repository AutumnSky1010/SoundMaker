using SoundMaker.Sounds.WaveTypes;

namespace SoundMaker.Sounds;

public class TrackBaseSound(SoundFormat format, int tempo)
{
    public int Tempo { get; } = tempo;

    public SoundFormat Format { get; } = format;

    /// <summary>
    /// トラックを管理する辞書<br/>
    /// 開始時間(ミリ秒)とトラック(複数)のペア
    /// </summary>
    private readonly Dictionary<int, List<Track<WaveTypeBase>>> _tracksTimeMap = [];

    public Track<T> CreateTrack<T>(int startMilliSecond, T waveType) where T : WaveTypeBase
    {
        var track = new Track<WaveTypeBase>(waveType, Format, Tempo, startMilliSecond);
        InsertTrack(startMilliSecond, track);
        return (Track<T>)(object)track;
    }

    public bool RemoveTracksAt(int startMilliSecond)
    {
        return _tracksTimeMap.Remove(startMilliSecond);
    }

    public bool RemoveTrack(Track<WaveTypeBase> track)
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

    public List<Track<WaveTypeBase>> GetTracks(int startMilliSecond)
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

    public bool TryGetTracks(int startMilliSecond, out List<Track<WaveTypeBase>> tracks)
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

    private void InsertTrack(int startMilliSecond, Track<WaveTypeBase> track)
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

    public bool MoveTrack(Track<WaveTypeBase> track, int newStartMilliSecond)
    {
        if (RemoveTrack(track))
        {
            InsertTrack(newStartMilliSecond, track);
            track.StartMilliSecond = newStartMilliSecond;
            return true;
        }
        return false;
    }

    public Track<T> CopyTrack<T>(Track<T> sourceTrack, int newStartMilliSecond) where T : WaveTypeBase
    {
        var newTrack = sourceTrack.Clone();
        newTrack.StartMilliSecond = newStartMilliSecond;
        InsertTrack(newStartMilliSecond, (Track<WaveTypeBase>)(object)newTrack);
        return newTrack;
    }

    public void Clear()
    {
        _tracksTimeMap.Clear();
    }

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
