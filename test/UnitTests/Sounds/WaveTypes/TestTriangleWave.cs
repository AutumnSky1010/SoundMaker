using SoundMaker.Sounds;
using SoundMaker.Sounds.WaveTypes;

namespace SoundMakerTests.UnitTests.Sounds.WaveTypes;
public class TestTriangleWave
{
    [Fact(DisplayName = "指定した長さの波形データが生成されるかのテスト")]
    [Obsolete]
    public void GenerateWaveTest()
    {
        var waveType = new TriangleWave();
        var length = 100;
        var tempo = 100;
        var volume = 100;
        double hertz = 100;
        var format = new SoundFormat(SamplingFrequencyType.FourtyEightKHz, BitRateType.SixteenBit, ChannelType.Stereo);
        var wave = waveType.GenerateWave(format, tempo, length, volume, hertz);
        Assert.Equal(length, wave.Length);
        wave = waveType.GenerateWave(format, length, volume, hertz);
        Assert.Equal(length, wave.Length);
    }
}
