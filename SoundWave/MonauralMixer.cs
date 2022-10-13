using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundMaker.SoundWave;
internal class MonauralMixer
{
	private IReadOnlyList<MonauralWave> _waves { get; }
    public MonauralMixer(IReadOnlyList<MonauralWave> waves)
	{
		this._waves = waves;
	}
	public MonauralWave Mix()
	{
		int maxLength = this.GetMaxLength();
		var resultIntList = Enumerable.Repeat<ulong>(0, maxLength).ToList();

		foreach (var wave in this._waves)
		{
			ushort[] waveValues = wave.GetValues();
			for (int i = 0; i < waveValues.Length; i++)
			{
				resultIntList[i] = resultIntList[i] + waveValues[i];
			}
		}
		return new MonauralWave(resultIntList.ConvertAll<ushort>((x) => (ushort)(x / (ulong)this._waves.Count)));
	}
	private int GetMaxLength()
	{
		int maxLength = 0;
		foreach (var wave in this._waves)
		{
			ushort[] waveValues = wave.GetValues();
			maxLength = waveValues.Length >= maxLength ? waveValues.Length : maxLength;
		}
		return maxLength;
	}
}
