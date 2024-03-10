using SoundMaker.Sounds.Score;

namespace SoundMaker.ScoreData.SMSC;
public class SMSCReadResult
{
    private readonly IReadOnlyList<ISoundComponent> _value;

    private SMSCReadResult(IReadOnlyList<ISoundComponent> value, IReadOnlyList<Error> errors)
    {
        _value = value;
        Errors = errors;
    }

    internal static SMSCReadResult Success(IReadOnlyList<ISoundComponent> value)
    {
        return new SMSCReadResult(value, Array.Empty<Error>());
    }

    internal static SMSCReadResult Failure(IReadOnlyList<Error> errors)
    {
        return new SMSCReadResult(Array.Empty<ISoundComponent>(), errors);
    }

    /// <summary>
    /// Errors when reading SMSC.<br/>
    /// SMSCを読み込んだ際のエラー
    /// </summary>
    public IReadOnlyList<Error> Errors { get; }

    /// <summary>
    /// Whether the reading was successful.<br/>
    /// 読み込みに成功したか
    /// </summary>
    public bool IsSuccess => Errors.Count is 0;

    /// <summary>
    /// Returns whether the reading was successful and, if successful, returns the result. If it fails, an empty array will be in value.<br/>
    /// 読み込みに成功したかを返し、成功した場合は結果を返す。失敗時は空の配列がvalueに入る。
    /// </summary>
    /// <param name="value">On success: result, On failure: empty array</param>
    /// <returns>On success: true, On failure: false</returns>
    public bool TryGetValue(out IReadOnlyList<ISoundComponent> value)
    {
        if (IsSuccess)
        {
            value = _value;
            return true;
        }

        value = Array.Empty<ISoundComponent>();
        return false;
    }

    /// <summary>
    /// Returns the result assuming the reading was successful. If it fails, an empty array is returned. <br/>
    /// 読み込みに成功した前提で結果を返す。失敗時は空の配列が戻る。
    /// </summary>
    /// <returns>On success: result, On failure: empty array</returns>
    public IReadOnlyList<ISoundComponent> Unwrap()
    {
        if (!IsSuccess)
        {
            return Array.Empty<ISoundComponent>();
        }

        return _value;
    }

}
