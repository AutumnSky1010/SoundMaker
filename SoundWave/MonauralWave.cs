using SoundMaker.WaveFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundMaker.SoundWave;
public class MonauralWave : IWave
{
	public MonauralWave(IReadOnlyCollection<ushort> wave)
	{
		ushort[] argumentIntegers = wave.ToArray();
		this._originalVolumeWave = new ushort[wave.Count];
		this._wave = new ushort[wave.Count];
		Array.Copy(argumentIntegers, this._originalVolumeWave, argumentIntegers.Length);
		Array.Copy(argumentIntegers, this._wave, argumentIntegers.Length);
	}

    private ushort[] _originalVolumeWave { get; set; }

    private ushort[] _wave { get; set; }

    public int Volume { get; private set; }

    public int Length
    {
        get => this._wave.Length;
    }

    public void ChangeVolume(int volume)
	{
		volume = volume < 0 ? 0 : volume;
		volume = volume > 100 ? 100 : volume;
		this.Volume = volume;
		for (int i = 0; i < _wave.Length; i++)
		{
			_wave[i] = (byte)(_originalVolumeWave[i] * volume / 100);
		}
	}

	public void Append(MonauralWave wave)
	{
		this._wave = this._wave.Concat(wave.GetValues()).ToArray();
		this._originalVolumeWave = new ushort[this._wave.Length];
		Array.Copy(this._wave, this._originalVolumeWave, this._wave.Length);
	}

	public byte[] GetBytes(BitRateType bitRate)
	{
        if (bitRate == BitRateType.SixteenBit)
		{
            var result = new List<byte>(this._wave.Length * 2);
			foreach (ushort value in this._wave)
			{
				var bytes = BitConverter.GetBytes((short)((value - short.MaxValue) / 2));
                result.Add(bytes[0]);
                result.Add(bytes[1]);
            }
			return result.ToArray();
        }
		else
		{
            var result = new List<byte>(this._wave.Length);
            foreach (ushort value in this._wave)
            {
                result.Add((byte)(value / 256));
            }
			return result.ToArray();
        }
	}

	public ushort[] GetValues() => this._wave;
}
