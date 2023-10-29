using SoundMaker;
using SoundMaker.Sounds;
using SoundMaker.Sounds.Score;
using SoundMaker.Sounds.SoundChannels;
using SoundMaker.WaveFile;
using Xunit.Abstractions;

namespace SoundMakerTests;
public class Experiment
{
    public Experiment(ITestOutputHelper output)
    {
        Output = output;
    }

    private ITestOutputHelper Output { get; }

    [Fact(DisplayName = "README.md‚ÌƒTƒ“ƒvƒ‹")]
    public void TestReflectedVolume()
    {
        Main();
    }

    private void Main()
    {
        // Create a sound format.
        var builder = FormatBuilder.Create()
            .WithFrequency(48000)
            .WithBitDepth(16)
            .WithChannelCount(2);

        var soundFormat = builder.ToSoundFormat();
        var wave = MakeStereoWave(soundFormat);

        // Write to a file.
        var sound = new SoundWaveChunk(wave.GetBytes(soundFormat.BitRate));
        var waveFileFormat = builder.ToFormatChunk();
        var writer = new WaveWriter(waveFileFormat, sound);
        var filePath = "sample.wav";
        writer.Write(filePath);
    }

    private StereoWave MakeStereoWave(SoundFormat format)
    {
        // The number of quarter notes per minute
        var tempo = 100;
        // First, you need to create sound channels.
        // Currently, it supports square wave, triangle wave, pseudo-triangle wave, and low-bit noise.
        var rightChannel = new SquareSoundChannel(tempo, format, SquareWaveRatio.Point25, PanType.Right)
        {
            // Add objects of classes that implement ISoundComponent to the channel.
            // Currently, you can use normal notes, rests, ties, and tuplets.
            new Note(Scale.C, 5, LengthType.Eighth, isDotted: true),
            new Tie(new Note(Scale.D, 5, LengthType.Eighth), LengthType.Eighth),
            new Tuplet(GetComponents(), LengthType.Quarter)
        };
        var rightChannel2 = new SquareSoundChannel(tempo, format, SquareWaveRatio.Point125, PanType.Right)
        {
            new Note(Scale.C, 4, LengthType.Eighth, isDotted: true),
            new Note(Scale.D, 4, LengthType.Quarter),
            new Rest(LengthType.Quarter)
        };
        var leftChannel = new TriangleSoundChannel(tempo, format, PanType.Left)
        {
            new Note(Scale.C, 3, LengthType.Eighth, isDotted: true),
            new Note(Scale.D, 3, LengthType.Quarter),
            new Rest(LengthType.Quarter)
        };

        var channels = new List<ISoundChannel>() { rightChannel, rightChannel2, leftChannel };
        // Mixing is done by the 'StereoMixer' class. 
        return new StereoMixer(channels).Mix();
    }

    private IReadOnlyList<BasicSoundComponentBase> GetComponents()
    {
        return new List<BasicSoundComponentBase>()
        {
            new Note(Scale.E, 5, LengthType.Eighth),
            new Note(Scale.F, 5, LengthType.Eighth),
            new Note(Scale.G, 5, LengthType.Eighth),
        };
    }
}
