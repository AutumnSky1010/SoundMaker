using SoundMaker.Sounds.Score;
using System.Diagnostics.CodeAnalysis;

namespace SoundMaker.ScoreData.SMSC;
public class SMSCReadResult
{
    private readonly IReadOnlyList<ISoundComponent>? _value;

    private SMSCReadResult(IReadOnlyList<ISoundComponent>? value, IReadOnlyList<Error> errors)
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
        return new SMSCReadResult(null, errors);
    }

    public IReadOnlyList<Error> Errors { get; }

    public bool IsSuccess => Errors.Count is 0;

    public bool TryGetValue([MaybeNullWhen(false)] out IReadOnlyList<ISoundComponent> value)
    {
        if (_value is not null && IsSuccess)
        {
            value = _value;
            return true;
        }

        value = null;
        return false;
    }

    public IReadOnlyList<ISoundComponent> Unwrap()
    {
        if (!IsSuccess || _value is null)
        {
            throw new NullReferenceException("The result does not contain a valid value.");
        }

        return _value;
    }
}
