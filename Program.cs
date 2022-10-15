using SoundMaker.WaveFile;
using SoundMaker.SoundWave.Score;
using SoundMaker.SoundWave;
namespace SoundMaker;
public static class Program
{
	private static void Main()
	{
		var format = new FormatChunk(SamplingFrequencyType.FourtyFourKHz, BitRateType.EightBit, ChannelType.Stereo);
		var wave = Naki(format);
		wave.Append(Naki(format));
		var sound = new SoundWaveChunk(wave, BitRateType.EightBit);
		IWriteable writer = new WaveWriter(format, sound);
		writer.Write("(テスト).wav");
		Console.WriteLine("書き込んだよ");
	}

	private static MonauralWave CDEFGABC(FormatChunk format)
	{
		var factory1 = new SquareWaveFactory(8, SquareWaveRatio.POINT_5);
		factory1.Add(new EqualTemperament(Scale.C, 4, 0.5));
		factory1.Add(new EqualTemperament(Scale.D, 4, 0.5));
		factory1.Add(new EqualTemperament(Scale.E, 4, 0.5));
		factory1.Add(new EqualTemperament(Scale.D, 4, 0.5));
		factory1.Add(new EqualTemperament(Scale.G, 4, 0.5));
		factory1.Add(new EqualTemperament(Scale.D, 4, 0.5));
		factory1.Add(new EqualTemperament(Scale.B, 4, 0.5));
		factory1.Add(new EqualTemperament(Scale.D, 4, 0.5));
        var factory2 = new SquareWaveFactory(8, SquareWaveRatio.POINT_125);
        factory2.Add(new Rest(0.5));
        factory2.Add(new EqualTemperament(Scale.D, 4, 0.5));
        factory2.Add(new Rest(0.5));
        factory2.Add(new EqualTemperament(Scale.F, 4, 0.5));
        factory2.Add(new Rest(0.5));
        factory2.Add(new EqualTemperament(Scale.A, 4, 0.5));
        factory2.Add(new Rest(0.5));
        factory2.Add(new EqualTemperament(Scale.C, 5, 0.5));
        var monaural1 = factory1.CreateMonaural(format);
		var monaural2 = factory2.CreateMonaural(format);
		return new MonauralMixer(new List<MonauralWave> { monaural1, monaural2, monaural2, monaural2 }).Mix();

	}
	public static StereoWave Naki(FormatChunk format)
	{
		IWaveFactory wave = new TriangleWaveFactory();
		IWaveFactory wave1 = new TriangleWaveFactory();
		IWaveFactory wave2 = new TriangleWaveFactory();
		IWaveFactory wave3 = new TriangleWaveFactory();
		// 「主旋律１」
		wave.Add(new EqualTemperament(Scale.F_SHARP, 4, second: 0.25));
		wave.Add(new EqualTemperament(Scale.B, 4, second: 0.25));
		wave.Add(new EqualTemperament(Scale.C_SHARP, 5, second: 0.25));
		wave.Add(new EqualTemperament(Scale.D, 5, second: 0.75));
		wave.Add(new EqualTemperament(Scale.E, 5, second: 0.25));
		wave.Add(new EqualTemperament(Scale.C_SHARP, 5, second: 0.75));
		wave.Add(new EqualTemperament(Scale.D, 5, second: 0.25));
		wave.Add(new EqualTemperament(Scale.B, 4, second: 1));

		wave.Add(new Rest(second: 0.25));

		wave.Add(new EqualTemperament(Scale.F_SHARP, 4, second: 0.25));
		wave.Add(new EqualTemperament(Scale.B, 4, second: 0.25));
		wave.Add(new EqualTemperament(Scale.C_SHARP, 5, second: 0.25));
		wave.Add(new EqualTemperament(Scale.D, 5, second: 0.75));
		wave.Add(new EqualTemperament(Scale.E, 5, second: 0.25));
		wave.Add(new EqualTemperament(Scale.C_SHARP, 5, second: 0.75));
		wave.Add(new EqualTemperament(Scale.A, 5, second: 0.25));
		wave.Add(new EqualTemperament(Scale.F_SHARP, 5, second: 1));

		wave.Add(new Rest(second: 0.25));

		wave.Add(new EqualTemperament(Scale.F_SHARP, 5, second: 0.25));
		wave.Add(new EqualTemperament(Scale.A, 5, second: 0.25));
		wave.Add(new EqualTemperament(Scale.B, 5, second: 0.25));
		wave.Add(new EqualTemperament(Scale.E, 5, second: 0.5));
		wave.Add(new EqualTemperament(Scale.E, 5, second: 0.75));
		wave.Add(new EqualTemperament(Scale.E, 5, second: 0.25));
		wave.Add(new EqualTemperament(Scale.F_SHARP, 5, second: 0.25));
		wave.Add(new EqualTemperament(Scale.A, 5, second: 0.25));

		wave.Add(new EqualTemperament(Scale.D, 5, second: 0.5));
		wave.Add(new EqualTemperament(Scale.D, 5, second: 0.75));
		wave.Add(new EqualTemperament(Scale.B, 4, second: 0.25));
		wave.Add(new EqualTemperament(Scale.B, 4, second: 0.25));
		wave.Add(new EqualTemperament(Scale.C_SHARP, 5, second: 0.25));
		wave.Add(new EqualTemperament(Scale.D, 5, second: 0.75));
		wave.Add(new EqualTemperament(Scale.E, 5, second: 0.25));
		wave.Add(new EqualTemperament(Scale.C_SHARP, 5, second: 0.75));

		wave.Add(new EqualTemperament(Scale.A_SHARP, 4, second: 0.25));
		wave.Add(new EqualTemperament(Scale.B, 4, second: 1));
		wave.Add(new EqualTemperament(Scale.C_SHARP, 5, second: 1));

		// 「主旋律２」
		wave1.Add(new Rest(second: 0.25));
		wave1.Add(new Rest(second: 0.25));
		wave1.Add(new Rest(second: 0.25));
		wave1.Add(new EqualTemperament(Scale.B, 4, second: 0.75));
		wave1.Add(new Rest(second: 0.25));
		wave1.Add(new EqualTemperament(Scale.A, 4, second: 0.75));
		wave1.Add(new Rest(second: 0.25));
		wave1.Add(new EqualTemperament(Scale.G, 4, second: 1));

		wave1.Add(new Rest(second: 0.25));

		wave1.Add(new Rest(second: 0.25));
		wave1.Add(new Rest(second: 0.25));
		wave1.Add(new Rest(second: 0.25));
		wave1.Add(new EqualTemperament(Scale.B, 4, second: 0.75));
		wave1.Add(new Rest(second: 0.25));
		wave1.Add(new EqualTemperament(Scale.A, 4, second: 0.75));
		wave1.Add(new Rest(second: 0.25));
		wave1.Add(new EqualTemperament(Scale.D, 5, second: 1));
		
		wave1.Add(new Rest(second: 0.25));

		wave1.Add(new Rest(second: 0.25));
		wave1.Add(new Rest(second: 0.25));
		wave1.Add(new Rest(second: 0.25));
		wave1.Add(new EqualTemperament(Scale.B, 4, second: 0.5));
		wave1.Add(new Rest(second: 0.75));
		wave1.Add(new Rest(second: 0.25));
		wave1.Add(new Rest(second: 0.25));
		wave1.Add(new Rest(second: 0.25));

		wave1.Add(new EqualTemperament(Scale.A, 4, second: 0.5));
		wave1.Add(new Rest(second: 0.75));
		wave1.Add(new Rest(second: 0.25));
		wave1.Add(new Rest(second: 0.25));
		wave1.Add(new Rest(second: 0.25));
		wave1.Add(new EqualTemperament(Scale.G, 4, second: 0.75));
		wave1.Add(new Rest(second: 0.25));
		wave1.Add(new EqualTemperament(Scale.A, 4, second: 0.75));

		wave1.Add(new Rest(second: 0.25));
		wave1.Add(new EqualTemperament(Scale.G, 4, second: 1));
		wave1.Add(new EqualTemperament(Scale.A, 4, second: 1));
		
		// 「ベース1」
		wave2.Add(new Rest(second: 0.25));
		wave2.Add(new Rest(second: 0.25));
		wave2.Add(new Rest(second: 0.25));

		wave2.Add(new EqualTemperament(Scale.B, 2, second: 0.25));
		wave2.Add(new EqualTemperament(Scale.F_SHARP, 3, second: 0.25));
		wave2.Add(new EqualTemperament(Scale.A, 3, second: 0.5));
		wave2.Add(new EqualTemperament(Scale.B, 2, second: 0.25));
		wave2.Add(new EqualTemperament(Scale.F_SHARP, 3, second: 0.25));
		wave2.Add(new EqualTemperament(Scale.A, 3, second: 0.5));

		wave2.Add(new EqualTemperament(Scale.G, 2, second: 0.25));
		wave2.Add(new EqualTemperament(Scale.B, 2, second: 0.25));
		wave2.Add(new EqualTemperament(Scale.D, 3, second: 0.25));
		wave2.Add(new EqualTemperament(Scale.G, 3, second: 0.25));
		wave2.Add(new EqualTemperament(Scale.B, 3, second: 1));


		wave2.Add(new EqualTemperament(Scale.B, 2, second: 0.25));
		wave2.Add(new EqualTemperament(Scale.F_SHARP, 3, second: 0.25));
		wave2.Add(new EqualTemperament(Scale.A, 3, second: 0.5));
		wave2.Add(new EqualTemperament(Scale.B, 2, second: 0.25));
		wave2.Add(new EqualTemperament(Scale.F_SHARP, 3, second: 0.25));
		wave2.Add(new EqualTemperament(Scale.A, 3, second: 0.5));

		wave2.Add(new EqualTemperament(Scale.G_SHARP, 2, second: 0.25));
		wave2.Add(new EqualTemperament(Scale.B, 2, second: 0.25));
		wave2.Add(new EqualTemperament(Scale.E, 3, second: 0.25));
		wave2.Add(new EqualTemperament(Scale.G_SHARP, 3, second: 0.25));
		wave2.Add(new EqualTemperament(Scale.B, 3, second: 1));
		//
		wave2.Add(new EqualTemperament(Scale.B, 2, second: 2));
		wave2.Add(new EqualTemperament(Scale.A, 2, second: 2));
		wave2.Add(new EqualTemperament(Scale.B, 2, second: 1));
		wave2.Add(new EqualTemperament(Scale.C, 3, second: 1));

		wave2.Add(new EqualTemperament(Scale.G, 2, second: 0.25));
		wave2.Add(new EqualTemperament(Scale.D, 3, second: 0.25));
		wave2.Add(new EqualTemperament(Scale.G, 2, second: 0.25));
		wave2.Add(new EqualTemperament(Scale.D, 3, second: 0.25));
		wave2.Add(new EqualTemperament(Scale.G, 2, second: 0.25));
		wave2.Add(new EqualTemperament(Scale.D, 3, second: 0.25));
		wave2.Add(new EqualTemperament(Scale.G, 2, second: 0.25));
		wave2.Add(new EqualTemperament(Scale.D, 3, second: 0.25));
		
		// 「ベース2」
		wave3.Add(new Rest(second: 0.25));
		wave3.Add(new Rest(second: 0.25));
		wave3.Add(new Rest(second: 0.25));

		wave3.Add(new Rest(second: 0.25));
		wave3.Add(new Rest(second: 0.25));
		wave3.Add(new Rest(second: 0.5));
		wave3.Add(new Rest(second: 0.25));
		wave3.Add(new Rest(second: 0.25));
		wave3.Add(new Rest(second: 0.5));

		wave3.Add(new Rest(second: 0.25));
		wave3.Add(new Rest(second: 0.25));
		wave3.Add(new Rest(second: 0.25));
		wave3.Add(new Rest(second: 0.25));
		wave3.Add(new Rest(second: 1));


		wave3.Add(new Rest(second: 0.25));
		wave3.Add(new Rest(second: 0.25));
		wave3.Add(new Rest(second: 0.5));
		wave3.Add(new Rest(second: 0.25));
		wave3.Add(new Rest(second: 0.25));
		wave3.Add(new Rest(second: 0.5));

		wave3.Add(new Rest(second: 0.25));
		wave3.Add(new Rest(second: 0.25));
		wave3.Add(new Rest(second: 0.25));
		wave3.Add(new Rest(second: 0.25));
		wave3.Add(new Rest(second: 1));
		//
		wave3.Add(new EqualTemperament(Scale.E, 2, second: 2));
		wave3.Add(new EqualTemperament(Scale.D, 2, second: 2));
		wave3.Add(new EqualTemperament(Scale.E, 2, second: 1));
		wave3.Add(new EqualTemperament(Scale.F_SHARP, 3, second: 1));

		var tracks = new List<MonauralWave>()
		{
			wave.CreateMonaural(format),wave1.CreateMonaural(format),wave2.CreateMonaural(format),wave3.CreateMonaural(format),
            wave.CreateMonaural(format),wave1.CreateMonaural(format),wave2.CreateMonaural(format),wave3.CreateMonaural(format),
            wave.CreateMonaural(format),wave1.CreateMonaural(format),wave2.CreateMonaural(format),wave3.CreateMonaural(format)
        };
		var stereo1 = new StereoWave(tracks[0].GetValues(), tracks[3].GetValues());
        var stereo2 = new StereoWave(tracks[1].GetValues(), tracks[2].GetValues());
		var stereoTracks = new List<StereoWave>()
		{
			stereo1, stereo2
		};
        return new StereoMixer(stereoTracks).Mix();
	}
}
