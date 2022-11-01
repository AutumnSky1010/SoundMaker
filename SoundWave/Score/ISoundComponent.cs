using SoundMaker.WaveFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundMaker.SoundWave.Score;
public interface ISoundComponent
{
    public LengthType Length { get; }

    public bool IsDotted { get; }

    public int GetWaveArrayLength(FormatChunk format, int tempo);

    public ushort[] GetSquareWave(FormatChunk format, SquareWaveRatio squareWaveRatio, int tempo);

    public ushort[] GetTriangleWave(FormatChunk format, int tempo);

    public ushort[] GetSquareWave(FormatChunk format, SquareWaveRatio squareWaveRatio, int tempo, int length);

    public ushort[] GetTriangleWave(FormatChunk format, int tempo, int length);
}
