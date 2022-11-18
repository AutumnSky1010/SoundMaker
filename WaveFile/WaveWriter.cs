namespace SoundMaker.WaveFile;
/// <summary>
/// write to the .wav file. waveファイルに書き込む
/// </summary>
public class WaveWriter
{
    /// <summary>
    /// constructor. コンストラクタ
    /// </summary>
    /// <param name="format">format chunk. フォーマットチャンク</param>
    /// <param name="soundWave">sound wave chunk. 音声波形のチャンク</param>
    public WaveWriter(FormatChunk format, SoundWaveChunk soundWave)
    {
        // ファイル全体サイズ = 音声波形データ + 44B
        this.Chunks.Add(new RIFFChunk(soundWave.Size + 36));
        this.Chunks.Add(format);
        this.Chunks.Add(soundWave);
    }

    private List<IChunk> Chunks { get; } = new(3);
    
    /// <summary>
    /// write to .wav file. .wavファイルに書き込む
    /// </summary>
    /// <param name="path">path of .wav file.</param>
    public void Write(string path)
    {
        using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write))
        using (var writer = new BinaryWriter(stream))
        {
            foreach (var chunk in this.Chunks)
            {
                writer.Write(chunk.GetBytes());
            }
        }
    }
}
