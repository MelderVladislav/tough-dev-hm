using UberPopug.Domains.Auth.Services.Codesets;

namespace UberPopug.Domains.Auth.Models;

public class AuthServiceResult<TResult>
{
    public AuthServiceResult()
    {
    }

    public AuthServiceResult(TResult result)
    {
        Result = result;
    }

    public AuthServiceResult(TResult result, ICollection<AuthServiceError> errors) : this(result)
    {
        Errors = errors;
    }

    public AuthServiceResult(AuthServiceError serviceError)
    {
        Errors = new[] { serviceError };
    }

    public AuthServiceResult(AuthErrorCode errorCode)
    {
        Errors = new[] { new AuthServiceError(errorCode) };
    }

    public AuthServiceResult(TResult result, AuthErrorCode errorCode) : this(errorCode)
    {
        Result = result;
    }

    public AuthServiceResult(ICollection<AuthServiceError> errors)
    {
        Errors = errors;
    }

    public TResult Result { get; set; }

    public ICollection<AuthServiceError> Errors { get; set; }

    public bool HasErrors => Errors?.Any() ?? false;
}