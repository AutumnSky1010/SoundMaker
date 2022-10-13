using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoundMaker.WaveFile;
using SoundMaker.SoundWave.Score;
namespace SoundMaker.SoundWave;
public enum SquareWaveRatio
{
	POINT_125,
	POINT_25,
	POINT_5
}
public class SquareWaveFactory : WaveFactoryBase, IWaveFactory
{
	private List<(double, double)> _ratio { get; } = new List<(double, double)>
	{
		(0.875, 0.125),
		(0.75, 0.25),
		(0.5, 0.5),
	};
	private int _ratioIndex { get; } 

	public SquareWaveFactory(int equalTemperamentCount, SquareWaveRatio ratio) : base(equalTemperamentCount)
	{
		this._ratioIndex = (int)ratio;
	}
	public SquareWaveFactory(SquareWaveRatio ratio) => this._ratioIndex = (int)ratio;
	public override MonauralWave CreateMonaural(FormatChunk format)
	{
		var result = new List<ushort>((int)this.Second * (int)format.SamplingFrequency);
		bool mode = false;
		int count = 1;
		for (int i = 0; i < this._soundComponents.Count; i++)
		{
			while (count <= this._soundComponents[i].Second * format.SamplingFrequency)
			{
				// 休符の時
				if (this._soundComponents[i] is Rest)
				{
					result.Add(0);
					count++;
					continue;
				}
				var equalTemperament = (EqualTemperament)this._soundComponents[i];
				double hertz = equalTemperament.Hertz;
				int repeatNumber = (int)(format.SamplingFrequency / equalTemperament.Hertz);
				for (int j = 1; j <= repeatNumber * this._ratio[_ratioIndex].Item1 && mode; j++, count++)
				{
					ushort sound = (ushort)(ushort.MaxValue * equalTemperament.Volume / 100);
					result.Add(sound);
				}
				for (int j = 1; j <= repeatNumber * this._ratio[_ratioIndex].Item2 && !mode; j++, count++)
				{
					byte sound = 0;
					result.Add(sound);
				}
				mode = !mode;
			}
			count = 1;
		}
		return new MonauralWave(result);
	}
}
