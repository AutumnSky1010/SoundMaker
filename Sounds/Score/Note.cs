using SoundMaker.Sounds.WaveTypes;

namespace SoundMaker.Sounds.Score;
/// <summary>
/// note. 音符を表すクラス
/// </summary>
public class Note : BasicSoundComponentBase
{
    /// <summary>
    /// constructor コンストラクタ
    /// </summary>
    /// <param name="scale">scale of the note.音の高さ</param>
    /// <param name="scaleNumber">sound height number. (C"4" is middle C.)音の高さの番号（Cの「4」が真ん中のド）</param>
    /// <param name="length">length (ex. "quarter" note) 長さ（音楽的な、「四分」音符、「全」休符のような長さを表す。）</param>
    /// <param name="isDotted">note/rest is dotted. 付点かを表す論理型</param>
	public Note(Scale scale, uint scaleNumber, LengthType length, bool isDotted = false)
        : base(length, isDotted)
    {
        this.CheckArgument(scale, scaleNumber);
        if ((int)scale >= 3)
        {
            scaleNumber--;
        }
        this.Hertz += this.AHertz[scaleNumber];
        for (int i = 0; i < (int)scale; i++)
        {
            this.Hertz *= 1.059463094;
        }
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
    /// hertz of the sound. 音の周波数
    /// </summary>
    public double Hertz { get; } = 0d;

    private int _volume = 100;
    /// <summary>
    /// volume of the sound. 音の大きさ(0 ~ 100の間)
    /// </summary>
    public int Volume
    {
        get { return this._volume; }
        set
        {
            this._volume = value < 0 ? 0 : value;
            this._volume = value > 100 ? 100 : value;
        }
    }

    private void CheckArgument(Scale scale, uint scaleNumber)
    {
        if (scaleNumber >= 9)
        {
            throw new ArgumentException();
        }
        if (scale != Scale.A && scale != Scale.B && scale != Scale.ASharp && scaleNumber == 0)
        {
            throw new ArgumentException();
        }
        if (scale != Scale.C && scaleNumber == 8)
        {
            throw new ArgumentException();
        }
    }

    public override ushort[] GenerateWave(SoundFormat format, int tempo, int length, WaveTypeBase waveType)
    {
        return waveType.GenerateWave(format, tempo, length, this.Volume, this.Hertz);
    }

    public override ushort[] GenerateWave(SoundFormat format, int tempo, WaveTypeBase waveType)
    {
        int length = this.GetWaveArrayLength(format, tempo);
        return this.GenerateWave(format, tempo, length, waveType);
    }
}
