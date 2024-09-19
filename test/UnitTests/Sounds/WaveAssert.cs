namespace SoundMakerTests.UnitTests.Sounds;
internal static class WaveAssert
{
    public static void Equal(short waveValue, short[] targetWave)
    {
        foreach (var targetWaveValue in targetWave)
        {
            Assert.Equal(targetWaveValue, waveValue);
        }
    }
}
