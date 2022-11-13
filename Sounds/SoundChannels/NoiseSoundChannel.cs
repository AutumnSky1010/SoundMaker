using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundMaker.Sounds.SoundChannels;
public class NoiseSoundChannel : SoundChannelBase
{
    public NoiseSoundChannel(int tempo, SoundFormat format, PanType panType, int componentsCount) : base(tempo, format, panType, componentsCount)
    {
    }

    public NoiseSoundChannel(int tempo, SoundFormat format, PanType panType) : base(tempo, format, panType)
    {
    }

    public override ushort[] CreateWave()
    {
        var result = new List<ushort>();
        foreach (var soundComponent in this.SoundComponents)
        {
            result.AddRange(soundComponent.GetNoiseWave(this.Format, this.Tempo));
        }
        return result.ToArray();
    }
}
