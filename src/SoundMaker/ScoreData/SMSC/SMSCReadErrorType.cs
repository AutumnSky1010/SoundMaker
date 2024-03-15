namespace SoundMaker.ScoreData.SMSC;

/// <summary>
/// Errors of reading SMSC data <br/>
/// SMSCデータ読み込み時のエラー
/// </summary>
public enum SMSCReadErrorType
{
    // OOが無い
    NotFoundComma,
    NotFoundLeftParentheses,
    NotFoundRightParentheses,
    // 不正
    InvalidLength,
    InvalidScale,
    // 未定義の文
    UndefinedStatement,
}
