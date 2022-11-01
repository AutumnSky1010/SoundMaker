using SoundMaker.WaveFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundMaker.SoundWave.Score;
public class Rest : SoundComponentBase
{
	public Rest(int length, bool isDotted = false)
		: base((LengthType)length, isDotted) { }

	public override ushort[] GetSquareWave(FormatChunk format, SquareWaveRatio squareWaveRatio, int tempo)
		=> GetWave(format, tempo);

	public override ushort[] GetSquareWave(FormatChunk format, SquareWaveRatio squareWaveRatio, int tempo, int length)
		=> GetWave(format, tempo);

	public override ushort[] GetTriangleWave(FormatChunk format, int tempo)
		=> GetWave(format, tempo);

	public override ushort[] GetTriangleWave(FormatChunk format, int tempo, int length)
		=> GetWave(format, tempo, length);

	private ushort[] GetWave(FormatChunk format, int tempo)
	{
		int length = this.GetWaveArrayLength(format, tempo);
		return GetWave(format, tempo, length);
	}

	private ushort[] GetWave(FormatChunk format, int tempo, int length)
		=> Enumerable.Repeat<ushort>(0, length).ToArray();
}
