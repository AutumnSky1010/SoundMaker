namespace SoundMaker.WaveFile;
/// <summary>
/// Chunk of the sound wave. <br/>波形データのチャンクを表すクラス
/// </summary>
public class SoundWaveChunk : IChunk
{
    /// <summary>
    /// Constructor. <br/>コンストラクタ
    /// </summary>
    /// <param name="soundWave">音の波形データの配列</param>
    public SoundWaveChunk(byte[] soundWave)
    {
        SoundWaveData = soundWave;
        Size = (uint)SoundWaveData.Length;
    }

    /// <summary>
    /// Size of the wave data. <br/>波形データのサイズ
    /// </summary>
    public uint Size { get; }

    private byte[] SoundWaveData { get; }

    public byte[] GetBytes()
    {
        // 0x61746164 は dataになる
        var result = BitConverter.GetBytes(0x61746164);
        result = result.Concat(BitConverter.GetBytes(Size)).ToArray();
        return result.Concat(SoundWaveData).ToArray();
    }
}
