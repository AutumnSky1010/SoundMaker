using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundMaker.SoundWave;
public class MonauralWave : IWave
{
	public MonauralWave(IReadOnlyCollection<byte> waveBytes)
	{
		byte[] argumentBytes = waveBytes.ToArray();
		this._originalVolumeWaveBytes = new byte[waveBytes.Count];
		this._waveBytes = new byte[waveBytes.Count];
		Array.Copy(argumentBytes, this._originalVolumeWaveBytes, argumentBytes.Length);
		Array.Copy(argumentBytes, this._waveBytes, argumentBytes.Length);
	}

    private byte[] _originalVolumeWaveBytes { get; set; }

    private byte[] _waveBytes { get; set; }

    public int Volume { get; private set; }

    public int Length
    {
        get => this._waveBytes.Length;
    }

    public void ChangeVolume(int volume)
	{
		volume = volume < 0 ? 0 : volume;
		volume = volume > 100 ? 100 : volume;
		this.Volume = volume;
		for (int i = 0; i < _waveBytes.Length; i++)
		{
			_waveBytes[i] = (byte)(_originalVolumeWaveBytes[i] * volume / 100);
		}
	}

	public void Merge(MonauralWave wave)
	{
		this._waveBytes = this._waveBytes.Concat(wave.GetBytes()).ToArray();
		this._originalVolumeWaveBytes = new byte[this._waveBytes.Length];
		Array.Copy(this._waveBytes, this._originalVolumeWaveBytes, this._waveBytes.Length);
	}

	public byte[] GetBytes() => this._waveBytes;
}
