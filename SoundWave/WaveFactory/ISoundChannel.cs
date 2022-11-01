using SoundMaker.WaveFile;
using SoundMaker.SoundWave.Score;
using SoundMaker.SoundWave.WaveFactory;

namespace SoundMaker.SoundWave;
public interface ISoundChannel
{
    ISoundComponent this[int index] { get; }

    public int ComponentCount { get; }

    public int WaveArrayLength { get; }

    public int Tempo { get; }

    FormatChunk Format { get; }

    PanType PanType { get; }

    ushort[] CreateWave();

    void Add(ISoundComponent components);

	void RemoveAt(int index);

	void Clear();
}
