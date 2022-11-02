using SoundMaker.Sounds.Score;

namespace SoundMaker.Sounds.SoundChannels;
/// <summary>
/// 矩形波を生成するサウンドチャンネル
/// </summary>
public class SquareSoundChannel : SoundChannelBase, ISoundChannel
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="tempo">一分間の四分音符・休符の数</param>
    /// <param name="format">音のフォーマット</param>
    /// <param name="ratio">デューティ比</param>
    /// <param name="panType">左右どちらから音が出るか</param>
    public SquareSoundChannel(int tempo, SoundFormat format, SquareWaveRatio ratio, PanType panType) : base(tempo, format, panType)
    {
        this.Ratio = ratio;
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="tempo">一分間の四分音符・休符の数</param>
    /// <param name="format">音のフォーマット</param>
    /// <param name="ratio">デューティ比</param>
    /// <param name="panType">左右どちらから音が出るか</param>
    /// <param name="componentsCount">サウンドコンポーネントの個数</param>
    public SquareSoundChannel(int tempo, SoundFormat format, SquareWaveRatio ratio, PanType panType, int componentsCount)
        : base(tempo, format, panType, componentsCount)
    {
        this.Ratio = ratio;
    }

    /// <summary>
    /// デューティ比
    /// </summary>
    private SquareWaveRatio Ratio { get; }

    public override ushort[] CreateWave()
    {
        var result = new List<ushort>();
        foreach (var soundComponent in this.SoundComponents)
        {
            result.AddRange(soundComponent.GetSquareWave(this.Format, this.Ratio, this.Tempo));
        }
        return result.ToArray();
    }
}
