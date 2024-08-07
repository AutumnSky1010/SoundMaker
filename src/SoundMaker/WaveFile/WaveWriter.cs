﻿namespace SoundMaker.WaveFile;
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
        // 実際にRIFFチャンクに書き込むのは、(ファイル全体サイズ - "WAVE"の文字列の大きさである8B)になる
        Chunks.Add(new RIFFChunk(soundWave.Size + 36));
        Chunks.Add(format);
        Chunks.Add(soundWave);
    }

    private List<IChunk> Chunks { get; } = new(3);

    /// <summary>
    /// write to .wav file. .wavファイルに書き込む
    /// </summary>
    /// <param name="path">path of .wav file.</param>
    public void Write(string path)
    {
        using var stream = new FileStream(path, FileMode.Create, FileAccess.Write);
        Write(stream);
    }

    /// <summary>
    /// write to stream. ストリームに書き込む
    /// </summary>
    /// <param name="stream">stream</param>
    public void Write(Stream stream)
    {
        var writer = new BinaryWriter(stream);
        foreach (var chunk in Chunks)
        {
            writer.Write(chunk.GetBytes());
        }
    }
}
