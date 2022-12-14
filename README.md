![SoundMakerCover](https://user-images.githubusercontent.com/66455966/206901705-974f5a63-46db-435c-bdb1-1717e8bb7883.png)  

## ðºï¸è¨èª(Language)
1) [æ¥æ¬èª](#æ¦è¦)
2) [English](#overview)

## ðµæ¦è¦
æ¬ã©ã¤ãã©ãªãç¨ããã¨ãä»¥ä¸ã®äºãå¯è½ã§ãã
- ããããã¥ã¼ã³ãµã¦ã³ãï¼ãä½æãã
- waveãã¡ã¤ã«ã«ãµã¦ã³ããæ¸ãè¾¼ã

## ðãã­ã¥ã¡ã³ã
[Wiki](https://github.com/AutumnSky1010/SoundMaker/wiki)

## â°ï¸è¦ä»¶
.NET 6

## â¬ã¤ã³ã¹ãã¼ã«æ¹æ³
### NuGet

[SoundMaker](https://www.nuget.org/packages/SoundMaker/)

## ð¶ç°¡åãªä½¿ãæ¹
```CSharp
using SoundMaker.Sounds;
using SoundMaker.Sounds.Score;
using SoundMaker.Sounds.SoundChannels;
using SoundMaker.WaveFile;

namespace YourNamespace;
public static class YourClass
{
	private static void Main()
	{
		// ãµã¦ã³ãã®å½¢å¼ãä½æããã
		var soundFormat = new SoundFormat(SoundMaker.Sounds.SamplingFrequencyType.FourtyEightKHz, SoundMaker.Sounds.BitRateType.SixteenBit, SoundMaker.Sounds.ChannelType.Stereo);
		StereoWave wave = MakeStereoWave(soundFormat);

		// ãã¡ã¤ã«ã«æ¸ãè¾¼ãã
		var sound = new SoundWaveChunk(wave.GetBytes(soundFormat.BitRate));
		var waveFileFormat = new FormatChunk(SoundMaker.WaveFile.SamplingFrequencyType.FourtyEightKHz, SoundMaker.WaveFile.BitRateType.SixteenBit, SoundMaker.WaveFile.ChannelType.Stereo);
		var writer = new WaveWriter(waveFileFormat, sound);
		string filePath = "sample.wav";
		writer.Write(filePath);
	}

	private static StereoWave MakeStereoWave(SoundFormat format)
	{
		// ä¸åéã®ååé³ç¬¦ã®åæ°
		int tempo = 100;
		// ã¾ããé³ã®ãã£ã³ãã«ãä½æããå¿è¦ãããã
		// ç¾æ®µéã§ã¯ç©å½¢æ³¢ãä¸è§æ³¢ãçä¼¼ä¸è§æ³¢ãã­ã¼ããããã¤ãºã«å¯¾å¿ãã¦ããã
		var rightChannel = new SquareSoundChannel(tempo, format, SquareWaveRatio.Point25, PanType.Right);
		var rightChannel2 = new SquareSoundChannel(tempo, format, SquareWaveRatio.Point125, PanType.Right);
		var leftChannel = new TriangleSoundChannel(tempo, format, PanType.Left);

		// ISoundComponentãå®è£ããã¯ã©ã¹ã®ãªãã¸ã§ã¯ãããã£ã³ãã«ã«è¿½å ãã¦ããã
		// ç¾æ®µéã§ã¯æ®éã®é³ç¬¦ãä¼ç¬¦ãã¿ã¤ãé£ç¬¦ãä½¿ããã¨ãã§ããã
		rightChannel.Add(new Note(Scale.C, 5, LengthType.Eighth, isDotted: true));
		rightChannel.Add(new Tie(new Note(Scale.D, 5, LengthType.Eighth), LengthType.Eighth));
		var notes = new List<BasicSoundComponentBase>()
		{
			new Note(Scale.E, 5, LengthType.Eighth),
			new Note(Scale.F, 5, LengthType.Eighth),
			new Note(Scale.G, 5, LengthType.Eighth),
		};
		rightChannel.Add(new Tuplet(notes, LengthType.Quarter));

		rightChannel2.Add(new Note(Scale.C, 4, LengthType.Eighth, isDotted: true));
		rightChannel2.Add(new Note(Scale.D, 4, LengthType.Quarter));
		rightChannel2.Add(new Rest(LengthType.Quarter));

		leftChannel.Add(new Note(Scale.C, 3, LengthType.Eighth, isDotted: true));
		leftChannel.Add(new Note(Scale.D, 3, LengthType.Quarter));
		leftChannel.Add(new Rest(LengthType.Quarter));

		var channels = new List<ISoundChannel>() { rightChannel, rightChannel2, leftChannel };
		// ããã¯ã¹ã¯'StereoMixer'ã¯ã©ã¹ã§è¡ãã 
		return new StereoMixer(channels).Mix();
	}
}


```

## ðè©³ç´°
### åºåå½¢å¼
**ãµã³ããªã³ã°å¨æ³¢æ°**
- 48000Hz
- 44100Hz

**éå­åãããæ°**
- 16bit
- 8bit

**ãã£ã³ãã«æ°**
- Stereo 2ch
- Monaural 1ch

## ðä½ã£ãäººã®ãã¤ãã¿ã¼
[Twitter](https://twitter.com/DTB_AutumnSky)  

## Â©ï¸ã©ã¤ã»ã³ã¹
MIT License

## ðµOverview
You can do The following content with this library.
- make the sound of chiptune
- export sound to a file of wave format.

## ðDocumentation
[Wiki](https://github.com/AutumnSky1010/SoundMaker/wiki)

## â°ï¸Requirement
.NET 6

## â¬Installation
### NuGet

[SoundMaker](https://www.nuget.org/packages/SoundMaker/)

## ð¶Usage
```CSharp
using SoundMaker.Sounds;
using SoundMaker.Sounds.Score;
using SoundMaker.Sounds.SoundChannels;
using SoundMaker.WaveFile;

namespace YourNamespace;
public static class YourClass
{
	private static void Main()
	{
		// make sound format.
		var soundFormat = new SoundFormat(SoundMaker.Sounds.SamplingFrequencyType.FourtyEightKHz, SoundMaker.Sounds.BitRateType.SixteenBit, SoundMaker.Sounds.ChannelType.Stereo);
		StereoWave wave = MakeStereoWave(soundFormat);

		// write.
		var sound = new SoundWaveChunk(wave.GetBytes(soundFormat.BitRate));
		var waveFileFormat = new FormatChunk(SoundMaker.WaveFile.SamplingFrequencyType.FourtyEightKHz, SoundMaker.WaveFile.BitRateType.SixteenBit, SoundMaker.WaveFile.ChannelType.Stereo);
		var writer = new WaveWriter(waveFileFormat, sound);
		string filePath = "sample.wav";
		writer.Write(filePath);
	}

	private static StereoWave MakeStereoWave(SoundFormat format)
	{
		// number of quarter notes per minute.
		int tempo = 100;
		// first, you should make the channel of sound.
		// can use square wave, triangle wave, pseudo triangle wave and low bit noise wave at this stage.
		var rightChannel = new SquareSoundChannel(tempo, format, SquareWaveRatio.Point25, PanType.Right);
		var rightChannel2 = new SquareSoundChannel(tempo, format, SquareWaveRatio.Point125, PanType.Right);
		var leftChannel = new TriangleSoundChannel(tempo, format, PanType.Left);

		// add the object implemented 'ISoundComponent' to the channel.
		// can use Note, Rest, Tuplet, Tie at this stage.
		rightChannel.Add(new Note(Scale.C, 5, LengthType.Eighth, isDotted: true));
		rightChannel.Add(new Tie(new Note(Scale.D, 5, LengthType.Eighth), LengthType.Eighth));
		var notes = new List<BasicSoundComponentBase>()
		{
			new Note(Scale.E, 5, LengthType.Eighth),
			new Note(Scale.F, 5, LengthType.Eighth),
			new Note(Scale.G, 5, LengthType.Eighth),
		};
		rightChannel.Add(new Tuplet(notes, LengthType.Quarter));

		rightChannel2.Add(new Note(Scale.C, 4, LengthType.Eighth, isDotted: true));
		rightChannel2.Add(new Note(Scale.D, 4, LengthType.Quarter));
		rightChannel2.Add(new Rest(LengthType.Quarter));

		leftChannel.Add(new Note(Scale.C, 3, LengthType.Eighth, isDotted: true));
		leftChannel.Add(new Note(Scale.D, 3, LengthType.Quarter));
		leftChannel.Add(new Rest(LengthType.Quarter));

		var channels = new List<ISoundChannel>() { rightChannel, rightChannel2, leftChannel };
		// can mix with StereoMixer class. 
		return new StereoMixer(channels).Mix();
	}
}


```

## ðFeatures
### Output format
**Sampling frequency**
- 48000Hz
- 44100Hz

**Bit rate**
- 16bit
- 8bit

**Number of Channels**
- Stereo 2ch
- Monaural 1ch

## ðAuthor
[Twitter](https://twitter.com/DTB_AutumnSky)

## Â©ï¸License
SoundMaker is licensed under the MIT License.

