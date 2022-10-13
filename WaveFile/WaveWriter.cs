using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundMaker.WaveFile;
public class WaveWriter : IWriteable
{
	private List<IChunk> _chunks { get; } = new List<IChunk>(3);
	public WaveWriter(FormatChunk format, SoundWaveChunk soundWave)
	{
		// ファイル全体サイズ = 音声波形データ + 44B
		this._chunks.Add(new RIFFChunk(soundWave.Size + 36));
		this._chunks.Add(format);
		this._chunks.Add(soundWave);
	}
	public void Write(string path)
	{
		using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write))
		using (var writer = new BinaryWriter(stream))
		{
			foreach (var chunk in _chunks)
			{
				writer.Write(chunk.GetBytes());
			}
		}
	}
}
