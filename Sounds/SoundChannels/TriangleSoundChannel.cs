using SoundMaker.Sounds.WaveTypes;

namespace SoundMaker.Sounds.SoundChannels;
/// <summary>
/// this generates triangle wave. 三角波を生成するチャンネル。
/// </summary>
public class TriangleSoundChannel : SoundChannelBase
{
    /// <summary>
    /// constructor コンストラクタ
    /// </summary>
    /// <param name="format">format of the sound. 音のフォーマット</param>
    /// <param name="tempo">quarter note/rest per minute. 一分間の四分音符・休符の数</param>
    /// <param name="panType">sound direction. 左右どちらから音が出るか</param>
    /// <param name="soundComponentCount">count of sound components. サウンドコンポーネントの個数</param>
    public TriangleSoundChannel(int tempo, SoundFormat format, PanType panType, int soundComponentCount) : base(tempo, format, panType, soundComponentCount) { }

    /// <summary>
    /// constructor コンストラクタ
    /// </summary>
    /// <param name="format">format of the sound. 音のフォーマット</param>
    /// <param name="tempo">quarter note/rest per minute. 一分間の四分音符・休符の数</param>
    /// <param name="panType">sound direction. 左右どちらから音が出るか</param>
    public TriangleSoundChannel(int tempo, SoundFormat format, PanType panType) : base(tempo, format, panType) { }

    public override ushort[] GenerateWave()
    {
        var result = new List<ushort>();
        foreach (var soundComponent in this.SoundComponents)
        {
            result.AddRange(soundComponent.GenerateWave(this.Format, this.Tempo, new TriangleWave()));
        }
        return result.ToArray();
    }
}
