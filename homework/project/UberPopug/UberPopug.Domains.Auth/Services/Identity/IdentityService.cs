using Microsoft.IdentityModel.Tokens;
using UberPopug.Domains.Auth.API.Configuration;
using UberPopug.Domains.Auth.Models;
using UberPopug.Domains.Auth.Models.Token;
using UberPopug.Domains.Auth.Models.User;
using UberPopug.Domains.Auth.Services.Codesets;
using UberPopug.Domains.Auth.Services.Security;
using UberPopug.Domains.Auth.Services.Stores;

namespace UberPopug.Domains.Auth.Services.Identity;

internal class IdentityService<T> : IIdentityService<T> where T : class, IUser, new()
{
    private readonly IdentityConfiguration identityConfiguration;
    private readonly IdentityConfirmationService identityConfirmationService;
    private readonly IdentityValidator identityValidator;
    private readonly IPasswordService passwordService;
    private readonly IRefreshTokenStore refreshTokenStore;
    private readonly ITokenService tokenService;
    private readonly IUserStore<T> userStoreService;

    public IdentityService(
        IUserStore<T> userStoreService,
        IRefreshTokenStore refreshTokenStore,
        IPasswordService passwordService,
        IdentityValidator identityValidator,
        IdentityConfiguration identityConfiguration,
        IdentityConfirmationService identityConfirmationService,
        ITokenService tokenService)
    {
        this.userStoreService = userStoreService;
        this.refreshTokenStore = refreshTokenStore;
        this.passwordService = passwordService;
        this.tokenService = tokenService;
        this.identityValidator = identityValidator;
        this.identityConfiguration = identityConfiguration;
        this.identityConfirmationService = identityConfirmationService;
    }

    public async Task<AuthServiceResult<T>>
        AddUser(string login, string email, string password,
            string[] roles) // TODO: Add validation for password and email (in parameters)
    {
        var emailSettings = identityConfiguration.EmailSettings;
        var shouldConfirmEmail = emailSettings?.ShouldConfirmEmail ?? false;

        var userId = Guid.NewGuid();

        var user = new T
        {
            Id = userId,
            Login = login,
            Email = email,
            PasswordHash = passwordService.CreateHashFromPassword(userId, password),
            IsActive = shouldConfirmEmail
        };

        try
        {
            var validationResult = identityValidator.Validate(login, password, email);

            if (validationResult.Result == false) return new AuthServiceResult<T>(validationResult.Errors);

            var addResult = await userStoreService.InsertUser(user, roles);

            if (addResult.Errors?.Any() ?? false) return new AuthServiceResult<T>(addResult.Errors);

            if (shouldConfirmEmail)
                await identityConfirmationService.SendConfirmationEmail(userId, user.Email, "Активация профиля");
        }
        catch (Exception e)
        {
            return new AuthServiceResult<T>(new[] { new AuthServiceError(AuthErrorCode.InternalError, e.Message) });
        }

        return new AuthServiceResult<T>(user);
    }

    public async Task<AuthServiceResult<TokenPair>> LoginUser(string login, string password)
    {
        var user = await userStoreService.FindUserByLogin(login);

        if (user == null) return new AuthServiceResult<TokenPair>(AuthErrorCode.UserNotFound);

        var shouldConfirmEmail = identityConfiguration.EmailSettings?.ShouldConfirmEmail ?? false;

        if (shouldConfirmEmail && user.IsActive == false)
            return new AuthServiceResult<TokenPair>(AuthErrorCode.UserNotActivated);

        var loginError = passwordService.HandleUserLoginAttempt(password, user);

        if (loginError != null)
        {
            await userStoreService.UpdateUser(user);

            return new AuthServiceResult<TokenPair>(loginError);
        }

        var userRolesNames = await userStoreService.GetUserRoleNames(user.Id);

        var tokenPair = await tokenService.GenerateTokenPair(user.Id, user.Login, userRolesNames, user.DefaultLanguage);

        await refreshTokenStore.AddRefreshToken(tokenPair.RefreshToken, user.Id);

        return new AuthServiceResult<TokenPair>(tokenPair);
    }

    public async Task<AuthServiceResult<TokenPair>> UpdateRefreshToken(string refreshToken, string accessToken)
    {
        var (principal, jwtToken) = tokenService.DecodeJwtToken(accessToken);

        if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256)) // get from config
            throw new ArgumentException("Invalid token");

        var userId = Guid.Parse(principal.Claims.FirstOrDefault(claim => claim.Type == "id").Value); // Ensure id

        var user = await userStoreService.FindUserById(userId);

        if (user == null) return new AuthServiceResult<TokenPair>(new AuthServiceError(AuthErrorCode.UserNotFound));

        var isTokenValid = await refreshTokenStore.CheckRefreshToken(refreshToken, user.Id);

        if (!isTokenValid) return new AuthServiceResult<TokenPair>(AuthErrorCode.InvalidToken);

        var userRolesNames = await userStoreService.GetUserRoleNames(user.Id);

        var tokenPair = await tokenService.GenerateTokenPair(user.Id, user.Login, userRolesNames, user.DefaultLanguage);

        await refreshTokenStore.AddRefreshToken(tokenPair.RefreshToken, user.Id);

        await refreshTokenStore.RemoveRefreshToken(refreshToken);

        return new AuthServiceResult<TokenPair>(tokenPair);
    }

    public async Task<AuthServiceResult<bool>> TryActivateUser(string token)
    {
        var activated = await identityConfirmationService.TryConfirmEmail(token);

        if (activated) return new AuthServiceResult<bool>(true);

        return new AuthServiceResult<bool>(false, AuthErrorCode.EmailActivationError);
    }
}