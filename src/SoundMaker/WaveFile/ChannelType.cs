namespace SoundMaker.WaveFile;
/// <summary>
/// The type which is expressed channels count of the sound. <br/>チャンネル数の種類を表す列挙型
/// </summary>
public enum ChannelType : ushort
{
    /// <summary>
    /// monaural(1ch)<br/> モノラル1ch
    /// </summary>
    Monaural = 0x0001,
    /// <summary>
    /// stereo(2ch)<br/> ステレオ2ch
    /// </summary>
    Stereo = 0x0002
}