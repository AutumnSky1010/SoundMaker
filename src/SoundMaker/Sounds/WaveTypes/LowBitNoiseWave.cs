namespace SoundMaker.Sounds.WaveTypes;
/// <summary>
/// the low bit noise. ロービットノイズ
/// </summary>
public class LowBitNoiseWave : WaveTypeBase
{
    [Obsolete("Use 'GenerateWave(SoundFormat format, int length, int volume, double hertz)'")]
    public override short[] GenerateWave(SoundFormat format, int tempo, int length, int volume, double hertz)
    {
        CheckGenerateWaveArgs(tempo, length, volume, hertz);
        return GenerateWave(format, length, volume, hertz);
    }

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
            height = (short)new Random().Next(0, short.MaxValue + 1);
            for (var i = 1; i <= allRepeatTimes - firstRepeatTimes && !mode && count <= length; i++, count++)
            {
                var sound = (short)(height * volume / 100);
                result.Add(sound);
            }
            mode = !mode;
        }
        return result.ToArray();
    }
}
