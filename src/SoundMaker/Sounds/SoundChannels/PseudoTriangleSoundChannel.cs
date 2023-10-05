using SoundMaker.Sounds.WaveTypes;

namespace SoundMaker.Sounds.SoundChannels;
public class PseudoTriangleSoundChannel : SoundChannelBase
{
    /// <summary>
    /// constructor コンストラクタ
    /// </summary>
    /// <param name="format">format of the sound. 音のフォーマット</param>
    /// <param name="tempo">quarter note/rest per minute. 一分間の四分音符・休符の数</param>
    /// <param name="panType">sound direction. 左右どちらから音が出るか</param>
    /// <exception cref="ArgumentOutOfRangeException">Tempo must be non-negative and greater than 0.</exception>
    public PseudoTriangleSoundChannel(int tempo, SoundFormat format, PanType panType) : base(tempo, format, panType)
    {
    }

    /// <summary>
    /// constructor コンストラクタ
    /// </summary>
    /// <param name="format">format of the sound. 音のフォーマット</param>
    /// <param name="tempo">quarter note/rest per minute. 一分間の四分音符・休符の数</param>
    /// <param name="panType">sound direction. 左右どちらから音が出るか</param>
    /// <param name="capacity">the total number of sound components the internal data structure can hold without resizing. 内部データ構造がリサイズされずに保持できるサウンドコンポーネントの総数。</param>
    /// <exception cref="ArgumentOutOfRangeException">Tempo must be non-negative and greater than 0.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Capacity must be non-negative.</exception>
    public PseudoTriangleSoundChannel(int tempo, SoundFormat format, PanType panType, int capacity) : base(tempo, format, panType, capacity)
    {
    }

    public override ushort[] GenerateWave()
    {
        var result = new List<ushort>();
        foreach (var soundComponent in this.SoundComponents)
        {
            result.AddRange(soundComponent.GenerateWave(this.Format, this.Tempo, new PseudoTriangleWave()));
        }
        return result.ToArray();
    }
}
