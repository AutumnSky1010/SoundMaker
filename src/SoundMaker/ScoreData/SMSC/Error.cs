namespace SoundMaker.ScoreData.SMSC;
/// <summary>
/// Error of reading SMSC data<br/>
/// SMSCデータの読み取り結果
/// </summary>
public record Error
{
    internal Error(SMSCReadErrorType type, Token? token)
    {
        Type = type;
        LineNumber = token?.LineNumber ?? 0;
        Literal = token?.Literal ?? "";
    }

    /// <summary>
    /// Type of errors <br/>
    /// エラーの種類
    /// </summary>
    public SMSCReadErrorType Type { get; }

    /// <summary>
    /// String of error location <br/>
    /// エラー箇所の文字列
    /// </summary>
    public string Literal { get; }

    /// <summary>
    /// Line number of error.<br/>
    /// エラー箇所の行番号
    /// </summary>
    public int LineNumber { get; }
}
