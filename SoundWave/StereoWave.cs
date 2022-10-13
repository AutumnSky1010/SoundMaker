using SoundMaker.WaveFile;
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
	public StereoWave(IReadOnlyCollection<ushort> rightWave, IReadOnlyCollection<ushort> leftWave)
	{
        ushort[] rightArgument = rightWave.ToArray();
        ushort[] leftArgument = leftWave.ToArray();

        this._rightOriginalVolumeWave = new ushort[rightWave.Count];
        this._leftOriginalVolumeWave = new ushort[leftWave.Count];
		Array.Copy(rightArgument, this._rightOriginalVolumeWave, rightArgument.Length);
		Array.Copy(leftArgument, this._leftOriginalVolumeWave, leftArgument.Length);

        this._rightWave = new ushort[rightWave.Count];
        this._leftWave = new ushort[leftWave.Count];
        Array.Copy(rightArgument, this._rightWave, rightArgument.Length);
        Array.Copy(leftArgument, this._leftWave, leftArgument.Length);
    }
	
	private ushort[] _rightOriginalVolumeWave { get; set; }

	private ushort[] _leftOriginalVolumeWave { get; set; }

	private ushort[] _rightWave { get; set; }

	private ushort[] _leftWave { get; set; }

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
				for (int i = 0; i < this._leftWave.Length; i++)
				{
					this._leftWave[i] = (ushort)(this._leftOriginalVolumeWave[i] * volume / 100); 
				}
				break;
			case WaveChannelType.RIGHT:
                for (int i = 0; i < this._rightWave.Length; i++)
                {
                    this._rightWave[i] = (ushort)(this._rightOriginalVolumeWave[i] * volume / 100);
                }
				break;
			default:
				var maxAndMinLength = this.GetMaxAndMinWaveLength();
				for (int i = 0; i < maxAndMinLength.Min; i++)
				{
                    this._rightWave[i] = (ushort)(this._rightOriginalVolumeWave[i] * volume / 100);
                    this._leftWave[i] = (ushort)(this._leftOriginalVolumeWave[i] * volume / 100);
                }

				// 残りを処理する。
				ushort[] wave = this._rightWave.Length == maxAndMinLength.Max ? this._rightWave : this._leftWave;
				ushort[] originalWave = this._rightWave.Length == maxAndMinLength.Max ? 
					this._rightOriginalVolumeWave : this._leftOriginalVolumeWave;
                for (int i = maxAndMinLength.Min; i < maxAndMinLength.Max; i++)
				{
					wave[i] = (ushort)(originalWave[i] * volume / 100);
				}
				break;
        }
    }

	public byte[] GetBytes(BitRateType bitRate)
	{
		if (bitRate == BitRateType.SixteenBit)
		{
            return this.Get16BitBytes().ToArray();
        }
		else
		{
            return this.Get8BitBytes().ToArray();
        }
    }

    private List<byte> Get8BitBytes()
    {
        var maxAndMinLength = this.GetMaxAndMinWaveLength();
        var resultWave = new List<byte>(maxAndMinLength.Max * 2);
        for (int i = 0; i < maxAndMinLength.Min; i++)
        {
            // Point : ステレオの波は左右左右左右左右・・・・・・左右
            resultWave.Add((byte)(this._leftWave[i] / 256));
            resultWave.Add((byte)(this._rightWave[i] / 256));
        }
        // 追加しきれていない波形データを追加する
        if (this._leftWave.Length == maxAndMinLength.Max)
        {
            for (int i = maxAndMinLength.Min; i < maxAndMinLength.Max; i++)
            {
                resultWave.Add((byte)(this._leftWave[i] / 256));
                resultWave.Add(0);
            }
        }
        else
        {
            for (int i = maxAndMinLength.Min; i < maxAndMinLength.Max; i += 2)
            {
                resultWave.Add(0);
                resultWave.Add((byte)(this._rightWave[i] / 256));
            }
        }
        return resultWave;
    }

	private List<byte> Get16BitBytes()
	{
        var maxAndMinLength = this.GetMaxAndMinWaveLength();
        var resultWave = new List<byte>(maxAndMinLength.Max * 4);
        for (int i = 0; i < maxAndMinLength.Min; i++)
        {
            byte[] leftBytes = BitConverter.GetBytes(this._leftWave[i]);
            byte[] rightBytes = BitConverter.GetBytes(this._rightWave[i]);
            // Point : ステレオの波は左右左右左右左右・・・・・・左右
            resultWave.Add(leftBytes[0]);
            resultWave.Add(leftBytes[1]);
            resultWave.Add(rightBytes[0]);
            resultWave.Add(rightBytes[1]);
        }
        // 追加しきれていない波形データを追加する
        if (this._leftWave.Length == maxAndMinLength.Max)
        {
            for (int i = maxAndMinLength.Min; i < maxAndMinLength.Max; i++)
            {
                byte[] leftBytes = BitConverter.GetBytes(this._leftWave[i]);
                resultWave.Add(leftBytes[0]);
                resultWave.Add(leftBytes[1]);
                resultWave.Add(0);
                resultWave.Add(0);
            }
        }
        else
        {
            for (int i = maxAndMinLength.Min; i < maxAndMinLength.Max; i += 2)
            {
                byte[] rightBytes = BitConverter.GetBytes(this._rightWave[i]);
                resultWave.Add(0);
                resultWave.Add(0);
                resultWave.Add(rightBytes[0]);
                resultWave.Add(rightBytes[1]);
            }
        }
        return resultWave;
    }

	public ushort[] GetRightWave()
	{
		var resultushorts = new ushort[this._rightWave.Length];
		Array.Copy(this._rightWave, resultushorts, this._rightWave.Length);
		return resultushorts;
	}

    public ushort[] GetLeftWave()
    {
        var resultushorts = new ushort[this._leftWave.Length];
        Array.Copy(this._leftWave, resultushorts, this._leftWave.Length);
        return resultushorts;
    }

    public void Merge(StereoWave wave)
	{
		throw new NotImplementedException();
	}

	private MaxAndMin GetMaxAndMinWaveLength()
	{
        if (this._rightWave.Length > this._leftWave.Length)
        {
			return new MaxAndMin(this._rightWave.Length, this._leftWave.Length);
        }
		return new MaxAndMin(this._leftWave.Length, this._rightWave.Length);
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
