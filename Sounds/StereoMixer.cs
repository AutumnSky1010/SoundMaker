using SoundMaker.Sounds.SoundChannels;

namespace SoundMaker.Sounds;
/// <summary>
/// ステレオ音声をミックスするクラス。
/// </summary>
public class StereoMixer : MixerBase
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="channels">チャンネルのリスト</param>
    public StereoMixer(IReadOnlyList<ISoundChannel> channels) : base(channels)
    {
    }

    /// <summary>
    /// ミックスするメソッド。
    /// </summary>
    /// <returns>ステレオ波形データ</returns>
    public StereoWave Mix()
    {
        int max = this.GetMaxWaveLength();
        var channelCount = this.GetChannelCount();
        ushort[] rightResult = Enumerable.Repeat((ushort)0, max).ToArray();
        ushort[] leftResult = Enumerable.Repeat((ushort)0, max).ToArray();
        foreach (var channel in this.Channels)
        {
            var waveNumericData = channel.CreateWave();
            if (channel.PanType is PanType.Left)
            {
                for (int i = 0; i < waveNumericData.Length; i++)
                {
                    leftResult[i] += (ushort)(waveNumericData[i] / channelCount.Left);
                }
            }
            else if (channel.PanType is PanType.Right)
            {
                for (int i = 0; i < waveNumericData.Length; i++)
                {
                    rightResult[i] += (ushort)(waveNumericData[i] / channelCount.Right);
                }
            }
            // 両方のチャンネルから音が出る場合
            else
            {
                for (int i = 0; i < waveNumericData.Length; i++)
                {
                    rightResult[i] += (ushort)(waveNumericData[i] / channelCount.Right);
                    leftResult[i] += (ushort)(waveNumericData[i] / channelCount.Left);
                }
            }

        }
        return new StereoWave(rightResult, leftResult);
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
