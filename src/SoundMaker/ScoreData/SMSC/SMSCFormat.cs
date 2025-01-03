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
    /// <param name="data">SMSC data. <br/>SMSCデータ</param>
    /// <returns>The read results. <br/>読み取り結果</returns>
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
    /// <param name="components">Sound components to write. <br/>書き込むサウンドコンポーネント</param>
    /// <returns>SMSC data. <br/>SMSCデータ</returns>
    public static string Serialize(IEnumerable<ISoundComponent> components)
    {
        return SMSCSerializer.Serialize(components);
    }
}

