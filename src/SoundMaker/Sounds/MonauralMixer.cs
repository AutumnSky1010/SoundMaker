using SoundMaker.Sounds.SoundChannels;

namespace SoundMaker.Sounds;
/// <summary>
/// mix waves to monaural wave. モノラル音声をミックスするクラス。
/// </summary>
public class MonauralMixer : MixerBase
{
    /// <summary>
    /// constructor. コンストラクタ
    /// </summary>
    /// <param name="channels">channels. チャンネルのリスト(読み取り専用)</param>
    public MonauralMixer(IReadOnlyList<ISoundChannel> channels) : base(channels)
    {
    }

    private object LockObject { get; } = new object();

    /// <summary>
    /// mix ミックスする。
    /// </summary>
    /// <returns>the mixed wave of monaural. モノラルの波形データ : MonauralWave</returns>
    public MonauralWave Mix()
    {
        var result = Enumerable.Repeat((ushort)0, GetMaxWaveLength()).ToArray();
        _ = Parallel.ForEach(Channels, channel =>
        {
            var waveNumericData = channel.GenerateWave();
            lock (LockObject)
            {
                for (var i = 0; i < waveNumericData.Length; i++)
                {
                    result[i] += (ushort)(waveNumericData[i] / Channels.Count);
                }
            }
        });
        return new MonauralWave(result);
    }
}
