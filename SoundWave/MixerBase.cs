using SoundMaker.WaveFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundMaker.SoundWave;
public abstract class MixerBase
{
    public MixerBase(IReadOnlyList<ISoundChannel> channels)
    {
        this.Channels = channels;
    }

    protected IReadOnlyList<ISoundChannel> Channels { get; }

    protected FormatChunk Format { get; }

    protected int GetMaxWaveLength()
    {
        int max = 0;
        foreach (var channel in this.Channels)
        {
            max = max < channel.WaveArrayLength ? channel.WaveArrayLength : max;
        }
        return max;
    }
}
