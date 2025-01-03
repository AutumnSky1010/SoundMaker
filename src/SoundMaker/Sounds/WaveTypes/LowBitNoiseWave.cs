namespace SoundMaker.Sounds.WaveTypes;

/// <summary>
/// The low bit noise. <br/>ロービットノイズ
/// </summary>
public class LowBitNoiseWave : WaveTypeBase
{
    public override short[] GenerateWave(SoundFormat format, int length, int volume, double hertz)
    {
        var result = new List<short>(length);
        var mode = false;
        var count = 1;
        while (count <= length)
        {
            var allRepeatTimes = (int)((int)format.SamplingFrequency / hertz);
            var firstRepeatTimes = (int)(allRepeatTimes * 0.5);
            var height = (short)new Random().Next(short.MinValue, short.MaxValue + 1);
            for (var i = 1; i <= firstRepeatTimes && mode && count <= length; i++, count++)
            {
                var sound = (short)(height * volume / 100);
                result.Add(sound);
            }
            height = (short)new Random().Next(short.MinValue, short.MaxValue + 1);
            for (var i = 1; i <= allRepeatTimes - firstRepeatTimes && !mode && count <= length; i++, count++)
            {
                var sound = (short)(height * volume / 100);
                result.Add(sound);
            }
            mode = !mode;
        }
        return result.ToArray();
    }

    internal override WaveTypeBase Clone()
    {
        return new LowBitNoiseWave();
    }
}
