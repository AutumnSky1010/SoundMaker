
namespace SoundMaker.SoundWave.Score;
public enum Scale
{
	A,
	A_SHARP,
	B,
	C,
	C_SHARP,
	D,
	D_SHARP,
	E,
	F,
	F_SHARP,
	G,
	G_SHARP,
}
public class EqualTemperament : ISoundComponent
{
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
	public double Hertz { get; } = 0d;
	public double Second { get; } = 0d;
	private int _volume { get; set; } = 100;
	public int Volume
	{
		get { return this._volume; }
		set
		{
			this._volume = value < 0 ? 0 : value;
			this._volume = value > 100 ? 100 : value;
		}
	}
	public EqualTemperament(Scale scale, uint scaleNumber, double second)
	{
		this.CheckArgument(scale, scaleNumber, second);
		this.Second = second;
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
	private void CheckArgument(Scale scale, uint scaleNumber, double second)
	{
		if (scaleNumber >= 9)
		{
			throw new ArgumentException();
		}
		if (scale != Scale.A && scale != Scale.B && scale != Scale.A_SHARP && scaleNumber == 0)
		{
			throw new ArgumentException();
		}
		if (scale != Scale.C && scaleNumber == 8)
		{
			throw new ArgumentException();
		}
		if (second <= 0)
		{
			throw new ArgumentException();
		}
	}
}
