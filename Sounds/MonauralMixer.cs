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
        ushort[] result = Enumerable.Repeat((ushort)0, this.GetMaxWaveLength()).ToArray();
        Parallel.ForEach(this.Channels, channel =>
        {
            var waveNumericData = channel.CreateWave();
            lock (this.LockObject)
            {
                for (int i = 0; i < waveNumericData.Length; i++)
                {
                    result[i] += (ushort)(waveNumericData[i] / this.Channels.Count);
                }
            }
        });
        return new MonauralWave(result);
    }
}
