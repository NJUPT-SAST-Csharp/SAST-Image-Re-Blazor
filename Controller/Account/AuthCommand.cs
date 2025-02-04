using System.Security.Claims;
using Blazored.LocalStorage;
using Controller.Exceptions;
using Controller.Shared;
using Microsoft.IdentityModel.JsonWebTokens;
using Model.Account;

namespace Controller.Account;

public readonly record struct AuthCommandResult(
    ClaimsPrincipal User,
    bool IsChanged,
    bool IsSuccessful = true
) : IResult
{
    public AuthCommandResult(string jwtTokenString, bool isChanged)
        : this(CreateClaimsPrincipal(jwtTokenString), isChanged) { }

    public static readonly AuthCommandResult Failed = new(new ClaimsPrincipal(), true, false);

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
    public async Task<AuthCommandResult> Handle(
        AuthCommand command,
        CancellationToken cancellationToken
    )
    {
        if (command.Token is null)
        {
            var state = localStorage.GetItem<AuthState>(AuthTokenProvider.LocalStorageKey);
            if (state is null)
            {
                return AuthCommandResult.Failed;
            }

            if (state.ExpiresAt < DateTime.UtcNow)
            {
                var response =
                    await account.RefreshTokenAsync(new(state.RefreshToken), cancellationToken)
                    ?? throw new ResponseNullOrEmptyException();

                localStorage.SetItem(
                    AuthTokenProvider.LocalStorageKey,
                    AuthState.FromJwtToken(response.Content)
                );
                return new AuthCommandResult(response.Content.AccessToken, false);
            }
            return new AuthCommandResult(state.AccessToken, false);
        }

        localStorage.SetItem(
            AuthTokenProvider.LocalStorageKey,
            AuthState.FromJwtToken(command.Token.Value)
        );
        return new AuthCommandResult(command.Token.Value.AccessToken, true);
    }
}

internal sealed record class AuthState(string AccessToken, string RefreshToken, DateTime ExpiresAt)
{
    public static AuthState FromJwtToken(JwtToken token)
    {
        return new(
            token.AccessToken,
            token.RefreshToken,
            DateTime.UtcNow.AddSeconds(token.ExpireIn)
        );
    }
};
