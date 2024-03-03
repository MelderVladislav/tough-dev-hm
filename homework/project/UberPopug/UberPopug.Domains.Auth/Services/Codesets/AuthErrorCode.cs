using System.ComponentModel;

namespace UberPopug.Domains.Auth.Services.Codesets;

public enum AuthErrorCode
{
    [Description("Internal error.")] InternalError = 1000,

    [Description("User not found.")] UserNotFound = 1001,

    [Description("User is blocked.")] UserIsBlocked = 1002,

    [Description("Wrong password.")] WrongPassword = 1003,

    [Description("Invalid token.")] InvalidToken = 1004,

    [Description("User with this login or email already exists.")]
    ExistingIdentity = 1005,

    [Description("Invalid password lenght.")]
    InvalidPasswordLenght = 1006,

    [Description("Password is empty.")] PasswordIsEmpty = 1007,

    [Description("Login is empty.")] LoginIsEmpty = 1008,

    [Description("Email is empty.")] EmailIsEmpty = 1009,

    [Description("Invalid email.")] InvalidEmail = 1010,

    [Description("Password validation error. Digit required.")]
    DigitRequired = 1011,

    [Description("Password validation error. Special character required.")]
    SpecialCharRequired = 1012,

    [Description("Password validation error. Uppercase character required.")]
    UppercaseCharRequired = 1013,

    [Description("Password validation error. Lowercase character required.")]
    LowercaseCharRequired = 1014,

    [Description("Invalid activation token.")]
    EmailActivationError = 1015,

    [Description("User is not activated.")]
    UserNotActivated = 1016
}