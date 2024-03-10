namespace UberPopug.Domains.Auth.Services.Tokens;

internal interface ITokenFactory
{
    string GenerateToken(int size);
}