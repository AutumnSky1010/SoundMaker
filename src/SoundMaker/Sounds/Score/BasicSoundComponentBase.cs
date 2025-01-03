using SoundMaker.Sounds.WaveTypes;

namespace SoundMaker.Sounds.Score;

/// <summary>
/// Provides a base class for a basic sound component to inherit from. <br/>音符・休符など音の基本部品を表す抽象基底クラス
/// </summary>
public abstract class BasicSoundComponentBase : ISoundComponent
{
    /// <summary>
    /// Constructor. <br/>コンストラクタ
    /// </summary>
    /// <param name="length">Length (ex. "quarter" note). <br/>長さ（音楽的な、「四分」音符、「全」休符のような長さを表す。）</param>
    /// <param name="isDotted">Is note/rest dotted. <br/>付点かを表す論理型</param>
    public BasicSoundComponentBase(LengthType length, bool isDotted)
    {
        Length = length;
        IsDotted = isDotted;
    }

    /// <summary>
    /// Length (ex. "quarter" note). <br/>長さ（音楽的な、「四分」音符、「全」休符のような長さを表す。）
    /// </summary>
    public LengthType Length { get; }

    /// <summary>
    /// Note/rest is dotted. <br/>付点かを表す論理型
    /// </summary>
    public bool IsDotted { get; }

    public abstract short[] GenerateWave(SoundFormat format, int tempo, int length, WaveTypeBase waveType);

    public abstract short[] GenerateWave(SoundFormat format, int tempo, WaveTypeBase waveType);

    public abstract ISoundComponent Clone();

    public int GetWaveArrayLength(SoundFormat format, int tempo)
    {
        return SoundWaveLengthCalculator.Calculate(format, tempo, Length, IsDotted);
    }
}
