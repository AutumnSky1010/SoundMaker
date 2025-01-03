using SoundMaker.Sounds.SoundChannels;

namespace SoundMaker.Sounds;

/// <summary>
/// Mix waves to stereo wave. <br/>ステレオ音声をミックスするクラス。
/// </summary>
/// <param name="channels">Channels. <br/>チャンネルのリスト</param>
public class StereoMixer(IReadOnlyList<ISoundChannel> channels) : MixerBase(channels)
{
    private object LockLeftObject { get; } = new object();

    private object LockRightObject { get; } = new object();

    /// <summary>
    /// Mix. <br/>ミックスするメソッド。
    /// </summary>
    /// <returns>Stereo wave data. <br/>ステレオ波形データ</returns>
    public StereoWave Mix()
    {
        var max = GetMaxWaveLength();
        var channelCount = GetChannelCount();
        var rightResult = Enumerable.Repeat((short)0, max).ToArray();
        var leftResult = Enumerable.Repeat((short)0, max).ToArray();
        _ = Parallel.ForEach(Channels, channel =>
        {
            Merge(leftResult, rightResult, channel, channelCount);
        });
        return new StereoWave(rightResult, leftResult);
    }

    private void Merge(short[] left, short[] right, ISoundChannel channel, ChannelCount channelCount)
    {
        var waveNumericData = channel.GenerateWave();
        if (channel.PanType is PanType.Left)
        {
            lock (LockLeftObject)
            {
                for (var i = 0; i < waveNumericData.Length; i++)
                {
                    left[i] += (short)(waveNumericData[i] / channelCount.Left);
                }
            }
        }
        else if (channel.PanType is PanType.Right)
        {
            lock (LockRightObject)
            {
                for (var i = 0; i < waveNumericData.Length; i++)
                {
                    right[i] += (short)(waveNumericData[i] / channelCount.Right);
                }
            }
        }
        // If sound is coming from both channels
        else
        {
            lock (LockLeftObject)
            {
                lock (LockRightObject)
                {
                    for (var i = 0; i < waveNumericData.Length; i++)
                    {
                        right[i] += (short)(waveNumericData[i] / channelCount.Right);
                        left[i] += (short)(waveNumericData[i] / channelCount.Left);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Method to count the number of channels for left and right. <br/>左右それぞれのチャンネルの個数を数えるメソッド。
    /// </summary>
    /// <returns>Number of channels for left and right. <br/>左右それぞれのチャンネルの個数</returns>
    private ChannelCount GetChannelCount()
    {
        var right = 0;
        var left = 0;
        foreach (var channel in Channels)
        {
            // Increment both if both channels are used.
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

    private readonly struct ChannelCount(int left, int right)
    {
        public int Right { get; } = right;

        public int Left { get; } = left;
    }
}
