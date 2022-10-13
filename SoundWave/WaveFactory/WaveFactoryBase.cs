using SoundMaker.WaveFile;
using SoundMaker.SoundWave.Score;

namespace SoundMaker.SoundWave;
public abstract class WaveFactoryBase : IWaveFactory
{
	protected List<ISoundComponent> _soundComponents { get; } = new List<ISoundComponent>();

	public double Second { get; protected set; } = 0;

	public WaveFactoryBase(int equalTemperamentCount)
	{
		this._soundComponents = new List<ISoundComponent>(equalTemperamentCount);
	}

	public WaveFactoryBase() { }

	public void Add(ISoundComponent equalTemperament)
	{
		this.Second += equalTemperament.Second;
		this._soundComponents.Add(equalTemperament);
	}
	
	public void Clear() => this._soundComponents.Clear();
	
	public void RemoveAt(int index) => this._soundComponents.RemoveAt(index);

    public abstract MonauralWave CreateMonaural(FormatChunk format);
}
