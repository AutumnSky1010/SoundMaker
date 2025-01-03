using SoundMaker.Sounds.WaveTypes;

namespace SoundMaker.Sounds.SoundChannels;
public class PseudoTriangleSoundChannel : SoundChannelBase
{
    /// <summary>
    /// Constructor. <br/>コンストラクタ
    /// </summary>
    /// <param name="format">Format of the sound. <br/>音のフォーマット</param>
    /// <param name="tempo">Quarter note/rest per minute. <br/>一分間の四分音符・休符の数</param>
    /// <param name="panType">Sound direction. <br/>左右どちらから音が出るか</param>
    /// <exception cref="ArgumentOutOfRangeException">Tempo must be non-negative and greater than 0.</exception>
    public PseudoTriangleSoundChannel(int tempo, SoundFormat format, PanType panType) : base(tempo, format, panType)
    {
    }

    /// <summary>
    /// Constructor. <br/>コンストラクタ
    /// </summary>
    /// <param name="format">Format of the sound. <br/>音のフォーマット</param>
    /// <param name="tempo">Quarter note/rest per minute. <br/>一分間の四分音符・休符の数</param>
    /// <param name="panType">Sound direction. <br/>左右どちらから音が出るか</param>
    /// <param name="capacity">The total number of sound components the internal data structure can hold without resizing. <br/>内部データ構造がリサイズされずに保持できるサウンドコンポーネントの総数。</param>
    /// <exception cref="ArgumentOutOfRangeException">Tempo must be non-negative and greater than 0.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Capacity must be non-negative.</exception>
    public PseudoTriangleSoundChannel(int tempo, SoundFormat format, PanType panType, int capacity) : base(tempo, format, panType, capacity)
    {
    }

    public override short[] GenerateWave()
    {
        var result = new List<short>();
        foreach (var soundComponent in SoundComponents)
        {
            var wave = soundComponent.GenerateWave(Format, Tempo, new PseudoTriangleWave());
            FadeInOut(wave);
            result.AddRange(wave);
        }
        return result.ToArray();
    }
}
