using SoundMaker.Sounds.Score;
using SoundMaker.Sounds.WaveTypes;

namespace SoundMaker.Sounds;
public class Track<T> where T : WaveTypeBase
{
    internal Track(T waveType, SoundFormat format, int tempo, int startMilliSecond)
    {
        WaveType = waveType;
        _format = format;
        _tempo = tempo;
        StartMilliSecond = startMilliSecond;
    }

    private List<ISoundComponent> _soundComponents = [];

    private readonly SoundFormat _format;

    private readonly int _tempo;

    internal int StartMilliSecond { get; set; }

    public int WaveArrayLength { get; private set; }

    public T WaveType { get; set; }

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

    public void Add(ISoundComponent component)
    {
        WaveArrayLength += component.GetWaveArrayLength(_format, _tempo);
        _soundComponents.Add(component);
    }

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

    public void Clear()
    {
        WaveArrayLength = 0;
        _soundComponents.Clear();
    }

    public void Import(IEnumerable<ISoundComponent> components)
    {
        _soundComponents = new List<ISoundComponent>(components);
        WaveArrayLength = components.Sum(component => component.GetWaveArrayLength(_format, _tempo));
    }

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
