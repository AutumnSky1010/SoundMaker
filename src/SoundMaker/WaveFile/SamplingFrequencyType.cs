namespace SoundMaker.WaveFile;
/// <summary>
/// The type which is expressed sampling frequency of the sound. <br/>サンプリング周波数の種類を表す列挙型
/// </summary>
public enum SamplingFrequencyType : uint
{
    /// <summary>
    /// 48000Hz
    /// </summary>
    FourtyEightKHz = 0x0000BB80,
    /// <summary>
    /// 44100Hz
    /// </summary>
    FourtyFourKHz = 0x0000AC44
}
