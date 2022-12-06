using SoundMaker.Sounds.Score;
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
    /// <param name="ratio">デューティ比</param>
    /// <param name="panType">pan of the sound. 左右どちらから音が出るか</param>
    public SquareSoundChannel(int tempo, SoundFormat format, SquareWaveRatio ratio, PanType panType) : base(tempo, format, panType)
    {
        this.Ratio = ratio;
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="tempo">quarter note/rest per minute. 一分間の四分音符・休符の数</param>
    /// <param name="format">format of the sound.音のフォーマット</param>
    /// <param name="ratio">デューティ比</param>
    /// <param name="panType">pan of the sound. 左右どちらから音が出るか</param>
    /// <param name="componentsCount">count of sound components. サウンドコンポーネントの個数</param>
    public SquareSoundChannel(int tempo, SoundFormat format, SquareWaveRatio ratio, PanType panType, int componentsCount)
        : base(tempo, format, panType, componentsCount)
    {
        this.Ratio = ratio;
    }

    /// <summary>
    /// デューティ比
    /// </summary>
    private SquareWaveRatio Ratio { get; }

    public override ushort[] GenerateWave()
    {
        var result = new List<ushort>();
        foreach (var soundComponent in this.SoundComponents)
        {
            result.AddRange(soundComponent.GenerateWave(this.Format, this.Tempo, new SquareWave(this.Ratio)));
        }
        return result.ToArray();
    }
}
