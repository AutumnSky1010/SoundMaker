namespace SoundMaker.Sounds.WaveTypes;
/// <summary>
/// the triangle wave. 三角波
/// </summary>
public class TriangleWave : WaveTypeBase
{
    public override short[] GenerateWave(SoundFormat format, int length, int volume, double hertz)
    {
        CheckGenerateWaveArgs(length, volume, hertz);
        var result = new List<short>(length);
        var unitWave = GenerateUnitWaveInternal(format, volume, hertz);
        for (var i = 0; i < length / unitWave.Length; i++)
        {
            result.AddRange(unitWave);
        }
        for (var i = 0; i < length % unitWave.Length; i++)
        {
            result.Add(0);
        }
        return result.ToArray();
    }

    internal override WaveTypeBase Clone()
    {
        return new TriangleWave();
    }

    /// <summary>
    /// Generates one cycle of a sound waveform at the specified frequency.<br/>
    /// 指定した周波数の音声波形1周期分を生成する。
    /// </summary>
    /// <param name="format">Format of the sound. <br/>音のフォーマット</param>
    /// <param name="volume">Volume <br/>音量（0 ~ 100）</param>
    /// <param name="hertz">Hertz of the sound. <br/>音の周波数</param>
    /// <returns>The array of wave data.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Hertz must be non-negative and greater than 0.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Volume must be below 100 and above 0.</exception>
    public short[] GenerateUnitWave(SoundFormat format, int volume, double hertz)
    {
        CheckGenerateUnitWaveArgs(volume, hertz);
        return GenerateUnitWaveInternal(format, volume, hertz);
    }

    private static short[] GenerateUnitWaveInternal(SoundFormat format, int volume, double hertz)
    {
        var repeatNumber = (int)((int)format.SamplingFrequency / hertz);

        var result = new short[repeatNumber];


        var halfCount = repeatNumber >> 1;
        var quarterCount = halfCount >> 1;

        var slope = ushort.MaxValue / halfCount;

        var volumeMagnification = volume / 100d;
        // halfCount基準で中央のオフセット(lengthが奇数の場合1)
        var midOffsetFromHalfCount = repeatNumber % 2;
        for (int i = 0; i < quarterCount; i++)
        {
            var sound = (short)(0 + slope * (i + 1));
            sound = (short)(sound * volumeMagnification);
            result[i] = sound;
            result[halfCount - i - 1] = sound;

            var negativeSound = (short)(-sound);
            result[result.Length - i - 1] = negativeSound;
            result[halfCount + i + midOffsetFromHalfCount] = negativeSound;
        }

        if (halfCount % 2 != 0)
        {
            result[quarterCount] = (short)(short.MaxValue * volumeMagnification);
            result[halfCount + quarterCount + midOffsetFromHalfCount] = (short)(short.MinValue * volumeMagnification);
        }

        return result;
    }
}
