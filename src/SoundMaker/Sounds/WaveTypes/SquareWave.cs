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
        SquareWaveRatio = squareWaveRatio;
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

    public override short[] GenerateWave(SoundFormat format, int length, int volume, double hertz)
    {
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

    internal override WaveTypeBase Clone()
    {
        return new SquareWave(SquareWaveRatio);
    }

    private List<short> GenerateUnitWave(SoundFormat format, int volume, double hertz)
    {
        var ratioIndex = (int)SquareWaveRatio;
        var allRepeatTimes = (int)((int)format.SamplingFrequency / hertz);
        var firstRepeatTimes = (int)(allRepeatTimes * Ratio[ratioIndex].Item1);
        // なぜか配列よりリストの方が早い
        var result = new List<short>(allRepeatTimes);
        // 音量の倍率(1.00 ~ 0.00)
        var volumeMagnification = volume / 100d;

        var top = (short)(short.MaxValue * volumeMagnification);
        var bottom = (short)(-top);

        for (var i = 0; i < firstRepeatTimes; i++)
        {
            result.Add(top);
        }
        for (var i = 0; i < allRepeatTimes - firstRepeatTimes; i++)
        {
            result.Add(bottom);
        }
        return result;
    }
}
