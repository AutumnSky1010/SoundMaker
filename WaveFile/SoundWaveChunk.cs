namespace SoundMaker.WaveFile;
/// <summary>
/// 波形データのチャンクを表すクラス
/// </summary>
public class SoundWaveChunk : IChunk
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="soundWave">音の波形データの配列</param>
    /// <param name="bitRate">量子化ビット数</param>
    public SoundWaveChunk(byte[] soundWave, BitRateType bitRate)
    {
        this._soundWaveData = soundWave;
        this._size = (uint)this._soundWaveData.Length;
    }

    private uint _size;

    /// <summary>
    /// 波形データのサイズ
    /// </summary>
    public uint Size
    {
        get { return this._size; }
    }

    private byte[] _soundWaveData { get; }

    public byte[] GetBytes()
    {
        var result = BitConverter.GetBytes(0x61746164);
        result = result.Concat(BitConverter.GetBytes(this._size)).ToArray();
        return result.Concat(this._soundWaveData).ToArray();
    }
}
