using SoundMaker.Sounds.WaveTypes;
using SoundMaker.Sounds;
using SoundMaker.Sounds.SoundChannels;

namespace SoundMakerTests.UnitTests.Sounds.WaveTypes;
public class TestSquareWave
{
    [Fact(DisplayName = "指定した長さの波形データが生成されるかのテスト")]
    public void GenerateWaveTest()
    {
        var waveType = new SquareWave(SquareWaveRatio.Point25);
        int length = 100;
        int tempo = 100;
        int volume = 100;
        double hertz = 100;
        SoundFormat format = new SoundFormat(SamplingFrequencyType.FourtyEightKHz, BitRateType.SixteenBit, ChannelType.Stereo);
        var wave = waveType.GenerateWave(format, tempo, length, volume, hertz);
        Assert.Equal(length, wave.Length);
        wave = waveType.GenerateWave(format, length, volume, hertz);
        Assert.Equal(length, wave.Length);
    }
}
