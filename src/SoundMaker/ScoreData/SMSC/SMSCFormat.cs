using SoundMaker.Sounds.Score;

namespace SoundMaker.ScoreData.SMSC;
/// <summary>
/// The SMSC (SoundMaker SCore) Format
/// </summary>
public static class SMSCFormat
{
    /// <summary>
    /// Reads SMSC data.<br/>
    /// SMSCデータを読み込む。
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static SMSCReadResult Read(string data)
    {
        var lexer = new Lexer(data);
        var result = new Parser(lexer.ReadAll()).Parse();
        return result;
    }

    /// <summary>
    /// Outputs SMSC data.<br/>
    /// SMSCデータを出力する。
    /// </summary>
    /// <param name="components">Sound components to write. 書き込むサウンドコンポーネント</param>
    /// <returns>SMSC data. SMSCデータ</returns>
    public static string Serialize(IEnumerable<ISoundComponent> components)
    {
        return SMSCSerializer.Serialize(components);
    }
}

