namespace SoundMaker.Sounds.WaveTypes;
/// <summary>
/// the low bit noise. ロービットノイズ
/// </summary>
public class LowBitNoiseWave : WaveTypeBase
{
    public override ushort[] GenerateWave(SoundFormat format, int tempo, int length, int volume, double hertz)
    {
        var result = new List<ushort>(length);
        bool mode = false;
        int count = 1;
        while (count <= length)
        {
            int allRepeatTimes = (int)((int)format.SamplingFrequency / hertz);
            int firstRepeatTimes = (int)(allRepeatTimes * 0.5);
            ushort height = (ushort)new Random().Next(0, ushort.MaxValue + 1);
            for (int i = 1; i <= firstRepeatTimes && mode && count <= length; i++, count++)
            {
                ushort sound = (ushort)(height * volume / 100);
                result.Add(sound);
            }
            height = (ushort)new Random().Next(0, ushort.MaxValue + 1);
            for (int i = 1; i <= allRepeatTimes - firstRepeatTimes && !mode && count <= length; i++, count++)
            {
                ushort sound = (ushort)(height * volume / 100);
                result.Add(sound);
            }
            mode = !mode;
        }
        return result.ToArray();
    }
}
