using SoundMaker;

namespace SoundMakerTests.UnitTests;
public class TestFormatBuilder
{
    [Fact()]
    public void TestToSoundFormat()
    {
        var formatBuilder = FormatBuilder.Create()
            .WithFrequency(48000)
            .WithBitDepth(16)
            .WithChannelCount(2);
        var result = formatBuilder.ToSoundFormat();
        Assert.Equal(SoundMaker.Sounds.SamplingFrequencyType.FourtyEightKHz, result.SamplingFrequency);
        Assert.Equal(SoundMaker.Sounds.BitRateType.SixteenBit, result.BitRate);
        Assert.Equal(SoundMaker.Sounds.ChannelType.Stereo, result.Channel);
    }

    [Fact()]
    public void TestToFormatChunk()
    {
        var formatBuilder = FormatBuilder.Create()
            .WithFrequency(48000)
            .WithBitDepth(16)
            .WithChannelCount(2);
        var result = formatBuilder.ToFormatChunk();
        Assert.Equal(SoundMaker.WaveFile.SamplingFrequencyType.FourtyEightKHz, (SoundMaker.WaveFile.SamplingFrequencyType)result.SamplingFrequency);
        Assert.Equal(SoundMaker.WaveFile.BitRateType.SixteenBit, (SoundMaker.WaveFile.BitRateType)result.BitRate);
    }
}
