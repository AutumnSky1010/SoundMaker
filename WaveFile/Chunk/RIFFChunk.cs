
namespace SoundMaker.WaveFile;
public struct RIFFChunk : IChunk
{
	public RIFFChunk(uint Size)
	{
		this._size = Size;
	}

	private uint _size { get; }
	private uint _format { get; } = 0x45564157;
	public byte[] GetBytes()
	{
		var result = BitConverter.GetBytes(0x46464952);
		result = result.Concat(BitConverter.GetBytes(this._size)).ToArray();
		return result.Concat(BitConverter.GetBytes(this._format)).ToArray();
	}
}
