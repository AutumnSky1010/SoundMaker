using SoundMaker.Sounds.SoundChannels;

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
        var max = GetMaxWaveLength();
        var channelCount = GetChannelCount();
        var rightResult = Enumerable.Repeat((ushort)0, max).ToArray();
        var leftResult = Enumerable.Repeat((ushort)0, max).ToArray();
        _ = Parallel.ForEach(Channels, channel =>
        {
            Merge(leftResult, rightResult, channel, channelCount);
        });
        return new StereoWave(rightResult, leftResult);
    }
    private void Merge(ushort[] left, ushort[] right, ISoundChannel channel, ChannelCount channelCount)
    {
        var waveNumericData = channel.GenerateWave();
        if (channel.PanType is PanType.Left)
        {
            lock (LockLeftObject)
            {
                for (var i = 0; i < waveNumericData.Length; i++)
                {
                    left[i] += (ushort)(waveNumericData[i] / channelCount.Left);
                }
            }
        }
        else if (channel.PanType is PanType.Right)
        {
            lock (LockRightObject)
            {
                for (var i = 0; i < waveNumericData.Length; i++)
                {
                    right[i] += (ushort)(waveNumericData[i] / channelCount.Right);
                }
            }
        }
        // 両方のチャンネルから音が出る場合
        else
        {
            lock (LockLeftObject)
            {
                lock (LockRightObject)
                {
                    for (var i = 0; i < waveNumericData.Length; i++)
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
        var right = 0;
        var left = 0;
        foreach (var channel in Channels)
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

    private readonly struct ChannelCount
    {
        public ChannelCount(int left, int right)
        {
            Left = left;
            Right = right;
        }

        public int Right { get; }

        public int Left { get; }
    }
}
