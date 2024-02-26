using SoundMaker.Sounds.WaveTypes;

namespace SoundMaker.Sounds.Score;
/// <summary>
/// provides a base class for a basic sound component to inherit from. 音符・休符など音の基本部品を表す抽象基底クラス
/// </summary>
public abstract class BasicSoundComponentBase : ISoundComponent
{
    /// <summary>
    /// constructor コンストラクタ
    /// </summary>
    /// <param name="length">length (ex. "quarter" note) 長さ（音楽的な、「四分」音符、「全」休符のような長さを表す。）</param>
    /// <param name="isDotted">is note/rest dotted. 付点かを表す論理型</param>
    public BasicSoundComponentBase(LengthType length, bool isDotted)
    {
        Length = length;
        IsDotted = isDotted;
    }

    /// <summary>
    /// length (ex. "quarter" note) 長さ（音楽的な、「四分」音符、「全」休符のような長さを表す。）
    /// </summary>
    public LengthType Length { get; }

    /// <summary>
    /// note/rest is dotted. 付点かを表す論理型
    /// </summary>
    public bool IsDotted { get; }

    public abstract ushort[] GenerateWave(SoundFormat format, int tempo, int length, WaveTypeBase waveType);

    public abstract ushort[] GenerateWave(SoundFormat format, int tempo, WaveTypeBase waveType);

    public int GetWaveArrayLength(SoundFormat format, int tempo)
    {
        return SoundWaveLengthCaluclator.Caluclate(format, tempo, Length, IsDotted);
    }
}
