using SoundMaker.SoundWave.Score;
using SoundMaker.WaveFile;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace SoundMaker.SoundWave;
internal class StereoMixer : MixerBase
{
    public StereoMixer(IReadOnlyList<ISoundChannel> channels) : base(channels)
    {
    }

    public StereoWave Mix()
    {
        int max = this.GetMaxWaveLength();
        var channelCount = this.GetChannelCount();
        ushort[] rightResult = Enumerable.Repeat((ushort)0, max).ToArray();
        ushort[] leftResult = Enumerable.Repeat((ushort)0, max).ToArray();
        foreach (var channel in this.Channels)
        {
            var waveNumericData = channel.CreateWave();
            if (channel.PanType is WaveFactory.PanType.LEFT)
            {
                for (int i = 0; i < waveNumericData.Length; i++)
                {
                    leftResult[i] += (ushort)(waveNumericData[i] / channelCount.Left);
                }
            }
            else
            {
                for (int i = 0; i < waveNumericData.Length; i++)
                {
                    rightResult[i] += (ushort)(waveNumericData[i] / channelCount.Right);
                }
            }

        }
        return new StereoWave(rightResult, leftResult);
    }

    private ChannelCount GetChannelCount()
    {
        int right = 0;
        int left = 0;
        foreach (var channel in this.Channels)
        {
            if (channel.PanType is WaveFactory.PanType.LEFT)
            {
                left++;
            }
            else
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
