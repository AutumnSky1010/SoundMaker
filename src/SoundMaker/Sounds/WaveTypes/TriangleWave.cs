namespace SoundMaker.Sounds.WaveTypes;
/// <summary>
/// the triangle wave. 三角波
/// </summary>
public class TriangleWave : WaveTypeBase
{
    [Obsolete("Use 'GenerateWave(SoundFormat format, int length, int volume, double hertz)'")]
    public override ushort[] GenerateWave(SoundFormat format, int tempo, int length, int volume, double hertz)
    {
        this.CheckGenerateWaveArgs(tempo, length, volume, hertz);
        return this.GenerateWave(format, length, volume, hertz);
    }

    public override ushort[] GenerateWave(SoundFormat format, int length, int volume, double hertz)
    {
        CheckGenerateWaveArgs(length, volume, hertz);
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
        double repeatNumber = (int)format.SamplingFrequency / hertz;
        // なぜか配列よりリストの方が早い
        var result = new List<ushort>((int)repeatNumber);
        // 直線の方程式の傾きを求める。
        double slope = ushort.MaxValue / (repeatNumber / 2);
        // 音量の倍率(1.00 ~ 0.00)
        double volumeMagnification = volume / 100d;

        // 傾き正波形
        for (int i = 1; i <= repeatNumber / 2; i++)
        {
            ushort sound = (ushort)(slope * i);
            sound = (ushort)(sound * volumeMagnification);
            result.Add(sound);
        }
        // 傾き負波形
        slope = -slope;
        for (int i = (int)(repeatNumber / 2 + 1); i <= repeatNumber; i++)
        {
            ushort sound = (ushort)(ushort.MaxValue + slope * i);
            sound = (ushort)(sound * volumeMagnification);
            result.Add(sound);
        }
        return result;
    }
}
