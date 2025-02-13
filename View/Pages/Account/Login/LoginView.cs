using Controller.Account;
using Controller.Shared;
using Masa.Blazor;
using Microsoft.AspNetCore.Components;
using View.Misc;

namespace View.Pages.Account.Login;

public sealed partial class LoginView
{
    [Inject]
    public I18n I18n { get; set; } = null!;

    [Inject]
    public NavigationManager Nav { get; set; } = null!;

    [Inject]
    public ICommandSender CommandSender { get; set; } = null!;

    private readonly AutoRestoredState<bool> loading = false;
    private readonly LoginCommand command = new();

    private async Task Submit()
    {
        using var _ = loading.CreateScope(true);

        var result = await CommandSender.CommandAsync(command);
        if (result.IsSuccessful)
        {
            Nav.NavigateTo("/user/" + result.User.FindFirst("id")?.Value);
        }
    }
}
