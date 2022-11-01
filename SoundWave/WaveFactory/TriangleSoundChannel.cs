using SoundMaker.WaveFile;
using SoundMaker.SoundWave.Score;
using SoundMaker.SoundWave.WaveFactory;

namespace SoundMaker.SoundWave;
public class TriangleSoundChannel : SoundChannelBase, ISoundChannel
{
	public TriangleSoundChannel(int tempo, FormatChunk format, PanType panType, int soundComponentCount) : base(tempo, format, panType, soundComponentCount) { }

	public TriangleSoundChannel(int tempo, FormatChunk format, PanType panType) : base(tempo, format, panType) { }

	public override ushort[] CreateWave()
	{
		var result = new List<ushort>();
		foreach (var soundComponent in this.SoundComponents)
		{
			result.AddRange(soundComponent.GetTriangleWave(this.Format, this.Tempo));
		}
		return result.ToArray();
	}
}
