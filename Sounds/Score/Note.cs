namespace SoundMaker.Sounds.Score;
/// <summary>
/// 音符を表すクラス
/// </summary>
public class Note : BasicSoundComponentBase
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="scale">音の高さ</param>
    /// <param name="scaleNumber">音の高さの番号（Cの「4」が真ん中のド）</param>
    /// <param name="length">長さ（音楽的な、「四分」音符、「全」休符のような長さを表す。）</param>
    /// <param name="isDotted">付点かどうかを表す論理型</param>
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
    /// デューティ比を基に繰り返し回数を求める為の値
    /// </summary>
    private List<(double, double)> Ratio { get; } = new List<(double, double)>
    {
        (0.875, 0.125),
        (0.75, 0.25),
        (0.5, 0.5),
    };

    /// <summary>
    /// 音の周波数
    /// </summary>
    public double Hertz { get; } = 0d;

    private int _volume = 100;
    /// <summary>
    /// 音の大きさ(0 ~ 100の間)
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

    public override ushort[] GetSquareWave(SoundFormat format, SquareWaveRatio squareWaveRatio, int tempo)
    {
        int length = this.GetWaveArrayLength(format, tempo);
        return this.GetSquareWave(format, squareWaveRatio, tempo, length);
    }

    public override ushort[] GetSquareWave(SoundFormat format, SquareWaveRatio squareWaveRatio, int tempo, int length)
    {
        var result = new List<ushort>(length);
        bool mode = false;
        int count = 1;
        int ratioIndex = (int)squareWaveRatio;
        while (count <= length)
        {
            int allRepeatTimes = (int)((int)format.SamplingFrequency / this.Hertz);
            int firstRepeatTimes = (int)(allRepeatTimes * this.Ratio[ratioIndex].Item1);

            if (count + allRepeatTimes >= length)
            {
                result.Add(0);
                count++;
                continue;
            }

            for (int i = 1; i <= firstRepeatTimes && mode && count <= length; i++, count++)
            {
                ushort sound = (ushort)(ushort.MaxValue * this.Volume / 100);
                result.Add(sound);
            }

            for (int i = 1; i <= allRepeatTimes - firstRepeatTimes && !mode && count <= length; i++, count++)
            {
                ushort sound = 0;
                result.Add(sound);
            }
            mode = !mode;
        }
        return result.ToArray();
    }

    public override ushort[] GetTriangleWave(SoundFormat format, int tempo)
    {
        int listLength = this.GetWaveArrayLength(format, tempo);
        return this.GetTriangleWave(format, tempo, listLength);
    }


    public override ushort[] GetTriangleWave(SoundFormat format, int tempo, int length)
    {
        var result = new List<ushort>(length);
        bool mode = false;
        int count = 1;
        // 音の長さまで繰り返す
        while (count <= length)
        {
            // △の波形の波形を作るための繰り返し回数
            double repeatNumber = (int)format.SamplingFrequency / this.Hertz;
            // 直線の方程式の傾きを求める。
            double slope = ushort.MaxValue / repeatNumber;
            if (count + repeatNumber >= length)
            {
                result.Add(0);
                count++;
                continue;
            }
            for (int j = 1; j <= repeatNumber && count <= length; j++, count++)
            {
                ushort sound = mode ? (ushort)(slope * j * this.Volume / 100) : (ushort)((ushort.MaxValue + slope * j) * this.Volume / 100);
                result.Add(sound);
                if (j == (int)(repeatNumber / 2))
                {
                    mode = !mode;
                    slope = -slope;
                }
            }
        }
        return result.ToArray();
    }
}
