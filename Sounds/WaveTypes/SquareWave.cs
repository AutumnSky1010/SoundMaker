using SoundMaker.Sounds.Score;

namespace SoundMaker.Sounds.WaveTypes;
/// <summary>
/// square wave. 矩形波
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

    public override ushort[] GenerateWave(SoundFormat format, int tempo, int length, int volume, double hertz)
    {
        var result = new List<ushort>(length);
        bool mode = false;
        int count = 1;
        int ratioIndex = (int)this.SquareWaveRatio;
        while (count <= length)
        {
            int allRepeatTimes = (int)((int)format.SamplingFrequency / hertz);
            int firstRepeatTimes = (int)(allRepeatTimes * this.Ratio[ratioIndex].Item1);

            if (count + allRepeatTimes >= length)
            {
                result.Add(0);
                count++;
                continue;
            }

            for (int i = 1; i <= firstRepeatTimes && mode && count <= length; i++, count++)
            {
                ushort sound = (ushort)(ushort.MaxValue * volume / 100);
                result.Add(sound);
            }

            for (int i = 1; i <= allRepeatTimes - firstRepeatTimes && !mode && count <= length; i++, count++)
            {
                ushort sound = 0;
                result.Add(sound);
            }
            mode = !mode;
        }
        return result.ToArray();
    }
}
