using SoundMaker.Sounds;
using SoundMaker.Sounds.Score;
using SoundMaker.Sounds.SoundChannels;
using SoundMaker.Sounds.WaveTypes;
using SoundMaker.WaveFile;
using System.Diagnostics;

namespace YourNamespace;
public static class YourClass
{
    private static void Main()
    {
        // サウンドの形式を作成する。
        var soundFormat = new SoundFormat(SoundMaker.Sounds.SamplingFrequencyType.FourtyEightKHz, SoundMaker.Sounds.BitRateType.SixteenBit, SoundMaker.Sounds.ChannelType.Stereo);

        var sw = new Stopwatch();
        sw.Start();
        var waveData = new PseudoTriangleWave().GenerateWave(soundFormat, (int)soundFormat.SamplingFrequency * 100, 50, 440.0d);
        sw.Stop();
        Console.WriteLine(sw.Elapsed.TotalMilliseconds);
        var wave = new StereoWave(new List<ushort>(waveData), new List<ushort>(waveData));

        // ファイルに書き込む。
        var sound = new SoundWaveChunk(wave.GetBytes(soundFormat.BitRate));
        var waveFileFormat = new FormatChunk(SoundMaker.WaveFile.SamplingFrequencyType.FourtyEightKHz, SoundMaker.WaveFile.BitRateType.SixteenBit, SoundMaker.WaveFile.ChannelType.Stereo);
        var writer = new WaveWriter(waveFileFormat, sound);
        string filePath = "sample.wav";
        writer.Write(filePath);
    }
}