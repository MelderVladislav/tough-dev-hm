using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.Extensions.Options;
using UberPopug.Domains.Auth.API.Configuration;
using UberPopug.Domains.Auth.Models.Token;

namespace UberPopug.Domains.Auth.Services.Tokens;

internal sealed class JwtFactory : IJwtFactory
{
    private readonly JwtIssuerOptions jwtOptions;
    private readonly IJwtTokenHandler jwtTokenHandler;

    public JwtFactory(IJwtTokenHandler jwtTokenHandler, IOptions<JwtIssuerOptions> jwtOptions,
        IdentityConfiguration identityConfiguration)
    {
        this.jwtTokenHandler = jwtTokenHandler;
        this.jwtOptions = jwtOptions.Value;
        this.jwtOptions.ValidFor = TimeSpan.FromMinutes(identityConfiguration.JwtTokenLifetimeInMinutes);

        ThrowIfInvalidOptions(this.jwtOptions);
    }

    public async Task<AccessToken> GenerateEncodedToken(Guid userId, string[] roles, string userLogin, string language)
    {
        var identity = GenerateClaimsIdentity(userId, roles, userLogin);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userLogin),
            new Claim(JwtRegisteredClaimNames.Jti, await jwtOptions.JtiGenerator()),
            new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(jwtOptions.IssuedAt).ToString(),
                ClaimValueTypes.Integer64),
            identity.FindFirst("id")
        }.Concat(identity.FindAll(ClaimTypes.Role));

        // Create the JWT security token and encode it.
        var jwt = new JwtSecurityToken(
            jwtOptions.Issuer,
            jwtOptions.Audience,
            claims,
            jwtOptions.NotBefore,
            jwtOptions.Expiration,
            jwtOptions.SigningCredentials);

        return new AccessToken(jwtTokenHandler.WriteToken(jwt), (int)jwtOptions.ValidFor.TotalSeconds);
    }

    private static ClaimsIdentity GenerateClaimsIdentity(Guid id, string[] roles, string userName)
    {
        var rolesClaims = roles.Select(r => new Claim(ClaimTypes.Role, r));
        return new ClaimsIdentity(new GenericIdentity(userName, "Token"), new[]
            {
                new Claim("id", id.ToString())
            }
            .Concat(rolesClaims));
    }

    /// <returns>Date converted to seconds since Unix epoch (Jan 1, 1970, midnight UTC).</returns>
    private static long ToUnixEpochDate(DateTime date)
    {
        return (long)Math.Round((date.ToUniversalTime() -
                                 new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
            .TotalSeconds);
    }

    private static void ThrowIfInvalidOptions(JwtIssuerOptions options)
    {
        if (options == null) throw new ArgumentNullException(nameof(options));

        if (options.ValidFor <= TimeSpan.Zero)
            throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtIssuerOptions.ValidFor));

        if (options.SigningCredentials == null)
            throw new ArgumentNullException(nameof(JwtIssuerOptions.SigningCredentials));

        if (options.JtiGenerator == null) throw new ArgumentNullException(nameof(JwtIssuerOptions.JtiGenerator));
    }
}