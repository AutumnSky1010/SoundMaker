namespace SoundMaker.ScoreData.SMSC;

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
