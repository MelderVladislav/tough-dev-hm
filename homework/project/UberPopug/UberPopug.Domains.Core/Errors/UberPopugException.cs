namespace UberPopug.Domains.Core.Errors;

public class UberPopugException : Exception
{
    public ICollection<ErrorDescription> Errors { get; set; }

    public bool ShouldLog { get; set; }

    public UberPopugException(ErrorDescription serviceError)
    {
        Errors = new[] { serviceError };
    }

    public UberPopugException(Enum errorCode,
        string? additionalMessage = null,
        Exception? exception = null,
        bool shouldLog = false)
    {
        Errors = new[] { new ErrorDescription(errorCode, additionalMessage, exception) };
        ShouldLog = shouldLog;
    }

    public UberPopugException(ICollection<ErrorDescription> errors)
    {
        Errors = errors;
    }
}