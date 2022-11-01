using SoundMaker.WaveFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundMaker.SoundWave.Score;
public abstract class SoundComponentBase : ISoundComponent
{
    public SoundComponentBase(LengthType length, bool isDotted)
    {
        this.Length = length;
        this.IsDotted = isDotted;
    }

    public LengthType Length { get; }

    public bool IsDotted { get; }

    public int WaveArrayLength { get; }

    public abstract ushort[] GetSquareWave(FormatChunk format, SquareWaveRatio squareWaveRatio, int tempo);

    public abstract ushort[] GetSquareWave(FormatChunk format, SquareWaveRatio squareWaveRatio, int tempo, int length);

    public abstract ushort[] GetTriangleWave(FormatChunk format, int tempo);

    public abstract ushort[] GetTriangleWave(FormatChunk format, int tempo, int length);

    public int GetWaveArrayLength(FormatChunk format, int tempo)
    {
        return this.GetWaveArrayLength(format, tempo, this.Length, this.IsDotted);
    }

    private int GetWaveArrayLength(FormatChunk format, int tempo, LengthType length, bool isDotted)
    {
        // 128分音符・休符の長さを求める（最小単位なので、これを掛け算すれば正確な長さを出せる。）
        // 一個分の長さ＝サンプリング周波数 * 60秒 / (テンポ(一分間に四分音符何個か) * 32(四分音符の長さを32で割ると128分音符))
        int baseNoteLength = (int)(format.SamplingFrequency * 60 / (tempo * 32d));
        // リストの長さ = 128分音符一個分の長さ * (128 / 長さ)　
        // 例) 128 / 4 = 32より、四分音符一個分の長さは、128分音符32個分の長さ。
        int listLength = baseNoteLength * (128 / (int)this.Length);
        if (this.IsDotted)
        {
            listLength += baseNoteLength * (128 / (int)this.Length / 2);
        }
        return listLength;
    }
}
