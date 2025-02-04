using System.Security.Claims;
using Blazored.LocalStorage;
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
        AuthenticationState state = new(result.User);
        return state;
    }
}
