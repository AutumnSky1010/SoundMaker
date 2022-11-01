using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoundMaker.WaveFile;
using SoundMaker.SoundWave.Score;
using SoundMaker.SoundWave.WaveFactory;

namespace SoundMaker.SoundWave;

public class SquareSoundChannel : SoundChannelBase, ISoundChannel
{
    public SquareSoundChannel(int tempo, FormatChunk format, SquareWaveRatio ratio, PanType panType) : base(tempo, format, panType)
    {
        this.Ratio = ratio;
    }

    private SquareWaveRatio Ratio { get; }

    public override ushort[] CreateWave()
    {
        var result = new List<ushort>();
        foreach (var soundComponent in this.SoundComponents)
        {
            result.AddRange(soundComponent.GetSquareWave(this.Format, this.Ratio, this.Tempo));
        }
        return result.ToArray();
    }
}
