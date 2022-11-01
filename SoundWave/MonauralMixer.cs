using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundMaker.SoundWave;
internal class MonauralMixer : MixerBase
{
    public MonauralMixer(IReadOnlyList<ISoundChannel> channels) : base(channels)
    {
    }

    public MonauralWave Mix()
    {
        ushort[] result = Enumerable.Repeat((ushort)0, this.GetMaxWaveLength()).ToArray();
        foreach (var channel in this.Channels)
        {
            var waveNumericData = channel.CreateWave();
            for (int i = 0; i < waveNumericData.Length; i++)
            {
                result[i] += (ushort)(waveNumericData[i] / this.Channels.Count);
            }
        }
        return new MonauralWave(result);
    }
}
