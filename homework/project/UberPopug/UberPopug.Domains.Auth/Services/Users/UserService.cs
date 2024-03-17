using UberPopug.Domains.Auth.Models;
using UberPopug.Domains.Auth.Models.Token;
using UberPopug.Domains.Auth.Services.Context;
using UberPopug.Domains.Auth.Services.Identity;
using UberPopug.Domains.Core.Entities;
using UberPopug.Domains.Core.Errors;
using UberPopug.Domains.Core.Events;

namespace UberPopug.Domains.Auth.Services.Users;

internal class UserService : IUserService
{
    private readonly IIdentityService<User> identityService;
    private readonly ILoggingEventBus eventBus;

    public UserService(IIdentityService<User> identityService,
        ILoggingEventBus eventBus,
        IUserContext userContext)
    {
        this.identityService = identityService;
        this.eventBus = eventBus;
    }

    public async Task<TokenPair> Refresh(string accessToken, string oldToken)
    {
        var serviceResult = await identityService.UpdateRefreshToken(oldToken, accessToken);

        return HandleErrors(serviceResult);
    }

    public async Task<Guid?> Register(string login, string email, string password, string role)
    {
        var userAddResult = await identityService.AddUser(login, email, password, [role]);

        HandleErrors(userAddResult);

        if (!userAddResult.HasErrors)
        {
            eventBus.Publish(new UserCreated { UserId = userAddResult.Result.Id, Login = userAddResult.Result.Login, Role = role});
        }

        return userAddResult.Result.Id;
    }

    public async Task<TokenPair> Login(string login, string password)
    {
        var loginResult = await identityService.LoginUser(login, password);

        return HandleErrors(loginResult);
    }

    private TResult HandleErrors<TResult>(AuthServiceResult<TResult> authServiceResult)
    {
        if (authServiceResult.HasErrors)
        {
            var errors = authServiceResult.Errors?
                .Select(e => new ErrorDescription((ErrorCode)e.ErrorCode, e.AdditionalMessage))
                .ToList();

            throw new UberPopugException(errors);
        }

        return authServiceResult.Result;
    }
}