using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SoundMaker.Sounds.WaveTypes;
/// <summary>
/// the pseudo triangle wave. 疑似三角波
/// </summary>
public class PseudoTriangleWave : WaveTypeBase
{
    [Obsolete("Use 'GenerateWave(SoundFormat format, int length, int volume, double hertz)'")]
    public override ushort[] GenerateWave(SoundFormat format, int tempo, int length, int volume, double hertz)
    {
        this.CheckGenerateWaveArgs(tempo, length, volume, hertz);
        return this.GenerateWave(format, length, volume, hertz);
    }

    public override ushort[] GenerateWave(SoundFormat format, int length, int volume, double hertz)
    {
        this.CheckGenerateWaveArgs(length, volume, hertz);

        // △の波形を作るための繰り返し回数
        int triangleWidth = (int)((int)format.SamplingFrequency / hertz);
        // 疑似三角波に出来ない場合は普通の三角波を生成する。
        if (triangleWidth <= 64)
        {
            return new TriangleWave().GenerateWave(format, length, volume, hertz);
        }

        var result = new List<ushort>(length);
        var unitWave = GenerateUnitWave(format, volume, hertz);
        for (int i = 0; i < length / unitWave.Count; i++)
        {
            result.AddRange(unitWave);
        }
        for (int i = 0; i < length % unitWave.Count; i++)
        {
            result.Add(0);
        }
        return result.ToArray();
    }

    private List<ushort> GenerateUnitWave(SoundFormat format, int volume, double hertz)
    {
        int repeatNumber = (int)((int)format.SamplingFrequency / hertz);
        // なぜか配列よりリストの方が早い
        var result = new List<ushort>((int)repeatNumber);
        // 音量の倍率(1.00 ~ 0.00)
        double volumeMagnification = volume / 100d;
        // 階段の幅
        int stairsWidth = repeatNumber / 32;
        // 幅の余り
        int r = repeatNumber % 32;

        // 上り波形
        for (int i = 0; i < 16; i++)
        {
            ushort sound = (ushort)(ushort.MaxValue * (i / 15d));
            sound = (ushort)(sound * volumeMagnification);
            for (int j = 0; j < stairsWidth; j++)
            {
                result.Add(sound);
            }
            // 余りがある場合は足す。波形の後ろ側の部分に足す
            // ex. r = 31の場合、i = 1 ~ 15の時足す。
            if (i <= r / 2 && i != 0)
            {
                result.Add(sound);
            }
        }
        // 下り波形
        // 下り波形では上り波形より数値を減らす
        ushort negativeDiff = (ushort)(ushort.MaxValue * (1 / 30d));
        for (int i = 15; i >= 0; i--)
        {
            ushort sound = (ushort)(ushort.MaxValue * (i / 15d));
            sound = sound != 0 ? (ushort)(sound - negativeDiff) : sound;
            sound = (ushort)(sound * volumeMagnification);
            for (int j = 0; j < stairsWidth; j++)
            {
                result.Add(sound);
            }
            // 余りがある場合は足す。波形の後ろ側の部分に足す
            // ex. r = 31の場合、i = 15 ~ 0の時足す。
            if (i < r / 2 + r % 2)
            {
                result.Add(sound);
            }
        }
        return result;
    }
}
