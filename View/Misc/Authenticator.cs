using System.Security.Claims;
using Controller.Account;
using Controller.Shared;
using Microsoft.AspNetCore.Components.Authorization;

namespace View.Misc;

public sealed class Authenticator(ICommandSender commander) : AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        AuthCommand command = new();
        var result = await commander.CommandAsync(command);

        if (result.IsSuccessful)
        {
            return new(result.User);
        }

        return new(new ClaimsPrincipal());
    }

    public void Refresh()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}

internal static class AuthenticationStateExtensions
{
    public static long? GetId(this AuthenticationState state)
    {
        return long.TryParse(state.User.FindFirst("id")?.Value, out long id) ? id : null;
    }

    public static string? GetUsername(this AuthenticationState state)
    {
        return state.User.FindFirst("username")?.Value;
    }
}
