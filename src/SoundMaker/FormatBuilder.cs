﻿using SoundMaker.Sounds;
using SoundMaker.WaveFile;

namespace SoundMaker;
/// <summary>
/// Represents a class used to build SoundFormat and FormatChunk.
/// </summary>
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
            Builder = builder;
        }

        private FormatBuilder Builder { get; }

        private static Dictionary<int, (Sounds.BitRateType sounds, WaveFile.BitRateType wave)> BitRates { get; } = new()
        {
            { 16, (Sounds.BitRateType.SixteenBit, WaveFile.BitRateType.SixteenBit) },
            { 8, (Sounds.BitRateType.EightBit, WaveFile.BitRateType.EightBit) }
        };

        /// <summary>
        /// Sets a bit depth(BitRateType) to format. <br/>
        /// 量子化ビット数を指定する。
        /// </summary>
        /// <param name="bitDepth">value</param>
        /// <returns>builder</returns>
        /// <exception cref="ArgumentException">The bitDepth value must be either 8 or 16.</exception>
        public ChannelTypeBuilder WithBitDepth(int bitDepth)
        {
            if (BitRates.TryGetValue(bitDepth, out var setting))
            {
                Builder.BitRateTypePair = setting;
                return new ChannelTypeBuilder(Builder);
            }
            throw new ArgumentException("The bitDepth value must be either 8 or 16.", nameof(bitDepth));
        }
    }

    public class SamplingFrequencyBuilder
    {
        internal SamplingFrequencyBuilder(FormatBuilder builder)
        {
            Builder = builder;
        }

        private FormatBuilder Builder { get; }

        private static Dictionary<int, (Sounds.SamplingFrequencyType sounds, WaveFile.SamplingFrequencyType wave)> SamplingFrequencies { get; } = new()
        {
            { 48000, (Sounds.SamplingFrequencyType.FourtyEightKHz, WaveFile.SamplingFrequencyType.FourtyEightKHz) },
            { 44100, (Sounds.SamplingFrequencyType.FourtyFourKHz, WaveFile.SamplingFrequencyType.FourtyFourKHz) }
        };

        /// <summary>
        /// Sets a sampling frequency to format. <br/>
        /// サンプリング周波数を指定する。
        /// </summary>
        /// <param name="frequency">sampling frequency</param>
        /// <returns>builder</returns>
        /// <exception cref="ArgumentException">The frequency value must be either 48000 or 44100.</exception>
        public BitDepthBuilder WithFrequency(int frequency)
        {
            if (SamplingFrequencies.TryGetValue(frequency, out var setting))
            {
                Builder.SamplingFrequencyTypePair = setting;
                return new BitDepthBuilder(Builder);
            }
            throw new ArgumentException("The frequency value must be either 48000 or 44100.", nameof(frequency));
        }
    }

    public class ChannelTypeBuilder
    {
        internal ChannelTypeBuilder(FormatBuilder builder)
        {
            Builder = builder;
        }

        private FormatBuilder Builder { get; }

        private static Dictionary<int, (Sounds.ChannelType sounds, WaveFile.ChannelType wave)> Channels { get; } = new()
        {
            { 1, (Sounds.ChannelType.Monaural, WaveFile.ChannelType.Monaural) },
            { 2, (Sounds.ChannelType.Stereo, WaveFile.ChannelType.Stereo) }
        };

        /// <summary>
        /// Sets a count fo channels to format.<br/>
        /// チャンネル数を設定する(1: モノラル、2: ステレオ)
        /// </summary>
        /// <param name="count">count of channels<br/>チャンネル数</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">The count value must be either 1 or 2.</exception>
        public FormatBuilder WithChannelCount(int count)
        {
            if (Channels.TryGetValue(count, out var setting))
            {
                Builder.ChannelTypePair = setting;
                return Builder;
            }
            throw new ArgumentException("The count value must be either 1 or 2.", nameof(count));
        }
    }

    /// <summary>
    /// Create an instance of builder. <br/>
    /// ビルダのインスタンスを作成する。
    /// </summary>
    /// <returns>builder</returns>
    public static SamplingFrequencyBuilder Create()
    {
        var builder = new FormatBuilder();
        return new SamplingFrequencyBuilder(builder);
    }

    /// <summary>
    /// Build to FormatChunk. <br/>
    /// FormatChunkにビルドする。 
    /// </summary>
    /// <returns>FormatChunk</returns>
    public FormatChunk ToFormatChunk()
    {
        return new FormatChunk(SamplingFrequencyTypePair.wave, BitRateTypePair.wave, ChannelTypePair.wave);
    }

    /// <summary>
    /// Build to SoundFormat. <br/>
    /// SoundFormatにビルドする。
    /// </summary>
    /// <returns>SoundFormat</returns>
    public SoundFormat ToSoundFormat()
    {
        return new SoundFormat(SamplingFrequencyTypePair.sounds, BitRateTypePair.sounds, ChannelTypePair.sounds);
    }
}
