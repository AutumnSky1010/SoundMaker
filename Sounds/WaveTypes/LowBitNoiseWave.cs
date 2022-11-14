namespace SoundMaker.Sounds.WaveTypes;
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
            /*
            if (count + allRepeatTimes >= length)
            {
                result.Add(0);
                count++;
                continue;
            }*/
            ushort height = (ushort)new Random().Next(0, ushort.MaxValue + 1);
            for (int i = 1; i <= firstRepeatTimes && mode && count <= length; i++, count++)
            {
                ushort sound = (ushort)(height * volume / 100);
                result.Add(sound);
            }
            height = (ushort)new Random().Next(0, ushort.MaxValue + 1);
            for (int i = 1; i <= allRepeatTimes - firstRepeatTimes && !mode && count <= length; i++, count++)
            {
                result.Add(height);
            }
            mode = !mode;
        }
        return result.ToArray();
    }
}
