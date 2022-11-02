namespace SoundMaker.Sounds.SoundChannels;
public class TriangleSoundChannel : SoundChannelBase, ISoundChannel
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="tempo">一分間の四分音符・休符の数</param>
    /// <param name="format">音のフォーマット</param>
    /// <param name="panType">左右どちらから音が出るか</param>
    /// <param name="soundComponentCount">サウンドコンポーネントの個数</param>
    public TriangleSoundChannel(int tempo, SoundFormat format, PanType panType, int soundComponentCount) : base(tempo, format, panType, soundComponentCount) { }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="tempo">一分間の四分音符・休符の数</param>
    /// <param name="format">音のフォーマット</param>
    /// <param name="panType">サウンドコンポーネントの個数</param>
    public TriangleSoundChannel(int tempo, SoundFormat format, PanType panType) : base(tempo, format, panType) { }

    public override ushort[] CreateWave()
    {
        var result = new List<ushort>();
        foreach (var soundComponent in this.SoundComponents)
        {
            result.AddRange(soundComponent.GetTriangleWave(this.Format, this.Tempo));
        }
        return result.ToArray();
    }
}
