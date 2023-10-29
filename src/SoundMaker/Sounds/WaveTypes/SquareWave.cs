using SoundMaker.Sounds.SoundChannels;

namespace SoundMaker.Sounds.WaveTypes;
/// <summary>
/// the square wave. 矩形波
/// </summary>
public class SquareWave : WaveTypeBase
{
    /// <summary>
    /// constructor. コンストラクタ
    /// </summary>
    /// <param name="squareWaveRatio">duty cycle. デューティ比</param>
    public SquareWave(SquareWaveRatio squareWaveRatio)
    {
        this.SquareWaveRatio = squareWaveRatio;
    }

    private SquareWaveRatio SquareWaveRatio { get; }

    /// <summary>
    /// デューティ比を基に繰り返し回数を求める為の値
    /// </summary>
    private List<(double, double)> Ratio { get; } = new List<(double, double)>
    {
        (0.875, 0.125),
        (0.75, 0.25),
        (0.5, 0.5),
    };

    [Obsolete("Use 'GenerateWave(SoundFormat format, int length, int volume, double hertz)'")]
    public override ushort[] GenerateWave(SoundFormat format, int tempo, int length, int volume, double hertz)
    {
        this.CheckGenerateWaveArgs(tempo, length, volume, hertz);
        return this.GenerateWave(format, length, volume, hertz);
    }

    public override ushort[] GenerateWave(SoundFormat format, int length, int volume, double hertz)
    {
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
        int ratioIndex = (int)this.SquareWaveRatio;
        int allRepeatTimes = (int)((int)format.SamplingFrequency / hertz);
        int firstRepeatTimes = (int)(allRepeatTimes * this.Ratio[ratioIndex].Item1);
        // なぜか配列よりリストの方が早い
        var result = new List<ushort>(allRepeatTimes);
        // 音量の倍率(1.00 ~ 0.00)
        double volumeMagnification = volume / 100d;

        for (int i = 0; i < firstRepeatTimes; i++)
        {
            result.Add(0);
        }
        for (int i = 0; i < allRepeatTimes - firstRepeatTimes; i++)
        {
            ushort sound = (ushort)(ushort.MaxValue * volumeMagnification);
            result.Add(sound);
        }
        return result;
    }
}
