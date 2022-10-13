using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundMaker.SoundWave;
internal class StereoMixer
{
    private IReadOnlyList<StereoWave> _waves { get; }
    public StereoMixer(IReadOnlyList<StereoWave> waves)
    {
        this._waves = waves;
    }

    public StereoWave Mix()
    {
        int maxLength = this.GetMaxLength();
        var resultRightIntList = Enumerable.Repeat<ulong>(0, maxLength).ToList();
        var resultLeftIntList = new List<ulong>(resultRightIntList);

        foreach (var wave in this._waves)
        {
            ushort[] rightWave = wave.GetRightWave();
            ushort[] leftWave = wave.GetLeftWave();
            int minLength = rightWave.Length < leftWave.Length ? rightWave.Length : leftWave.Length;
            for (int i = 0; i < minLength; i++)
            {
                resultRightIntList[i] = resultRightIntList[i] + rightWave[i];
                resultLeftIntList[i] = resultLeftIntList[i] + leftWave[i];
            }
            if (minLength == rightWave.Length)
            {
                for (int i = minLength; i < leftWave.Length; i++)
                {
                    resultLeftIntList[i] = resultLeftIntList[i] + leftWave[i];
                }
            }
            else
            {
                for (int i = minLength; i < rightWave.Length; i++)
                {
                    resultRightIntList[i] = resultRightIntList[i] + rightWave[i];
                }
            }
        }
        return new StereoWave(
            resultRightIntList.ConvertAll<ushort>((x) => (ushort)(x / (ulong)this._waves.Count)),
            resultLeftIntList.ConvertAll<ushort>((x) => (ushort)(x / (ulong)this._waves.Count)));
    }

    private int GetMaxLength()
    {
        int maxLength = 0;
        foreach (var wave in this._waves)
        {
            maxLength = wave.Length >= maxLength ? wave.Length : maxLength;
        }
        return maxLength;
    }
}
