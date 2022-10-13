using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundMaker.SoundWave;

public enum WaveChannelType
{
	BOTH,
	RIGHT,
	LEFT,
}
public class StereoWave : IWave
{
	public StereoWave(IReadOnlyCollection<byte> rightWaveBytes, IReadOnlyCollection<byte> leftWaveBytes)
	{
        byte[] rightArgumentBytes = rightWaveBytes.ToArray();
        byte[] leftArgumentBytes = leftWaveBytes.ToArray();

        this._rightOriginalVolumeWaveBytes = new byte[rightWaveBytes.Count];
        this._leftOriginalVolumeWaveBytes = new byte[leftWaveBytes.Count];
		Array.Copy(rightArgumentBytes, this._rightOriginalVolumeWaveBytes, rightArgumentBytes.Length);
		Array.Copy(leftArgumentBytes, this._leftOriginalVolumeWaveBytes, leftArgumentBytes.Length);

        this._rightWaveBytes = new byte[rightWaveBytes.Count];
        this._leftWaveBytes = new byte[leftWaveBytes.Count];
        Array.Copy(rightArgumentBytes, this._rightWaveBytes, rightArgumentBytes.Length);
        Array.Copy(leftArgumentBytes, this._leftWaveBytes, leftArgumentBytes.Length);
    }
	
	private byte[] _rightOriginalVolumeWaveBytes { get; set; }

	private byte[] _leftOriginalVolumeWaveBytes { get; set; }

	private byte[] _rightWaveBytes { get; set; }

	private byte[] _leftWaveBytes { get; set; }

    public int Volume { get; private set; }

	public int Length
	{
		get => this.GetMaxAndMinWaveLength().Max;
	}

	public void ChangeVolume(int volume, WaveChannelType channelType)
	{
        volume = volume < 0 ? 0 : volume;
        volume = volume > 100 ? 100 : volume;
		switch (channelType)
		{
			case WaveChannelType.LEFT:
				for (int i = 0; i < this._leftWaveBytes.Length; i++)
				{
					this._leftWaveBytes[i] = (byte)(this._leftOriginalVolumeWaveBytes[i] * volume / 100); 
				}
				break;
			case WaveChannelType.RIGHT:
                for (int i = 0; i < this._rightWaveBytes.Length; i++)
                {
                    this._rightWaveBytes[i] = (byte)(this._rightOriginalVolumeWaveBytes[i] * volume / 100);
                }
				break;
			default:
				var maxAndMinLength = this.GetMaxAndMinWaveLength();
				for (int i = 0; i < maxAndMinLength.Min; i++)
				{
                    this._rightWaveBytes[i] = (byte)(this._rightOriginalVolumeWaveBytes[i] * volume / 100);
                    this._leftWaveBytes[i] = (byte)(this._leftOriginalVolumeWaveBytes[i] * volume / 100);
                }

				// 残りを処理する。
				byte[] wave = this._rightWaveBytes.Length == maxAndMinLength.Max ? this._rightWaveBytes : this._leftWaveBytes;
				byte[] originalWave = this._rightWaveBytes.Length == maxAndMinLength.Max ? 
					this._rightOriginalVolumeWaveBytes : this._leftOriginalVolumeWaveBytes;
                for (int i = maxAndMinLength.Min; i < maxAndMinLength.Max; i++)
				{
					wave[i] = (byte)(originalWave[i] * volume / 100);
				}
				break;
        }
    }

	public byte[] GetBytes()
	{
        var maxAndMinLength = this.GetMaxAndMinWaveLength();
		var resultWave = new List<byte>(maxAndMinLength.Max * 2);
		for (int i = 0; i < maxAndMinLength.Min; i++)
		{
            // Point : ステレオの波は左右左右左右左右・・・・・・左右
            resultWave.Add(this._leftWaveBytes[i]);
			resultWave.Add(this._rightWaveBytes[i]);
		}
		
		// 追加しきれていない波形データを追加する
		if (this._leftWaveBytes.Length == maxAndMinLength.Max)
		{
            for (int i = maxAndMinLength.Min; i < maxAndMinLength.Max; i++)
            {
				resultWave.Add(this._leftWaveBytes[i]);
				resultWave.Add(0);
            }
        }
		else
		{
            for (int i = maxAndMinLength.Min; i < maxAndMinLength.Max; i += 2)
            {
                resultWave.Add(0);
                resultWave.Add(this._rightWaveBytes[i]);
            }
        }
		return resultWave.ToArray();
    }

	public byte[] GetRightBytes()
	{
		var resultBytes = new byte[this._rightWaveBytes.Length];
		Array.Copy(this._rightWaveBytes, resultBytes, this._rightWaveBytes.Length);
		return resultBytes;
	}

    public byte[] GetLeftBytes()
    {
        var resultBytes = new byte[this._leftWaveBytes.Length];
        Array.Copy(this._leftWaveBytes, resultBytes, this._leftWaveBytes.Length);
        return resultBytes;
    }

    public void Merge(StereoWave wave)
	{
		throw new NotImplementedException();
	}

	private MaxAndMin GetMaxAndMinWaveLength()
	{
        if (this._rightWaveBytes.Length > this._leftWaveBytes.Length)
        {
			return new MaxAndMin(this._rightWaveBytes.Length, this._leftWaveBytes.Length);
        }
		return new MaxAndMin(this._leftWaveBytes.Length, this._rightWaveBytes.Length);
    }

	private struct MaxAndMin
	{
		public MaxAndMin(int max, int min)
		{
			this.Min = min;
			this.Max = max;
		}

		public int Max { get; }

		public int Min { get; }
	}
}
