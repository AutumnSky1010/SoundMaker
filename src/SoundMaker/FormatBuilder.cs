using SoundMaker.Sounds;
using SoundMaker.WaveFile;
using System.Collections.ObjectModel;

namespace SoundMaker;
public class FormatBuilder
{
    private FormatBuilder() { }

    private (Sounds.BitRateType sounds, WaveFile.BitRateType wave) BitRateTypePair { get; set; }

    private (Sounds.ChannelType sounds, WaveFile.ChannelType wave) ChannelTypePair { get; set; }

    private (Sounds.SamplingFrequencyType sounds, WaveFile.SamplingFrequencyType wave) SamplingFrequencyTypePair { get; set; }

    public class BitDepthBuilder
    {
        internal BitDepthBuilder(FormatBuilder builder)
        {
            this.Builder = builder;
        }

        private FormatBuilder Builder { get; }

        private static Dictionary<int, (Sounds.BitRateType sounds, WaveFile.BitRateType wave)> BitRates { get; } = new()
        {
            { 16, (Sounds.BitRateType.SixteenBit, WaveFile.BitRateType.SixteenBit) },
            { 8, (Sounds.BitRateType.EightBit, WaveFile.BitRateType.EightBit) }
        };

        public ChannelTypeBuilder WithBitDepth(int bitDepth)
        {
            if (BitRates.TryGetValue(bitDepth, out var setting))
            {
                this.Builder.BitRateTypePair = setting;
                return new ChannelTypeBuilder(this.Builder);
            }
            throw new ArgumentException("The bitDepth value must be either 8 or 16.", nameof(bitDepth));
        }
    }

    public class SamplingFrequencyBuilder
    {
        internal SamplingFrequencyBuilder(FormatBuilder builder)
        {
            this.Builder = builder;
        }

        private FormatBuilder Builder { get; }

        private static Dictionary<int, (Sounds.SamplingFrequencyType sounds, WaveFile.SamplingFrequencyType wave)> SamplingFrequencies { get; } = new()
        {
            { 48000, (Sounds.SamplingFrequencyType.FourtyEightKHz, WaveFile.SamplingFrequencyType.FourtyEightKHz) },
            { 44100, (Sounds.SamplingFrequencyType.FourtyFourKHz, WaveFile.SamplingFrequencyType.FourtyFourKHz) }
        };

        public BitDepthBuilder WithFrequency(int frequency)
        {
            if (SamplingFrequencies.TryGetValue(frequency, out var setting))
            {
                this.Builder.SamplingFrequencyTypePair = setting;
                return new BitDepthBuilder(this.Builder);
            }
            throw new ArgumentException("The frequency value must be either 48000 or 44100.", nameof(frequency));
        }
    }

    public class ChannelTypeBuilder
    {
        internal ChannelTypeBuilder(FormatBuilder builder)
        {
            this.Builder = builder;
        }

        private FormatBuilder Builder { get; }

        private static Dictionary<int, (Sounds.ChannelType sounds, WaveFile.ChannelType wave)> Channels { get; } = new()
        {
            { 1, (Sounds.ChannelType.Monaural, WaveFile.ChannelType.Monaural) },
            { 2, (Sounds.ChannelType.Stereo, WaveFile.ChannelType.Stereo) }
        };

        public FormatBuilder WithChannelCount(int count)
        {
            if (Channels.TryGetValue(count, out var setting))
            {
                this.Builder.ChannelTypePair = setting;
                return this.Builder;
            }
            throw new ArgumentException("The count value must be either 1 or 2.", nameof(count));
        }
    }

    public static SamplingFrequencyBuilder Create()
    {
        var builder = new FormatBuilder();
        return new SamplingFrequencyBuilder(builder);
    }

    public FormatChunk ToFormatChunk()
    {
        return new FormatChunk(this.SamplingFrequencyTypePair.wave, this.BitRateTypePair.wave, this.ChannelTypePair.wave);
    }

    public SoundFormat ToSoundFormat()
    {
        return new SoundFormat(this.SamplingFrequencyTypePair.sounds, this.BitRateTypePair.sounds, this.ChannelTypePair.sounds);
    }
}
