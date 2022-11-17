namespace SoundMaker.WaveFile;
/// <summary>
/// type of channels count. チャンネル数の種類を表す列挙型
/// </summary>
public enum ChannelType : ushort
{
    /// <summary>
    /// monaural(1ch) モノラル1ch
    /// </summary>
    Monaural = 0x0001,
    /// <summary>
    /// stereo(2ch) ステレオ2ch
    /// </summary>
    Stereo = 0x0002
}