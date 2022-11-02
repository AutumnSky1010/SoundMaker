namespace SoundMaker.WaveFile;
/// <summary>
/// チャンネル数の種類を表す列挙型
/// </summary>
public enum ChannelType : ushort
{
    /// <summary>
    /// モノラル1ch
    /// </summary>
    Monaural = 0x0001,
    /// <summary>
    /// ステレオ2ch
    /// </summary>
    Stereo = 0x0002
}