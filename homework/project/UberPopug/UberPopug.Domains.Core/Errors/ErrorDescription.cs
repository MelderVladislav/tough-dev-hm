using System.ComponentModel;

namespace UberPopug.Domains.Core.Errors;

public class ErrorDescription
{
    public Enum ErrorCode { get; }

    public string ErrorMessage { get; }

    public string? AdditionalMessage { get; }

    public object? Payload { get; }

    public ErrorDescription(Enum error, string? additionalMessage = null, object? payload = null)
    {
        ErrorCode = error;
        ErrorMessage = GetDescription(error);
        AdditionalMessage = additionalMessage;
        Payload = payload;
    }

    public string? GetDescription(Enum enumVal)
    {
        var type = enumVal.GetType();
        var memInfo = type.GetMember(enumVal.ToString());
        var attributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

        return attributes.Length > 0 ? (attributes[0] as DescriptionAttribute)?.Description : null;
    }
}