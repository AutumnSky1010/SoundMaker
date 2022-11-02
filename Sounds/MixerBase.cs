using SoundMaker.Sounds.SoundChannels;

namespace SoundMaker.Sounds;
/// <summary>
/// ミキサーの抽象基底クラス
/// </summary>
public abstract class MixerBase
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="channels">音声チャンネルのリスト(読み取り専用)</param>
    public MixerBase(IReadOnlyList<ISoundChannel> channels)
    {
        this.Channels = channels;
    }

    /// <summary>
    /// チャンネルのリスト
    /// </summary>
    protected IReadOnlyList<ISoundChannel> Channels { get; }

    /// <summary>
    /// 音のフォーマット
    /// </summary>
    protected SoundFormat Format { get; }

    /// <summary>
    /// 各チャンネルの波形データで一番長い配列の長さを返すメソッド。
    /// </summary>
    /// <returns>最長の配列の長さ : int</returns>
    protected int GetMaxWaveLength()
    {
        int max = 0;
        foreach (var channel in this.Channels)
        {
            max = max < channel.WaveArrayLength ? channel.WaveArrayLength : max;
        }
        return max;
    }
}
