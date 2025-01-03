using SoundMaker.Sounds.WaveTypes;

namespace SoundMaker.Sounds.Score;

/// <summary>
/// The note. <br/>音符を表すクラス
/// </summary>
public class Note : BasicSoundComponentBase
{
    /// <summary>
    /// Constructor. <br/>コンストラクタ
    /// </summary>
    /// <param name="scale">Scale of the note. <br/>音の高さ</param>
    /// <param name="scaleNumber">Sound height number. (C"4" is middle C.) <br/>音の高さの番号（Cの「4」が真ん中のド）</param>
    /// <param name="length">Length (ex. "quarter" note). <br/>長さ（音楽的な、「四分」音符、「全」休符のような長さを表す。）</param>
    /// <param name="isDotted">Is note/rest dotted. <br/>付点かを表す論理型</param>
    /// <exception cref="ArgumentException">Scale and scale number must be only the range of sound that the piano can produce.</exception>
    public Note(Scale scale, int scaleNumber, LengthType length, bool isDotted = false)
        : base(length, isDotted)
    {
        CheckArgument(scale, scaleNumber);
        Hertz += ScaleHertzDictionary.GetHertz(scale, scaleNumber);
        Scale = scale;
        ScaleNumber = scaleNumber;
    }

    /// <summary>
    /// Easiness constructor (use case: construct Tie). Scale is "A4". <br/>簡易コンストラクタ(使用場面: タイの初期化)。音の高さは"A4"
    /// </summary>
    /// <param name="length">Length (ex. "quarter" note). <br/>長さ（音楽的な、「四分」音符、「全」休符のような長さを表す。）</param>
    /// <param name="isDotted">Is note/rest dotted. <br/>付点かを表す論理型</param>
    public Note(LengthType length, bool isDotted = false) : base(length, isDotted)
    {
        var scale = Scale.A;
        var scaleNumber = 4;
        Hertz += ScaleHertzDictionary.GetHertz(scale, scaleNumber);
        Scale = scale;
        ScaleNumber = scaleNumber;
    }

    /// <summary>
    /// Scale of the note. <br/>音の高さ
    /// </summary>
    public Scale Scale { get; }

    /// <summary>
    /// Sound height number. (C"4" is middle C.) <br/>音の高さの番号（Cの「4」が真ん中のド）
    /// </summary>
    public int ScaleNumber { get; }

    /// <summary>
    /// Hertz of the sound. <br/>音の周波数
    /// </summary>
    public double Hertz { get; } = 0d;

    private int _volume = 100;

    /// <summary>
    /// Volume of the sound. (0 ~ 100) <br/>音の大きさ(0 ~ 100の間)
    /// </summary>
    public int Volume
    {
        get => _volume;
        set
        {
            if (value < 0)
            {
                value = 0;
            }
            else if (value > 100)
            {
                value = 100;
            }
            _volume = value;
        }
    }

    private static void CheckArgument(Scale scale, int scaleNumber)
    {
        var message = "'scale' and 'scaleNumber' must be only the range of sound that the piano can produce.";
        if (!ScaleHertzDictionary.IsValidScale(scale, scaleNumber))
        {
            throw new ArgumentException(message);
        }
    }

    public override short[] GenerateWave(SoundFormat format, int tempo, int length, WaveTypeBase waveType)
    {
        return waveType.GenerateWave(format, length, Volume, Hertz);
    }

    public override short[] GenerateWave(SoundFormat format, int tempo, WaveTypeBase waveType)
    {
        var length = GetWaveArrayLength(format, tempo);
        return GenerateWave(format, tempo, length, waveType);
    }

    public override Note Clone()
    {
        return new(Scale, ScaleNumber, Length, IsDotted)
        {
            Volume = Volume,
        };
    }
}
