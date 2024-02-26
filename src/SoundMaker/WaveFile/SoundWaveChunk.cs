namespace SoundMaker.WaveFile;
/// <summary>
/// chunk of the sound wave. 波形データのチャンクを表すクラス
/// </summary>
public class SoundWaveChunk : IChunk
{
    /// <summary>
    /// constructor. コンストラクタ
    /// </summary>
    /// <param name="soundWave">音の波形データの配列</param>
    public SoundWaveChunk(byte[] soundWave)
    {
        SoundWaveData = soundWave;
        Size = (uint)SoundWaveData.Length;
    }

    /// <summary>
    /// size of the wave data. 波形データのサイズ
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
