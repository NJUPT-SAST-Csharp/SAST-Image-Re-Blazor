using System.Security.Claims;
using Blazored.LocalStorage;
using Controller.Notifiers;
using Controller.Shared;
using Microsoft.IdentityModel.JsonWebTokens;
using Model.Account;

namespace Controller.Account;

public readonly record struct AuthCommandResult(ClaimsPrincipal User, bool IsSuccessful = true)
    : IResult
{
    public AuthCommandResult(string jwtTokenString)
        : this(CreateClaimsPrincipal(jwtTokenString)) { }

    public static readonly AuthCommandResult Failed = new(new ClaimsPrincipal(), false);

    private static ClaimsPrincipal CreateClaimsPrincipal(string jwtTokenString)
    {
        JsonWebToken jwt = new(jwtTokenString);
        ClaimsIdentity identity = new(jwt.Claims, JwtConstants.TokenType);
        return new(identity);
    }
};

public sealed record AuthCommand(JwtToken? Token = null) : ICommandRequest<AuthCommandResult> { }

internal sealed class AuthRequestHandler(
    ILocalStorageService localStorage,
    IAccountAPI account,
    IAuthStateChangedNotifier notifier
) : ICommandRequestHandler<AuthCommand, AuthCommandResult>
{
    public async Task<AuthCommandResult> Handle(
        AuthCommand command,
        CancellationToken cancellationToken
    )
    {
        if (command.Token is null)
        {
            var state = await localStorage.GetItemAsync<AuthState>(
                AuthTokenProvider.LocalStorageKey,
                cancellationToken
            );
            if (state is null)
            {
                return AuthCommandResult.Failed;
            }

            if (state.ExpiresAt < DateTime.UtcNow)
            {
                _ = account
                    .RefreshTokenAsync(new(state.RefreshToken), cancellationToken)
                    .ContinueWith(
                        async task =>
                        {
                            var result = await task;
                            if (result.IsSuccessful)
                            {
                                var state = AuthState.FromJwtToken(result.Content);
                                await localStorage.SetItemAsync(
                                    AuthTokenProvider.LocalStorageKey,
                                    state
                                );
                                notifier.Notify();
                            }
                        },
                        cancellationToken
                    );
            }

            return new AuthCommandResult(state.AccessToken);
        }

        await localStorage.SetItemAsync(
            AuthTokenProvider.LocalStorageKey,
            AuthState.FromJwtToken(command.Token.Value),
            cancellationToken
        );

        var result = new AuthCommandResult(command.Token.Value.AccessToken);
        notifier.Notify();
        return result;
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
