namespace SoundMaker.Sounds.WaveTypes;
/// <summary>
/// the triangle wave. 三角波
/// </summary>
public class TriangleWave : WaveTypeBase
{
    [Obsolete("Use 'GenerateWave(SoundFormat format, int length, int volume, double hertz)'")]
    public override short[] GenerateWave(SoundFormat format, int tempo, int length, int volume, double hertz)
    {
        CheckGenerateWaveArgs(tempo, length, volume, hertz);
        return GenerateWave(format, length, volume, hertz);
    }

    public override short[] GenerateWave(SoundFormat format, int length, int volume, double hertz)
    {
        CheckGenerateWaveArgs(length, volume, hertz);
        var result = new List<short>(length);
        var unitWave = GenerateUnitWave(format, volume, hertz);
        for (var i = 0; i < length / unitWave.Count; i++)
        {
            result.AddRange(unitWave);
        }
        for (var i = 0; i < length % unitWave.Count; i++)
        {
            result.Add(0);
        }
        return result.ToArray();
    }

    private List<short> GenerateUnitWave(SoundFormat format, int volume, double hertz)
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
            result[quarterCount] = short.MaxValue;
            result[halfCount + quarterCount + midOffsetFromHalfCount] = short.MinValue;
        }

        return result.ToList();
    }
}
