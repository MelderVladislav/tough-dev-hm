﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using UberPopug.Domains.Auth.API.Configuration;
using UberPopug.Domains.Auth.Models.Token;
using UberPopug.Domains.Auth.Services.Tokens;

namespace UberPopug.Domains.Auth.Services.Security;

internal class TokenService : ITokenService
{
    private readonly IdentityConfiguration identityConfiguration;
    private readonly IJwtFactory jwtFactory;

    private readonly ITokenFactory tokenFactory;

    public TokenService(
        IJwtFactory jwtFactory,
        ITokenFactory tokenFactory,
        IdentityConfiguration identityConfiguration)
    {
        this.jwtFactory = jwtFactory;
        this.tokenFactory = tokenFactory;
        this.identityConfiguration = identityConfiguration;
    }

    public async Task<TokenPair> GenerateTokenPair(Guid userId, string login, string[] roles, string language)
    {
        var refreshToken = tokenFactory.GenerateToken(identityConfiguration.RefreshTokenSize);

        var jwtToken = await jwtFactory.GenerateEncodedToken(userId, roles, login, language);

        return new TokenPair(jwtToken.Token, refreshToken, jwtToken.ExpiresIn);
    }

    public (ClaimsPrincipal, JwtSecurityToken) DecodeJwtToken(string token)
    {
        if (string.IsNullOrWhiteSpace(token)) return (null, null);

        try
        {
            var principal = new JwtSecurityTokenHandler()
                .ValidateToken(token,
                    new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = identityConfiguration.Issuer,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.ASCII.GetBytes(identityConfiguration.JwtSecret)),
                        ValidAudience = identityConfiguration.Audience,
                        ValidateAudience = true,
                        ValidateLifetime = false,
                        ClockSkew = TimeSpan.FromMinutes(1)
                    },
                    out var validatedToken);

            return (principal, validatedToken as JwtSecurityToken);
        }
        catch (Exception ex)
        {
            return (null, null);
        }
    }
}