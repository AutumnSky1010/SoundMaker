using SoundMaker.WaveFile;
using SoundMaker.SoundWave.Score;

namespace SoundMaker.SoundWave;
public class TriangleWaveFactory : WaveFactoryBase, IWaveFactory
{
	public TriangleWaveFactory(int soundComponentCount) : base(soundComponentCount) { }
	public TriangleWaveFactory() : base() { }
	public override MonauralWave CreateMonaural(FormatChunk format)
	{
		var result = new List<ushort>((int)this.Second * (int)format.SamplingFrequency);
		bool mode = true;
		int count = 1;
		for (int i = 0; i < this._soundComponents.Count; i++)
		{
			// 音の長さの秒数まで繰り返す
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
				// △の波形の波形を作るための繰り返し回数
				double repeatNumber = format.SamplingFrequency / hertz;
				// 直線の方程式の傾きを求める。
				double slope = ushort.MaxValue / repeatNumber;
				for (int j = 1; j <= repeatNumber; j++, count++)
				{
					ushort sound = mode ? (ushort)(slope * j * equalTemperament.Volume / 100) : (ushort)(( ushort.MaxValue + slope * j) * equalTemperament.Volume / 100);
					result.Add(sound);
					if (j == (int)(repeatNumber / 2))
					{
                        mode = !mode;
						slope = -slope;
                    }
				}
				
			}
			count = 1;
		}
		return new MonauralWave(result);
	}
}
