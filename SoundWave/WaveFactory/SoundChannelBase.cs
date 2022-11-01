using SoundMaker.WaveFile;
using SoundMaker.SoundWave.Score;
using SoundMaker.SoundWave.WaveFactory;

namespace SoundMaker.SoundWave;
public abstract class SoundChannelBase : ISoundChannel
{
	public SoundChannelBase(int tempo, FormatChunk format, PanType panType, int componentsCount)
	{
		this.SoundComponents = new List<ISoundComponent>(componentsCount);
		this.Format = format;
		this.PanType = panType;
		this.Tempo = tempo;
	}

	public SoundChannelBase(int tempo, FormatChunk format, PanType panType)
	{
		this.PanType = panType;
		this.Format = format;
		this.Tempo = tempo;
	}

    protected List<ISoundComponent> SoundComponents { get; } = new List<ISoundComponent>();

    public FormatChunk Format { get; }

    public PanType PanType { get; }

	public int ComponentCount => this.SoundComponents.Count;

	public int Tempo { get; }

	public int WaveArrayLength { get; private set; }

	public ISoundComponent this[int index] => this.SoundComponents[index];

	public void Add(ISoundComponent component)
	{
		this.SoundComponents.Add(component);
		this.WaveArrayLength += component.GetWaveArrayLength(this.Format, this.Tempo);
	}
	
	public void Clear()
	{
		this.SoundComponents.Clear();
		this.WaveArrayLength = 0;
    }
	
	public void RemoveAt(int index)
	{
		if (this.SoundComponents.Count <= index)
		{
			return;
		}
		var component = this.SoundComponents[index];
		this.WaveArrayLength -= component.GetWaveArrayLength(this.Format, this.Tempo);
		this.SoundComponents.Remove(component);
    }

    public abstract ushort[] CreateWave();
}
