using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundMaker.Sounds.WaveTypes;
/// <summary>
/// the triangle wave. 三角波
/// </summary>
public class TriangleWave : WaveTypeBase
{
    public override ushort[] GenerateWave(SoundFormat format, int tempo, int length, int volume, double hertz)
    {
        bool mode = false;
        var result = new List<ushort>(length);
        int count = 1;
        // 音の長さまで繰り返す
        while (count <= length)
        {
            // △の波形の波形を作るための繰り返し回数
            double repeatNumber = (int)format.SamplingFrequency / hertz;
            // 直線の方程式の傾きを求める。
            double slope = ushort.MaxValue / (repeatNumber / 2);
            if (count + repeatNumber >= length)
            {
                result.Add(0);
                count++;
                continue;
            }
            for (int j = 1; j <= repeatNumber && count <= length; j++, count++)
            {
                ushort sound = mode ? (ushort)(slope * j) : (ushort)(ushort.MaxValue + slope * j);
                sound = (ushort)(sound * (volume / 100d));
                result.Add(sound);
                if (j == (int)(repeatNumber / 2))
                {
                    mode = !mode;
                    slope = -slope;
                }
            }
        }
        return result.ToArray();
    }
}
