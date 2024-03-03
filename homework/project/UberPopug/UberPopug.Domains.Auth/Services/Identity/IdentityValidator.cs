using System.Net.Mail;
using System.Text.RegularExpressions;
using UberPopug.Domains.Auth.API.Configuration;
using UberPopug.Domains.Auth.Models;
using UberPopug.Domains.Auth.Services.Codesets;

namespace UberPopug.Domains.Auth.Services.Identity;

public class IdentityValidator
{
    private readonly IdentityConfiguration identityConfiguration;

    public IdentityValidator(IdentityConfiguration identityConfiguration)
    {
        this.identityConfiguration = identityConfiguration;
    }

    public AuthServiceResult<bool> Validate(string login, string password, string email)
    {
        if (string.IsNullOrEmpty(login)) return new AuthServiceResult<bool>(false, AuthErrorCode.LoginIsEmpty);

        if (string.IsNullOrEmpty(password)) return new AuthServiceResult<bool>(false, AuthErrorCode.PasswordIsEmpty);

        if (string.IsNullOrEmpty(email)) return new AuthServiceResult<bool>(false, AuthErrorCode.EmailIsEmpty);

        var minPasswordLenght = identityConfiguration.IdentityValidationParameters?.MinPasswordLenght ?? 8;

        var maxPasswordLenght = identityConfiguration.IdentityValidationParameters?.MinPasswordLenght ?? 32;

        var hasMinMaxChars = new Regex(@".{" + minPasswordLenght + "," + maxPasswordLenght + "}");

        var errorsList = new List<AuthServiceError>();

        if (!hasMinMaxChars.IsMatch(password))
            errorsList.Add(new AuthServiceError(AuthErrorCode.PasswordIsEmpty,
                $"Password should has minimal {minPasswordLenght} and maximum {maxPasswordLenght} digit lenght"));

        var hasNumber = new Regex(@"[0-9]+");

        if (identityConfiguration.IdentityValidationParameters != null)
        {
            if (identityConfiguration.IdentityValidationParameters.DigitRequired && !hasNumber.IsMatch(password))
                errorsList.Add(new AuthServiceError(AuthErrorCode.DigitRequired));

            var hasUpperChar = new Regex(@"[A-Z]+");

            if (identityConfiguration.IdentityValidationParameters.UpperCaseLetterRequired &&
                !hasUpperChar.IsMatch(password))
                errorsList.Add(new AuthServiceError(AuthErrorCode.UppercaseCharRequired));

            var hasLowerChar = new Regex(@"[a-z]+");

            if (identityConfiguration.IdentityValidationParameters.LowerCaseLetterRequired &&
                !hasLowerChar.IsMatch(password))
                errorsList.Add(new AuthServiceError(AuthErrorCode.LowercaseCharRequired));

            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

            if (identityConfiguration.IdentityValidationParameters.SpecialCharacterRequired &&
                !hasSymbols.IsMatch(password)) errorsList.Add(new AuthServiceError(AuthErrorCode.SpecialCharRequired));

            if (identityConfiguration.IdentityValidationParameters.ValidateEmail && !IsValidEmail(email))
                errorsList.Add(new AuthServiceError(AuthErrorCode.InvalidEmail));
        }

        if (!errorsList.Any()) return new AuthServiceResult<bool>(true);

        return new AuthServiceResult<bool>(false, errorsList);
    }

    private bool IsValidEmail(string emailaddress)
    {
        try
        {
            var m = new MailAddress(emailaddress);

            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }
}