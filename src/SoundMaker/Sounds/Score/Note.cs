using SoundMaker.Sounds.WaveTypes;

namespace SoundMaker.Sounds.Score;
/// <summary>
/// the note. 音符を表すクラス
/// </summary>
public class Note : BasicSoundComponentBase
{
    private static readonly IReadOnlyDictionary<(Scale, int), double> _hertz = new Dictionary<(Scale, int), double>()
    {
        // A0 ~
        {(Scale.A, 0),      27.500 },
        {(Scale.ASharp, 0), 29.135 },
        {(Scale.B, 0),      30.868 },
        // C1 ~                       C2 ~                           C3 ~                           C4 ~
        {(Scale.C, 1),      32.703 }, {(Scale.C, 2),      65.406 },  {(Scale.C, 3),      130.813 }, {(Scale.C, 4),      261.626 },
        {(Scale.CSharp, 1), 34.648 }, {(Scale.CSharp, 2), 69.296 },  {(Scale.CSharp, 3), 138.591 }, {(Scale.CSharp, 4), 277.183 },
        {(Scale.D, 1),      36.708 }, {(Scale.D, 2),      73.416 },  {(Scale.D, 3),      146.832 }, {(Scale.D, 4),      293.665 },
        {(Scale.DSharp, 1), 38.891 }, {(Scale.DSharp, 2), 77.782 },  {(Scale.DSharp, 3), 155.563 }, {(Scale.DSharp, 4), 311.127 },
        {(Scale.E, 1),      41.203 }, {(Scale.E, 2),      82.407 },  {(Scale.E, 3),      164.814 }, {(Scale.E, 4),      329.628 },
        {(Scale.F, 1),      43.654 }, {(Scale.F, 2),      87.307 },  {(Scale.F, 3),      174.614 }, {(Scale.F, 4),      349.228 },
        {(Scale.FSharp, 1), 46.249 }, {(Scale.FSharp, 2), 92.499 },  {(Scale.FSharp, 3), 184.997 }, {(Scale.FSharp, 4), 369.994 },
        {(Scale.G, 1),      48.999 }, {(Scale.G, 2),      97.999 },  {(Scale.G, 3),      195.998 }, {(Scale.G, 4),      391.995 },
        {(Scale.GSharp, 1), 51.913 }, {(Scale.GSharp, 2), 103.826 }, {(Scale.GSharp, 3), 207.652 }, {(Scale.GSharp, 4), 415.305 },
        {(Scale.A, 1),      55.000 }, {(Scale.A, 2),      110.000 }, {(Scale.A, 3),      220.000 }, {(Scale.A, 4),      440.000 },
        {(Scale.ASharp, 1), 58.270 }, {(Scale.ASharp, 2), 116.541 }, {(Scale.ASharp, 3), 233.082 }, {(Scale.ASharp, 4), 466.164 },
        {(Scale.B, 1),      61.735 }, {(Scale.B, 2),      123.471 }, {(Scale.B, 3),      246.942 }, {(Scale.B, 4),      493.883 }, 
        // C5 ~                        C6 ~                            C7 ~
        {(Scale.C, 5),      523.251 }, {(Scale.C, 6),      1046.502 }, {(Scale.C, 7),      2093.005 },
        {(Scale.CSharp, 5), 554.365 }, {(Scale.CSharp, 6), 1108.731 }, {(Scale.CSharp, 7), 2217.461 },
        {(Scale.D, 5),      587.330 }, {(Scale.D, 6),      1174.659 }, {(Scale.D, 7),      2349.318 },
        {(Scale.DSharp, 5), 622.254 }, {(Scale.DSharp, 6), 1244.508 }, {(Scale.DSharp, 7), 2489.016 },
        {(Scale.E, 5),      659.255 }, {(Scale.E, 6),      1318.510 }, {(Scale.E, 7),      2637.020 },
        {(Scale.F, 5),      698.456 }, {(Scale.F, 6),      1396.913 }, {(Scale.F, 7),      2793.826 },
        {(Scale.FSharp, 5), 739.989 }, {(Scale.FSharp, 6), 1479.978 }, {(Scale.FSharp, 7), 2959.955 },
        {(Scale.G, 5),      783.991 }, {(Scale.G, 6),      1567.982 }, {(Scale.G, 7),      3135.963 },
        {(Scale.GSharp, 5), 830.609 }, {(Scale.GSharp, 6), 1661.219 }, {(Scale.GSharp, 7), 3322.438 },
        {(Scale.A, 5),      880.000 }, {(Scale.A, 6),      1760.000 }, {(Scale.A, 7),      3520.000 },
        {(Scale.ASharp, 5), 932.328 }, {(Scale.ASharp, 6), 1864.655 }, {(Scale.ASharp, 7), 3729.310 },
        {(Scale.B, 5),      987.767 }, {(Scale.B, 6),      1975.533 }, {(Scale.B, 7),      3951.066 },
        // C8
        {(Scale.C, 8),      4186.009 },
    };


    /// <summary>
    /// constructor コンストラクタ
    /// </summary>
    /// <param name="scale">scale of the note.音の高さ</param>
    /// <param name="scaleNumber">sound height number. (C"4" is middle C.)音の高さの番号（Cの「4」が真ん中のド）</param>
    /// <param name="length">length (ex. "quarter" note) 長さ（音楽的な、「四分」音符、「全」休符のような長さを表す。）</param>
    /// <param name="isDotted">is note/rest dotted. 付点かを表す論理型</param>
    /// <exception cref="ArgumentException">Scale and scale number must be only the range of sound that the piano can produce.</exception>
    public Note(Scale scale, int scaleNumber, LengthType length, bool isDotted = false)
        : base(length, isDotted)
    {
        CheckArgument(scale, scaleNumber);
        Hertz += _hertz[(scale, scaleNumber)];
        Scale = scale;
        ScaleNumber = scaleNumber;
    }

    /// <summary>
    /// easiness constructor(use case: construct Tie).Scale is "A4". 簡易コンストラクタ(使用場面: タイの初期化)。音の高さは"A4"
    /// </summary>
    /// <param name="length">length (ex. "quarter" note) 長さ（音楽的な、「四分」音符、「全」休符のような長さを表す。）</param>
    /// <param name="isDotted">is note/rest dotted. 付点かを表す論理型</param>
    public Note(LengthType length, bool isDotted = false) : base(length, isDotted)
    {
        var scale = Scale.A;
        var scaleNumber = 4;
        Hertz += AHertz[scaleNumber];
        Scale = scale;
        ScaleNumber = scaleNumber;
    }

    /// <summary>
    /// 「ラ」の周波数
    /// </summary>
    private double[] AHertz { get; } = new double[]
    {
        // A0からA
        27.5d,
        55.0d,
        110.0d,
        220.0d,
        440.0d,
        880.0d,
        1760.0d,
        3520.0d
    };
    /// <summary>
    /// scale of the note. 音の高さ
    /// </summary>
    public Scale Scale { get; }

    /// <summary>
    /// sound height number. (C"4" is middle C.)音の高さの番号（Cの「4」が真ん中のド）
    /// </summary>
    public int ScaleNumber { get; }

    /// <summary>
    /// hertz of the sound. 音の周波数
    /// </summary>
    public double Hertz { get; } = 0d;

    private int _volume = 100;
    /// <summary>
    /// volume of the sound.(0 ~ 100) 音の大きさ(0 ~ 100の間)
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

    private void CheckArgument(Scale scale, int scaleNumber)
    {
        var message = "'scale' and 'scaleNumber' must be only the range of sound that the piano can produce.";
        if (scaleNumber is >= 9 or <= (-1))
        {
            throw new ArgumentException(message);
        }
        if (scale != Scale.A && scale != Scale.B && scale != Scale.ASharp && scaleNumber == 0)
        {
            throw new ArgumentException(message);
        }
        if (scale != Scale.C && scaleNumber == 8)
        {
            throw new ArgumentException(message);
        }
    }

    public override ushort[] GenerateWave(SoundFormat format, int tempo, int length, WaveTypeBase waveType)
    {
        return waveType.GenerateWave(format, length, Volume, Hertz);
    }

    public override ushort[] GenerateWave(SoundFormat format, int tempo, WaveTypeBase waveType)
    {
        var length = GetWaveArrayLength(format, tempo);
        return GenerateWave(format, tempo, length, waveType);
    }
}
