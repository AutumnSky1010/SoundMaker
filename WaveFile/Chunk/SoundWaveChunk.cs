using SoundMaker.SoundWave;

namespace SoundMaker.WaveFile;
public class SoundWaveChunk : IChunk
{
	public SoundWaveChunk(IWave soundWave, BitRateType bitRate)
	{
		this._soundWaveData = soundWave.GetBytes(bitRate);
		this._size = (uint)this._soundWaveData.Length;
	}
	private uint _size;
	public uint Size
	{
		get { return _size; }
	}
	private byte[] _soundWaveData { get; }
	public byte[] GetBytes()
	{
		var result = BitConverter.GetBytes(0x61746164);
		result = result.Concat(BitConverter.GetBytes(this._size)).ToArray();
		return result.Concat(this._soundWaveData).ToArray();
	} 
}
