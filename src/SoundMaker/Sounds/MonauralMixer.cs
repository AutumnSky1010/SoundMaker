using SoundMaker.Sounds.SoundChannels;

namespace SoundMaker.Sounds;

/// <summary>
/// Mix waves to monaural wave. <br/>モノラル音声をミックスするクラス。
/// </summary>
/// <param name="channels">Channels. <br/>チャンネルのリスト(読み取り専用)</param>
public class MonauralMixer(IReadOnlyList<ISoundChannel> channels) : MixerBase(channels)
{
    private object LockObject { get; } = new object();

    /// <summary>
    /// Mix. <br/>ミックスする。
    /// </summary>
    /// <returns>The mixed wave of monaural. <br/>モノラルの波形データ : MonauralWave</returns>
    public MonauralWave Mix()
    {
        var result = Enumerable.Repeat((short)0, GetMaxWaveLength()).ToArray();
        _ = Parallel.ForEach(Channels, channel =>
        {
            var waveNumericData = channel.GenerateWave();
            lock (LockObject)
            {
                for (var i = 0; i < waveNumericData.Length; i++)
                {
                    result[i] += (short)(waveNumericData[i] / Channels.Count);
                }
            }
        });
        return new MonauralWave(result);
    }
}
