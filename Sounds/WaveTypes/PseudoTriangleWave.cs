using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundMaker.Sounds.WaveTypes;
/// <summary>
/// the pseudo triangle wave. 疑似三角波
/// </summary>
public class PseudoTriangleWave : WaveTypeBase
{
    public override ushort[] GenerateWave(SoundFormat format, int tempo, int length, int volume, double hertz)
    {
        this.CheckGenerateWaveArgs(tempo, length, volume, hertz);
        var result = new List<ushort>(length);
        int count = 1;
        // 音の長さまで繰り返す
        while (count <= length)
        {
            // △の波形を作るための繰り返し回数
            double triangleWidth = (int)format.SamplingFrequency / hertz;
            // 32段階で値の大きさを変えるために、上を32で割る。
            int repeatNumber = (int)(triangleWidth / 32d);
            // △最後にできる謎の空白地帯を無くすために、余りを算出する。この余りを各フェーズに1ずつ振り分ける
            int repeatRemainderNumber = (int)triangleWidth % 32;
            if (count + triangleWidth >= length)
            {
                result.Add(0);
                count++;
                continue;
            }
            int phase = 0;
            bool mode = true;
            for (int j = 1; j <= triangleWidth && count <= length; j++, count++)
            {
                ushort sound = (ushort)(ushort.MaxValue * (phase / 15d));
                sound = (ushort)(sound * (volume / 100d));
                result.Add(sound);
                
                if ((repeatRemainderNumber == 0 && j % repeatNumber == 0) || 
                    (repeatRemainderNumber != 0 && j % (repeatNumber + 1) == 0))
                {
                    phase = mode ? phase + 1 : phase - 1;
                    phase = phase == -1 ? 0 : phase;
                    repeatRemainderNumber = repeatRemainderNumber != 0 ? repeatRemainderNumber - 1 : repeatRemainderNumber;
                }
                if (mode && phase == 16)
                {
                    phase = 15;
                    mode = !mode;
                }
            }
        }
        return result.ToArray();
    }
}
