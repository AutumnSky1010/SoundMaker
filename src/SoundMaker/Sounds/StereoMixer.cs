using SoundMaker.Sounds.SoundChannels;
using System.Threading.Channels;

namespace SoundMaker.Sounds;
/// <summary>
/// mix waves to stereo wave. ステレオ音声をミックスするクラス。
/// </summary>
public class StereoMixer : MixerBase
{
    /// <summary>
    /// constructor. コンストラクタ
    /// </summary>
    /// <param name="channels">channels. チャンネルのリスト</param>
    public StereoMixer(IReadOnlyList<ISoundChannel> channels) : base(channels)
    {
    }

    private object LockLeftObject { get; } = new object();

    private object LockRightObject { get; } = new object();

    /// <summary>
    /// mix. ミックスするメソッド。
    /// </summary>
    /// <returns>ステレオ波形データ</returns>
    public StereoWave Mix()
    {
        int max = this.GetMaxWaveLength();
        var channelCount = this.GetChannelCount();
        ushort[] rightResult = Enumerable.Repeat((ushort)0, max).ToArray();
        ushort[] leftResult = Enumerable.Repeat((ushort)0, max).ToArray();
        Parallel.ForEach (this.Channels, channel =>
        {
            this.Merge(leftResult, rightResult, channel, channelCount);
        });
        return new StereoWave(rightResult, leftResult);
    }
    private void Merge(ushort[] left, ushort[] right, ISoundChannel channel, ChannelCount channelCount)
    {
        var waveNumericData = channel.GenerateWave();
        if (channel.PanType is PanType.Left)
        {
            lock (this.LockLeftObject)
            {
                for (int i = 0; i < waveNumericData.Length; i++)
                {
                    left[i] += (ushort)(waveNumericData[i] / channelCount.Left);
                }
            }
        }
        else if (channel.PanType is PanType.Right)
        {
            lock (this.LockRightObject)
            {
                for (int i = 0; i < waveNumericData.Length; i++)
                {
                    right[i] += (ushort)(waveNumericData[i] / channelCount.Right);
                }
            }
        }
        // 両方のチャンネルから音が出る場合
        else
        {
            lock (this.LockLeftObject)
            {
                lock (this.LockRightObject)
                {
                    for (int i = 0; i < waveNumericData.Length; i++)
                    {
                        right[i] += (ushort)(waveNumericData[i] / channelCount.Right);
                        left[i] += (ushort)(waveNumericData[i] / channelCount.Left);
                    }
                }
            }
        }
    }
    /// <summary>
    /// 左右それぞれのチャンネルの個数を数えるメソッド。
    /// </summary>
    /// <returns>左右それぞれのチャンネルの個数</returns>
    private ChannelCount GetChannelCount()
    {
        int right = 0;
        int left = 0;
        foreach (var channel in this.Channels)
        {
            // 両方の場合は両方インクリメントする。
            if (channel.PanType is PanType.Left || channel.PanType is PanType.Both)
            {
                left++;
            }
            if (channel.PanType is PanType.Right || channel.PanType is PanType.Both)
            {
                right++;
            }
        }
        return new ChannelCount(left, right);
    }

    private struct ChannelCount
    {
        public ChannelCount(int left, int right)
        {
            this.Left = left;
            this.Right = right;
        }

        public int Right { get; }

        public int Left { get; }
    }
}
