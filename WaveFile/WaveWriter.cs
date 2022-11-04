namespace SoundMaker.WaveFile;
/// <summary>
/// waveファイルに書き込む
/// </summary>
public class WaveWriter
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="format">フォーマットチャンク</param>
    /// <param name="soundWave">音声波形のチャンク</param>
    public WaveWriter(FormatChunk format, SoundWaveChunk soundWave)
    {
        // ファイル全体サイズ = 音声波形データ + 44B
        this.Chunks.Add(new RIFFChunk(soundWave.Size + 36));
        this.Chunks.Add(format);
        this.Chunks.Add(soundWave);
    }

    private List<IChunk> Chunks { get; } = new(3);

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
