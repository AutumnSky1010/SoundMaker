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
            byte[] rightWaveByte = wave.GetRightBytes();
            byte[] leftWaveByte = wave.GetLeftBytes();
            int minLength = rightWaveByte.Length < leftWaveByte.Length ? rightWaveByte.Length : leftWaveByte.Length;
            for (int i = 0; i < minLength; i++)
            {
                resultRightIntList[i] = resultRightIntList[i] + rightWaveByte[i];
                resultLeftIntList[i] = resultLeftIntList[i] + leftWaveByte[i];
            }
            if (minLength == rightWaveByte.Length)
            {
                for (int i = minLength; i < leftWaveByte.Length; i++)
                {
                    resultLeftIntList[i] = resultLeftIntList[i] + leftWaveByte[i];
                }
            }
            else
            {
                for (int i = minLength; i < rightWaveByte.Length; i++)
                {
                    resultRightIntList[i] = resultRightIntList[i] + rightWaveByte[i];
                }
            }
        }
        return new StereoWave(
            resultRightIntList.ConvertAll<byte>((x) => (byte)(x / (ulong)this._waves.Count)),
            resultLeftIntList.ConvertAll<byte>((x) => (byte)(x / (ulong)this._waves.Count)));
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
