namespace SoundMaker.WaveFile;

/// <summary>
/// Write to the .wav file. <br/>waveファイルに書き込む
/// </summary>
public class WaveWriter
{
    /// <summary>
    /// Constructor. <br/>コンストラクタ
    /// </summary>
    /// <param name="format">Format chunk. <br/>フォーマットチャンク</param>
    /// <param name="soundWave">Sound wave chunk. <br/>音声波形のチャンク</param>
    public WaveWriter(FormatChunk format, SoundWaveChunk soundWave)
    {
        // Total file size = audio wave data + 44B
        // ファイル全体サイズ = 音声波形データ + 44B
        // The actual size written to the RIFF chunk is (total file size - the size of the "WAVE" string, which is 8B).
        // 実際にRIFFチャンクに書き込むのは、(ファイル全体サイズ - "WAVE"の文字列の大きさである8B)になる
        Chunks.Add(new RIFFChunk(soundWave.Size + 36));
        Chunks.Add(format);
        Chunks.Add(soundWave);
    }

    private List<IChunk> Chunks { get; } = new(3);

    /// <summary>
    /// Write to .wav file. <br/>.wavファイルに書き込む
    /// </summary>
    /// <param name="path">Path of .wav file. <br/>.wavファイルのパス</param>
    public void Write(string path)
    {
        using var stream = new FileStream(path, FileMode.Create, FileAccess.Write);
        Write(stream);
    }

    /// <summary>
    /// Write to stream. <br/>ストリームに書き込む
    /// </summary>
    /// <param name="stream">Stream. <br/>ストリーム</param>
    public void Write(Stream stream)
    {
        var writer = new BinaryWriter(stream);
        foreach (var chunk in Chunks)
        {
            writer.Write(chunk.GetBytes());
        }
    }
}
