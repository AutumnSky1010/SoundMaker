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
	public List<byte> Mix()
	{
		int maxLength = this.GetMaxLength();
		var resultIntList = Enumerable.Repeat<ulong>(0, maxLength).ToList();

		foreach (var wave in this._waves)
		{
			byte[] waveByte = wave.GetBytes();
			for (int i = 0; i < waveByte.Length; i++)
			{
				resultIntList[i] = resultIntList[i] + waveByte[i];
			}
		}
		return resultIntList.ConvertAll<byte>((x) => (byte)(x / (ulong)this._waves.Count));
	}
	private int GetMaxLength()
	{
		int maxLength = 0;
		foreach (var wave in this._waves)
		{
			byte[] waveBytes = wave.GetBytes();
			maxLength = waveBytes.Length >= maxLength ? waveBytes.Length : maxLength;
		}
		return maxLength;
	}
}
