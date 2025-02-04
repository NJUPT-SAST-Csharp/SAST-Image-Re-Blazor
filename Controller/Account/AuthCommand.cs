using System.Security.Claims;
using Blazored.LocalStorage;
using Controller.Shared;
using Microsoft.IdentityModel.JsonWebTokens;
using Model.Account;

namespace Controller.Account;

public readonly record struct AuthCommandResult(ClaimsPrincipal User, bool IsChanged)
{
    public AuthCommandResult(string jwtTokenString, bool isChanged)
        : this(CreateClaimsPrincipal(jwtTokenString), isChanged) { }

    public static readonly AuthCommandResult Failed = new(new ClaimsPrincipal(), true);

    private static ClaimsPrincipal CreateClaimsPrincipal(string jwtTokenString)
    {
        JsonWebToken jwt = new(jwtTokenString);
        ClaimsIdentity identity = new(jwt.Claims, JwtConstants.TokenType);
        return new(identity);
    }
};

public sealed record AuthCommand(JwtToken? Token = null) : ICommandRequest<AuthCommandResult> { }

internal sealed class AuthRequestHandler(ISyncLocalStorageService localStorage, IAccountAPI account)
    : ICommandRequestHandler<AuthCommand, AuthCommandResult>
{
    const string LocalStorageKey = "auth";

    public async Task<AuthCommandResult> Handle(
        AuthCommand command,
        CancellationToken cancellationToken
    )
    {
        if (command.Token is null)
        {
            var state = localStorage.GetItem<AuthState>(LocalStorageKey);
            if (state is null)
            {
                return AuthCommandResult.Failed;
            }

            if (state.ExpiresAt < DateTime.UtcNow)
            {
                var response = await account.RefreshTokenAsync(
                    new(state.RefreshToken),
                    cancellationToken
                );
                if (response is null || response.IsSuccessful == false)
                {
                    return AuthCommandResult.Failed;
                }
                localStorage.SetItem(LocalStorageKey, response.Content);
                return new AuthCommandResult(response.Content.AccessToken, false);
            }
            return new AuthCommandResult(state.AccessToken, false);
        }

        localStorage.SetItem(LocalStorageKey, new AuthState(command.Token.Value));
        return new AuthCommandResult(command.Token.Value.AccessToken, true);
    }
}

internal sealed record class AuthState(string AccessToken, string RefreshToken, DateTime ExpiresAt)
{
    public AuthState(JwtToken token)
        : this(token.AccessToken, token.RefreshToken, DateTime.UtcNow.AddSeconds(token.ExpireIn))
    { }
};
