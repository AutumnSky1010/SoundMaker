namespace SoundMaker.Sounds.WaveTypes;
/// <summary>
/// Provides a base class for a basic wave type to inherit from.
/// </summary>
public abstract class WaveTypeBase
{
    /// <summary>
    /// Generate array of wave data. <br/>波形データの配列を生成する。
    /// </summary>
    /// <param name="format">format of the sound. 音のフォーマット</param>
    /// <param name="length">length of the array. 配列の長さ</param>
    /// <param name="volume">volume 音量（0 ~ 100）</param>
    /// <param name="hertz">hertz of the sound. 音の周波数</param>
    /// <returns>the array of wave data. :  short[]</returns>
    /// <exception cref="ArgumentOutOfRangeException">Length must be non-negative.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Hertz must be non-negative and greater than 0.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Volume must be below than 100 and more than 0.</exception>
    public abstract short[] GenerateWave(SoundFormat format, int length, int volume, double hertz);

    internal abstract WaveTypeBase Clone();

    protected void CheckGenerateWaveArgs(int length, int volume, double hertz)
    {
        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "'length' must be non-negative.");
        }

        CheckGenerateUnitWaveArgs(volume, hertz);
    }

    protected static void CheckGenerateUnitWaveArgs(int volume, double hertz)
    {
        if (volume is > 100 or < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(volume), "'volume must be below than 100 and more than 0.");
        }
        if (hertz <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(hertz), "'hertz must be non-negative and greater than 0.");
        }
    }
}
