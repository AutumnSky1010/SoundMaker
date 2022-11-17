using SoundMaker.Sounds.WaveTypes;

namespace SoundMaker.Sounds.SoundChannels;
/// <summary>
/// this generates low bit noise wave. ロービットノイズを生成するサウンドチャンネル
/// </summary>
public class LowBitNoiseSoundChannel : SoundChannelBase
{
    public LowBitNoiseSoundChannel(int tempo, SoundFormat format, PanType panType, int componentsCount) : base(tempo, format, panType, componentsCount)
    {
    }

    public LowBitNoiseSoundChannel(int tempo, SoundFormat format, PanType panType) : base(tempo, format, panType)
    {
    }

    public override ushort[] CreateWave()
    {
        var result = new List<ushort>();
        foreach (var soundComponent in this.SoundComponents)
        {
            result.AddRange(soundComponent.GenerateWave(this.Format, this.Tempo, new LowBitNoiseWave()));
        }
        return result.ToArray();
    }
}
