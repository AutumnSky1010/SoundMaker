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
    private static List<(double, double)> Ratio { get; } =
    [
        (0.875, 0.125),
        (0.75, 0.25),
        (0.5, 0.5),
    ];

    public override short[] GenerateWave(SoundFormat format, int length, int volume, double hertz)
    {
        var result = new List<short>(length);
        var unitWave = GenerateUnitWaveInternal(format, volume, hertz, SquareWaveRatio);
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
        return new SquareWave(SquareWaveRatio);
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
        return GenerateUnitWaveInternal(format, volume, hertz, SquareWaveRatio);
    }

    private static short[] GenerateUnitWaveInternal(SoundFormat format, int volume, double hertz, SquareWaveRatio squareWaveRatio)
    {
        var ratioIndex = (int)squareWaveRatio;
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
        return result.ToArray();
    }
}
