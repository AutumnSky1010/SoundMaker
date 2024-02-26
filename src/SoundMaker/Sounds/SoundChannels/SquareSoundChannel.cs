using SoundMaker.Sounds.WaveTypes;

namespace SoundMaker.Sounds.SoundChannels;
/// <summary>
/// this generates square wave. 矩形波を生成するサウンドチャンネル
/// </summary>
public class SquareSoundChannel : SoundChannelBase
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="tempo">quarter note/rest per minute. 一分間の四分音符・休符の数</param>
    /// <param name="format">format of the sound.音のフォーマット</param>
    /// <param name="ratio">duty cycle. デューティ比</param>
    /// <param name="panType">pan of the sound. 左右どちらから音が出るか</param>
    /// <exception cref="ArgumentOutOfRangeException">Tempo must be non-negative and greater than 0.</exception>
    public SquareSoundChannel(int tempo, SoundFormat format, SquareWaveRatio ratio, PanType panType) : base(tempo, format, panType)
    {
        Ratio = ratio;
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="tempo">quarter note/rest per minute. 一分間の四分音符・休符の数</param>
    /// <param name="format">format of the sound.音のフォーマット</param>
    /// <param name="ratio">duty cycle. デューティ比</param>
    /// <param name="panType">pan of the sound. 左右どちらから音が出るか</param>
    /// <param name="capacity">the total number of sound components the internal data structure can hold without resizing. 内部データ構造がリサイズされずに保持できるサウンドコンポーネントの総数。</param>
    /// <exception cref="ArgumentOutOfRangeException">Tempo must be non-negative and greater than 0.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Capacity must be non-negative.</exception>
    public SquareSoundChannel(int tempo, SoundFormat format, SquareWaveRatio ratio, PanType panType, int capacity)
        : base(tempo, format, panType, capacity)
    {
        Ratio = ratio;
    }

    /// <summary>
    /// デューティ比
    /// </summary>
    private SquareWaveRatio Ratio { get; }

    public override ushort[] GenerateWave()
    {
        var result = new List<ushort>();
        foreach (var soundComponent in SoundComponents)
        {
            result.AddRange(soundComponent.GenerateWave(Format, Tempo, new SquareWave(Ratio)));
        }
        return result.ToArray();
    }
}
