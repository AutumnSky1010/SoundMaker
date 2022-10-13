using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundMaker.WaveFile;
public enum ChannelType : ushort
{
	Monaural = 0x0001,
	Stereo = 0x0002
}
public enum BitRateType : ushort
{
	SixteenBit = 0x0010,
	EightBit = 0x0008
}
public struct FormatChunk : IChunk
{
	public FormatChunk(BitRateType bitRate, ChannelType type)
	{
		this._channel = (ushort)type;
		this.BitRate = (ushort)bitRate;
		this._blockSize = (ushort)(this.BitRate * this._channel / 8);
		this._byteSizePerSecond = this._blockSize * this.SamplingFrequency;
	}

	private uint _chankSize { get; } = 0x00000010;

	private ushort _soundFormat { get; } = 0x0001;

	private ushort _channel { get; }

	public uint SamplingFrequency { get; } = 0x0000AC44;

	private uint _byteSizePerSecond { get; }

	private ushort _blockSize { get; }

	public ushort BitRate { get; }

	public byte[] GetBytes()
	{
		var result = BitConverter.GetBytes(0x20746D66);
		result = result.Concat(BitConverter.GetBytes(this._chankSize)).ToArray();
		result = result.Concat(BitConverter.GetBytes(this._soundFormat)).ToArray();
		result = result.Concat(BitConverter.GetBytes(this._channel)).ToArray();
		result = result.Concat(BitConverter.GetBytes(this.SamplingFrequency)).ToArray();
		result = result.Concat(BitConverter.GetBytes(this._byteSizePerSecond)).ToArray();
		result = result.Concat(BitConverter.GetBytes(this._blockSize)).ToArray();
		return result.Concat(BitConverter.GetBytes(this.BitRate)).ToArray();
	}
}
