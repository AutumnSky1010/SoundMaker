using SoundMaker.Sounds.Score;
using SoundMaker.Sounds.SoundChannels;
using SoundMaker.Sounds;
using SoundMaker.WaveFile;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundMakerTests.UseCaseTests;
public class CreateWaveFileTest
{
    private List<WaveCase> MonauralWaveCases { get; } = new List<WaveCase>
    {
        new WaveCase() 
        {
            FormatChunk = new FormatChunk(
                SoundMaker.WaveFile.SamplingFrequencyType.FourtyEightKHz,
                SoundMaker.WaveFile.BitRateType.SixteenBit,
                SoundMaker.WaveFile.ChannelType.Monaural),
            SoundFormat = new SoundFormat(
                SoundMaker.Sounds.SamplingFrequencyType.FourtyEightKHz,
                SoundMaker.Sounds.BitRateType.SixteenBit,
                SoundMaker.Sounds.ChannelType.Monaural),
            Path = @"Sounds\48000hz16bit1ch"
        },
        new WaveCase()
        {
            FormatChunk = new FormatChunk(
                SoundMaker.WaveFile.SamplingFrequencyType.FourtyEightKHz,
                SoundMaker.WaveFile.BitRateType.EightBit,
                SoundMaker.WaveFile.ChannelType.Monaural),
            SoundFormat = new SoundFormat(
                SoundMaker.Sounds.SamplingFrequencyType.FourtyEightKHz,
                SoundMaker.Sounds.BitRateType.EightBit,
                SoundMaker.Sounds.ChannelType.Monaural),
            Path = @"Sounds\48000hz8bit1ch"
        },

        new WaveCase()
        {
            FormatChunk = new FormatChunk(
                SoundMaker.WaveFile.SamplingFrequencyType.FourtyFourKHz,
                SoundMaker.WaveFile.BitRateType.SixteenBit,
                SoundMaker.WaveFile.ChannelType.Monaural),
            SoundFormat = new SoundFormat(
                SoundMaker.Sounds.SamplingFrequencyType.FourtyFourKHz,
                SoundMaker.Sounds.BitRateType.SixteenBit,
                SoundMaker.Sounds.ChannelType.Monaural),
            Path = @"Sounds\44100hz16bit1ch"
        },
        new WaveCase()
        {
            FormatChunk = new FormatChunk(
                SoundMaker.WaveFile.SamplingFrequencyType.FourtyFourKHz,
                SoundMaker.WaveFile.BitRateType.EightBit,
                SoundMaker.WaveFile.ChannelType.Monaural),
            SoundFormat = new SoundFormat(
                SoundMaker.Sounds.SamplingFrequencyType.FourtyFourKHz,
                SoundMaker.Sounds.BitRateType.EightBit,
                SoundMaker.Sounds.ChannelType.Monaural),
            Path = @"Sounds\44100hz8bit1ch"
        },
    };

    private List<WaveCase> StereoWaveCases { get; } = new List<WaveCase>
    {

        new WaveCase()
        {
            FormatChunk = new FormatChunk(
                SoundMaker.WaveFile.SamplingFrequencyType.FourtyEightKHz,
                SoundMaker.WaveFile.BitRateType.SixteenBit,
                SoundMaker.WaveFile.ChannelType.Stereo),
            SoundFormat = new SoundFormat(
                SoundMaker.Sounds.SamplingFrequencyType.FourtyEightKHz,
                SoundMaker.Sounds.BitRateType.SixteenBit,
                SoundMaker.Sounds.ChannelType.Stereo),
            Path = @"Sounds\48000hz16bit2ch"
        },
        new WaveCase()
        {
            FormatChunk = new FormatChunk(
                SoundMaker.WaveFile.SamplingFrequencyType.FourtyEightKHz,
                SoundMaker.WaveFile.BitRateType.EightBit,
                SoundMaker.WaveFile.ChannelType.Stereo),
            SoundFormat = new SoundFormat(
                SoundMaker.Sounds.SamplingFrequencyType.FourtyEightKHz,
                SoundMaker.Sounds.BitRateType.EightBit,
                SoundMaker.Sounds.ChannelType.Stereo),
            Path = @"Sounds\48000hz8bit2ch"
        },
        new WaveCase()
        {
            FormatChunk = new FormatChunk(
                SoundMaker.WaveFile.SamplingFrequencyType.FourtyFourKHz,
                SoundMaker.WaveFile.BitRateType.SixteenBit,
                SoundMaker.WaveFile.ChannelType.Stereo),
            SoundFormat = new SoundFormat(
                SoundMaker.Sounds.SamplingFrequencyType.FourtyFourKHz,
                SoundMaker.Sounds.BitRateType.SixteenBit,
                SoundMaker.Sounds.ChannelType.Stereo),
            Path = @"Sounds\44100hz16bit2ch"
        },
        new WaveCase()
        {
            FormatChunk = new FormatChunk(
                SoundMaker.WaveFile.SamplingFrequencyType.FourtyFourKHz,
                SoundMaker.WaveFile.BitRateType.EightBit,
                SoundMaker.WaveFile.ChannelType.Stereo),
            SoundFormat = new SoundFormat(
                SoundMaker.Sounds.SamplingFrequencyType.FourtyFourKHz,
                SoundMaker.Sounds.BitRateType.EightBit,
                SoundMaker.Sounds.ChannelType.Stereo),
            Path = @"Sounds\44100hz8bit2ch"
        },
    };
    [Fact(DisplayName = "音量が正しく変わるかをテストする")]
    public void ChangeVolume()
    {
        Directory.CreateDirectory("Sounds");
        int tempo = 150;
        PanType panType = PanType.Both;
        var waveCase = this.StereoWaveCases[0];
        var waves = new List<StereoWave>()
        {
            GetStereo(SetAllScale(new SquareSoundChannel(tempo, waveCase.SoundFormat, SquareWaveRatio.Point5, panType))),
            GetStereo(SetAllScale(new SquareSoundChannel(tempo, waveCase.SoundFormat, SquareWaveRatio.Point25, panType))),
            GetStereo(SetAllScale(new SquareSoundChannel(tempo, waveCase.SoundFormat, SquareWaveRatio.Point125, panType))),
            GetStereo(SetAllScale(new TriangleSoundChannel(tempo, waveCase.SoundFormat, panType))),
            GetStereo(SetAllScale(new PseudoTriangleSoundChannel(tempo, waveCase.SoundFormat, panType))),
            GetStereo(SetAllScale(new LowBitNoiseSoundChannel(tempo, waveCase.SoundFormat, panType)))
        };
        var result = new StereoWave(new ushort[0], new ushort[0]);
        foreach (var wave in waves)
        {
            wave.ChangeVolume(1, SoundDirectionType.Both);
            result.Append(wave);
        }
        WriteFile(result.GetBytes(waveCase.SoundFormat.BitRate), waveCase.FormatChunk, $"{waveCase.Path}-volume1-AllScale");
    }

    [Fact(DisplayName = "全ての音程が正しく出力され、ファイルに書き込めるかのテスト。", Skip = "処理に時間がかかるので、使うときのみ外してください。")]
    public void CreateAllScaleWave()
    {
        Directory.CreateDirectory("Sounds");
        int tempo = 150;
        PanType panType = PanType.Both;
        foreach (var waveCase in this.MonauralWaveCases)
        {
            var squareChan5 = GetMonaural(SetAllScale(new SquareSoundChannel(tempo, waveCase.SoundFormat, SquareWaveRatio.Point5, panType)));
            var squareChan25 = GetMonaural(SetAllScale(new SquareSoundChannel(tempo, waveCase.SoundFormat, SquareWaveRatio.Point25, panType)));
            var squareChan125 = GetMonaural(SetAllScale(new SquareSoundChannel(tempo, waveCase.SoundFormat, SquareWaveRatio.Point125, panType)));
            var triangleChan = GetMonaural(SetAllScale(new TriangleSoundChannel(tempo, waveCase.SoundFormat, panType)));
            var pseudoTriangleChan = GetMonaural(SetAllScale(new PseudoTriangleSoundChannel(tempo, waveCase.SoundFormat, panType)));
            var noiseChan = GetMonaural(SetAllScale(new LowBitNoiseSoundChannel(tempo, waveCase.SoundFormat, panType)));

            WriteFile(squareChan5.GetBytes(waveCase.SoundFormat.BitRate), waveCase.FormatChunk, $"{waveCase.Path}-Square5-AllScale");
            WriteFile(squareChan25.GetBytes(waveCase.SoundFormat.BitRate), waveCase.FormatChunk, $"{waveCase.Path}-Square25-AllScale");
            WriteFile(squareChan125.GetBytes(waveCase.SoundFormat.BitRate), waveCase.FormatChunk, $"{waveCase.Path}-Square125-AllScale");
            WriteFile(triangleChan.GetBytes(waveCase.SoundFormat.BitRate), waveCase.FormatChunk, $"{waveCase.Path}-Triangle-AllScale");
            WriteFile(pseudoTriangleChan.GetBytes(waveCase.SoundFormat.BitRate), waveCase.FormatChunk, $"{waveCase.Path}-PseudoTriangele-AllScale");
            WriteFile(noiseChan.GetBytes(waveCase.SoundFormat.BitRate), waveCase.FormatChunk, $"{waveCase.Path}-Noise-AllScale");
        }
        foreach (var waveCase in this.StereoWaveCases)
        {
            var squareChan5 = GetStereo(SetAllScale(new SquareSoundChannel(tempo, waveCase.SoundFormat, SquareWaveRatio.Point5, panType)));
            var squareChan25 = GetStereo(SetAllScale(new SquareSoundChannel(tempo, waveCase.SoundFormat, SquareWaveRatio.Point25, panType)));
            var squareChan125 = GetStereo(SetAllScale(new SquareSoundChannel(tempo, waveCase.SoundFormat, SquareWaveRatio.Point125, panType)));
            var triangleChan = GetStereo(SetAllScale(new TriangleSoundChannel(tempo, waveCase.SoundFormat, panType)));
            var pseudoTriangleChan = GetStereo(SetAllScale(new PseudoTriangleSoundChannel(tempo, waveCase.SoundFormat, panType)));
            var noiseChan = GetStereo(SetAllScale(new LowBitNoiseSoundChannel(tempo, waveCase.SoundFormat, panType)));

            WriteFile(squareChan5.GetBytes(waveCase.SoundFormat.BitRate), waveCase.FormatChunk, $"{waveCase.Path}-Square5-AllScale");
            WriteFile(squareChan25.GetBytes(waveCase.SoundFormat.BitRate), waveCase.FormatChunk, $"{waveCase.Path}-Square25-AllScale");
            WriteFile(squareChan125.GetBytes(waveCase.SoundFormat.BitRate), waveCase.FormatChunk, $"{waveCase.Path}-Square125-AllScale");
            WriteFile(triangleChan.GetBytes(waveCase.SoundFormat.BitRate), waveCase.FormatChunk, $"{waveCase.Path}-Triangle-AllScale");
            WriteFile(pseudoTriangleChan.GetBytes(waveCase.SoundFormat.BitRate), waveCase.FormatChunk, $"{waveCase.Path}-PseudoTriangele-AllScale");
            WriteFile(noiseChan.GetBytes(waveCase.SoundFormat.BitRate), waveCase.FormatChunk, $"{waveCase.Path}-Noise-AllScale");
        }
    }

    private void WriteFile(byte[] wave, FormatChunk format, string name)
    {
        var sound = new SoundWaveChunk(wave);
        var writer = new WaveWriter(format, sound);
        string filePath = $"{name}.wav";
        writer.Write(filePath);
        Console.WriteLine($"{filePath}を書き込んだよ");
    }

    private static StereoWave GetStereo(ISoundChannel channel)
    {
        return new StereoMixer(new List<ISoundChannel>() { channel }).Mix();
    }

    private static MonauralWave GetMonaural(ISoundChannel channel)
    {
        return new MonauralMixer(new List<ISoundChannel>() { channel }).Mix();
    }

    private static ISoundChannel SetAllScale(ISoundChannel channel)
    {
		channel.Add(new Note(Scale.A, 0, LengthType.Quarter));
        //channel.Add(new Note(Scale.ASharp, 0, LengthType.Quarter));
        channel.Add(new Note(Scale.B, 0, LengthType.Quarter));
		for (int i = 1; i <= 7; i++)
		{
            channel.Add(new Note(Scale.C, i, LengthType.Quarter));
            //channel.Add(new Note(Scale.CSharp, i, LengthType.Quarter));
            channel.Add(new Note(Scale.D, i, LengthType.Quarter));
            //channel.Add(new Note(Scale.DSharp, i, LengthType.Quarter));
            channel.Add(new Note(Scale.E, i, LengthType.Quarter));
            channel.Add(new Note(Scale.F, i, LengthType.Quarter));
            //channel.Add(new Note(Scale.FSharp, i, LengthType.Quarter));
            channel.Add(new Note(Scale.G, i, LengthType.Quarter));
            //channel.Add(new Note(Scale.GSharp, i, LengthType.Quarter));
            channel.Add(new Note(Scale.A, i, LengthType.Quarter));
            //channel.Add(new Note(Scale.ASharp, i, LengthType.Quarter));
            channel.Add(new Note(Scale.B, i, LengthType.Quarter));
        }
		channel.Add(new Note(Scale.C, 8, LengthType.Quarter));
        return channel;
    }
}
