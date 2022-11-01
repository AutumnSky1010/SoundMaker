using SoundMaker.WaveFile;
using SoundMaker.SoundWave.Score;
using SoundMaker.SoundWave;
namespace SoundMaker;
public static class Program
{
	private static void Main()
	{
		var format = new FormatChunk(SamplingFrequencyType.FourtyEightKHz, BitRateType.EightBit, ChannelType.Stereo);
		var wave = Shinko.GetShinko(format);
		wave.ChangeVolume(30, WaveChannelType.BOTH);
		var sound = new SoundWaveChunk(wave, BitRateType.EightBit);
		IWriteable writer = new WaveWriter(format, sound);
		writer.Write("shinko.wav");
		Console.WriteLine("書き込んだよ");
	}

	public static StereoWave Naki(FormatChunk format)
	{
		int tempo = 144;
		ISoundChannel wave = new SquareSoundChannel(tempo, format, SquareWaveRatio.POINT_5, SoundWave.WaveFactory.PanType.LEFT);
		ISoundChannel wave1 = new SquareSoundChannel(tempo, format, SquareWaveRatio.POINT_5, SoundWave.WaveFactory.PanType.LEFT);
		ISoundChannel wave2 = new SquareSoundChannel(tempo, format, SquareWaveRatio.POINT_5, SoundWave.WaveFactory.PanType.RIGHT);
		ISoundChannel wave3 = new SquareSoundChannel(tempo, format, SquareWaveRatio.POINT_5, SoundWave.WaveFactory.PanType.RIGHT);
		// 「主旋律１」
		wave.Add(new Note(Scale.F_SHARP, 4, 8));
		wave.Add(new Note(Scale.B, 4, 8));
		wave.Add(new Note(Scale.C_SHARP, 5, 8));
		wave.Add(new Note(Scale.D, 5, 4, true));
		wave.Add(new Note(Scale.E, 5, 8));
		wave.Add(new Note(Scale.C_SHARP, 5, 4, true));
		wave.Add(new Note(Scale.D, 5, 8));
		wave.Add(new Note(Scale.B, 4, 2));

		wave.Add(new Rest(8));

		wave.Add(new Note(Scale.F_SHARP, 4, 8));
		wave.Add(new Note(Scale.B, 4, 8));
		wave.Add(new Note(Scale.C_SHARP, 5, 8));
		wave.Add(new Note(Scale.D, 5, 4, true));
		wave.Add(new Note(Scale.E, 5, 8));
		wave.Add(new Note(Scale.C_SHARP, 5, 4, true));
		wave.Add(new Note(Scale.A, 5, 8));
		wave.Add(new Note(Scale.F_SHARP, 5, 2));

		wave.Add(new Rest(8));

		wave.Add(new Note(Scale.F_SHARP, 5, 8));
		wave.Add(new Note(Scale.A, 5, 8));
		wave.Add(new Note(Scale.B, 5, 8));
		wave.Add(new Note(Scale.E, 5, 4));
		wave.Add(new Note(Scale.E, 5, 4, true));
		wave.Add(new Note(Scale.E, 5, 8));
		wave.Add(new Note(Scale.F_SHARP, 5, 8));
		wave.Add(new Note(Scale.A, 5, 8));

		wave.Add(new Note(Scale.D, 5, 4));
		wave.Add(new Note(Scale.D, 5, 4, true));
		wave.Add(new Note(Scale.B, 4, 8));
		wave.Add(new Note(Scale.B, 4, 8));
		wave.Add(new Note(Scale.C_SHARP, 5, 8));
		wave.Add(new Note(Scale.D, 5, 4, true));
		wave.Add(new Note(Scale.E, 5, 8));
		wave.Add(new Note(Scale.C_SHARP, 5, 4, true));

		wave.Add(new Note(Scale.A_SHARP, 4, 8));
		wave.Add(new Note(Scale.B, 4, 2));
		wave.Add(new Note(Scale.C_SHARP, 5, 2));

		// 「主旋律２」
		wave1.Add(new Rest(8));
		wave1.Add(new Rest(8));
		wave1.Add(new Rest(8));
		wave1.Add(new Note(Scale.B, 4, 4, true));
		wave1.Add(new Rest(8));
		wave1.Add(new Note(Scale.A, 4, 4, true));
		wave1.Add(new Rest(8));
		wave1.Add(new Note(Scale.G, 4, 2));

		wave1.Add(new Rest(8));

		wave1.Add(new Rest(8));
		wave1.Add(new Rest(8));
		wave1.Add(new Rest(8));
		wave1.Add(new Note(Scale.B, 4, 4, true));
		wave1.Add(new Rest(8));
		wave1.Add(new Note(Scale.A, 4, 4, true));
		wave1.Add(new Rest(8));
		wave1.Add(new Note(Scale.D, 5, 2));
		
		wave1.Add(new Rest(8));

		wave1.Add(new Rest(8));
		wave1.Add(new Rest(8));
		wave1.Add(new Rest(8));
		wave1.Add(new Note(Scale.B, 4, 4));
		wave1.Add(new Rest(4, true));
		wave1.Add(new Rest(8));
		wave1.Add(new Rest(8));
		wave1.Add(new Rest(8));

		wave1.Add(new Note(Scale.A, 4, 4));
		wave1.Add(new Rest(4, true));
		wave1.Add(new Rest(8));
		wave1.Add(new Rest(8));
		wave1.Add(new Rest(8));
		wave1.Add(new Note(Scale.G, 4, 4, true));
		wave1.Add(new Rest(8));
		wave1.Add(new Note(Scale.A, 4, 4, true));

		wave1.Add(new Rest(8));
		wave1.Add(new Note(Scale.G, 4, 2));
		wave1.Add(new Note(Scale.A, 4, 2));
		
		// 「ベース1」
		wave2.Add(new Rest(8));
		wave2.Add(new Rest(8));
		wave2.Add(new Rest(8));

		wave2.Add(new Note(Scale.B, 2, 8));
		wave2.Add(new Note(Scale.F_SHARP, 3, 8));
		wave2.Add(new Note(Scale.A, 3, 4));
		wave2.Add(new Note(Scale.B, 2, 8));
		wave2.Add(new Note(Scale.F_SHARP, 3, 8));
		wave2.Add(new Note(Scale.A, 3, 4));

		wave2.Add(new Note(Scale.G, 2, 8));
		wave2.Add(new Note(Scale.B, 2, 8));
		wave2.Add(new Note(Scale.D, 3, 8));
		wave2.Add(new Note(Scale.G, 3, 8));
		wave2.Add(new Note(Scale.B, 3, 2));


		wave2.Add(new Note(Scale.B, 2, 8));
		wave2.Add(new Note(Scale.F_SHARP, 3, 8));
		wave2.Add(new Note(Scale.A, 3, 4));
		wave2.Add(new Note(Scale.B, 2, 8));
		wave2.Add(new Note(Scale.F_SHARP, 3, 8));
		wave2.Add(new Note(Scale.A, 3, 4));

		wave2.Add(new Note(Scale.G_SHARP, 2, 8));
		wave2.Add(new Note(Scale.B, 2, 8));
		wave2.Add(new Note(Scale.E, 3, 8));
		wave2.Add(new Note(Scale.G_SHARP, 3, 8));
		wave2.Add(new Note(Scale.B, 3, 2));
		//
		wave2.Add(new Note(Scale.B, 2, 1));
		wave2.Add(new Note(Scale.A, 2, 1));
		wave2.Add(new Note(Scale.B, 2, 2));
		wave2.Add(new Note(Scale.C, 3, 2));

		wave2.Add(new Note(Scale.G, 2, 8));
		wave2.Add(new Note(Scale.D, 3, 8));
		wave2.Add(new Note(Scale.G, 2, 8));
		wave2.Add(new Note(Scale.D, 3, 8));
		wave2.Add(new Note(Scale.G, 2, 8));
		wave2.Add(new Note(Scale.D, 3, 8));
		wave2.Add(new Note(Scale.G, 2, 8));
		wave2.Add(new Note(Scale.D, 3, 8));
		
		// 「ベース2」
		wave3.Add(new Rest(8));
		wave3.Add(new Rest(8));
		wave3.Add(new Rest(8));

		wave3.Add(new Rest(8));
		wave3.Add(new Rest(8));
		wave3.Add(new Rest(4));
		wave3.Add(new Rest(8));
		wave3.Add(new Rest(8));
		wave3.Add(new Rest(4));

		wave3.Add(new Rest(8));
		wave3.Add(new Rest(8));
		wave3.Add(new Rest(8));
		wave3.Add(new Rest(8));
		wave3.Add(new Rest(2));


		wave3.Add(new Rest(8));
		wave3.Add(new Rest(8));
		wave3.Add(new Rest(4));
		wave3.Add(new Rest(8));
		wave3.Add(new Rest(8));
		wave3.Add(new Rest(4));

		wave3.Add(new Rest(8));
		wave3.Add(new Rest(8));
		wave3.Add(new Rest(8));
		wave3.Add(new Rest(8));
		wave3.Add(new Rest(2));
		//
		wave3.Add(new Note(Scale.E, 2, 1));
		wave3.Add(new Note(Scale.D, 2, 1));
		wave3.Add(new Note(Scale.E, 2, 2));
		wave3.Add(new Note(Scale.F_SHARP, 3, 2));

		var channels = new List<ISoundChannel>()
		{
			wave, wave1, wave2, wave3
		};
		var mixer = new StereoMixer(channels);
		return mixer.Mix();
	}
}
