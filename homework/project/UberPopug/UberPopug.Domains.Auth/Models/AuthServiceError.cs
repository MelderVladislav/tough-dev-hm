using UberPopug.Domains.Auth.Services.Codesets;
using UberPopug.Domains.Auth.Utility.Enums;

namespace UberPopug.Domains.Auth.Models;

public class AuthServiceError
{
    public AuthServiceError(AuthErrorCode error, string? additionalMessage = null)
    {
        ErrorCode = error;
        ErrorMessage = error.GetDescription();
        AdditionalMessage = additionalMessage;
    }

    public AuthErrorCode ErrorCode { get; set; }

    public string? ErrorMessage { get; set; }

    public string? AdditionalMessage { get; set; }
}