using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using Controller.Account;
using Controller.Shared;
using Microsoft.AspNetCore.Components.Authorization;

namespace View.Misc;

public sealed class Authenticator(ICommandSender commander) : AuthenticationStateProvider
{
    [MemberNotNullWhen(true, nameof(Username), nameof(Id))]
    public bool IsAuthenticated { get; private set; } = false;

    public long? Id { get; private set; } = null;

    public string? Username { get; private set; } = null;

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        AuthCommand command = new();
        var result = await commander.CommandAsync(command);

        var user = result.User ?? new ClaimsPrincipal();

        IsAuthenticated = user.Identity is not null && user.Identity.IsAuthenticated;
        Id = long.TryParse(user.FindFirst("id")?.Value, out long id) ? id : null;
        Username = user.FindFirst("username")?.Value;

        return new(user);
    }

    public void Refresh()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}
