namespace SoundMakerTests.UnitTests.Sounds;
internal static class WaveAssert
{
    public static void Equal(ushort waveValue, ushort[] targetWave)
    {
        foreach (var targetWaveValue in targetWave)
        {
            Assert.Equal(targetWaveValue, waveValue);
        }
    }
}
