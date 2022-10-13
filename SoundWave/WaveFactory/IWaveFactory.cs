using SoundMaker.WaveFile;
using SoundMaker.SoundWave.Score;
namespace SoundMaker.SoundWave;
public interface IWaveFactory
{
	MonauralWave CreateMonaural(FormatChunk format);

	void Add(ISoundComponent components);

	void RemoveAt(int index);

	void Clear();
}
