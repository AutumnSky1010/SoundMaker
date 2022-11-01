using SoundMaker.WaveFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundMaker.SoundWave.Score;
public class Tuplet : SoundComponentBase
{
    public Tuplet(IReadOnlyList<ISoundComponent> tupletComponents, LengthType length, bool isDotted = false):base(length, isDotted)
    {
        this.TupletComponents = new List<ISoundComponent>(tupletComponents);
    }

    private IReadOnlyList<ISoundComponent> TupletComponents { get; }

    public int Count => this.TupletComponents.Count;

    public override ushort[] GetSquareWave(FormatChunk format, SquareWaveRatio squareWaveRatio, int tempo)
    {
        var length = this.GetWaveArrayLength(format, tempo);
        return GetSquareWave(format, squareWaveRatio, tempo, length);
    }

    public override ushort[] GetSquareWave(FormatChunk format, SquareWaveRatio squareWaveRatio, int tempo, int length)
    {
        var result = new List<ushort>(length);
        // コンポーネントの数の分で割って、商を求める
        var componentLength = length / this.Count;
        int i;
        for (i = 0; i < this.Count - 1; i++)
        {
            result.AddRange(this.TupletComponents[i].GetSquareWave(format, squareWaveRatio, tempo, componentLength));
        }
        var lastComponentLength = componentLength + length % this.Count;
        result.AddRange(this.TupletComponents[i].GetSquareWave(format, squareWaveRatio, tempo, lastComponentLength));
        return result.ToArray();
    }

    public override ushort[] GetTriangleWave(FormatChunk format, int tempo)
    {
        var length = this.GetWaveArrayLength(format, tempo);
        return GetTriangleWave(format, tempo, length);
    }

    public override ushort[] GetTriangleWave(FormatChunk format, int tempo, int length)
    {
        var result = new List<ushort>(length);
        // コンポーネントの数の分で割って、商を求める
        var componentLength = length / this.Count;
        int i;
        for (i = 0; i < length - 1; i++)
        {
            result.AddRange(this.TupletComponents[i].GetTriangleWave(format, tempo, componentLength));
        }
        var lastComponentLength = componentLength + length % this.Count;
        result.AddRange(this.TupletComponents[i].GetTriangleWave(format, tempo, lastComponentLength));
        return result.ToArray();
    }
}
