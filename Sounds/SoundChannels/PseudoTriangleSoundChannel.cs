using SoundMaker.Sounds.WaveTypes;

namespace SoundMaker.Sounds.SoundChannels;
public class PseudoTriangleSoundChannel : SoundChannelBase
{
    public PseudoTriangleSoundChannel(int tempo, SoundFormat format, PanType panType) : base(tempo, format, panType)
    {
    }

    public PseudoTriangleSoundChannel(int tempo, SoundFormat format, PanType panType, int componentsCount) : base(tempo, format, panType, componentsCount)
    {
    }

    public override ushort[] CreateWave()
    {
        var result = new List<ushort>();
        foreach (var soundComponent in this.SoundComponents)
        {
            result.AddRange(soundComponent.GenerateWave(this.Format, this.Tempo, new PseudoTriangleWave()));
        }
        return result.ToArray();
    }
}
