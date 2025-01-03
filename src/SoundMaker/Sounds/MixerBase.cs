using SoundMaker.Sounds.SoundChannels;

namespace SoundMaker.Sounds;

/// <summary>
/// Provides a base class for a mixer to inherit from. <br/>ミキサーの抽象基底クラス
/// </summary>
/// <param name="channels">List of sound channels (read-only). <br/>音声チャンネルのリスト(読み取り専用)</param>
public abstract class MixerBase(IReadOnlyList<ISoundChannel> channels)
{
    /// <summary>
    /// List of channels. <br/>チャンネルのリスト
    /// </summary>
    protected IReadOnlyList<ISoundChannel> Channels { get; } = channels;

    /// <summary>
    /// Sound format. <br/>音のフォーマット
    /// </summary>
    protected SoundFormat Format { get; }

    /// <summary>
    /// Method to return the length of the longest array in each channel's waveform data. <br/>各チャンネルの波形データで一番長い配列の長さを返すメソッド。
    /// </summary>
    /// <returns>Length of the longest array. <br/>最長の配列の長さ : int</returns>
    protected int GetMaxWaveLength()
    {
        var max = 0;
        foreach (var channel in Channels)
        {
            max = max < channel.WaveArrayLength ? channel.WaveArrayLength : max;
        }
        return max;
    }
}
