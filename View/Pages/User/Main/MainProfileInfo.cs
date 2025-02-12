using Controller.Account;
using Controller.Shared;
using Masa.Blazor;
using Microsoft.AspNetCore.Components;

namespace View.Pages.User.Main;

public sealed partial class MainProfileInfo
{
    [Inject]
    public I18n I18n { get; set; } = null!;

    [Inject]
    public ICommandSender CommandSender { get; set; } = null!;

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object> InputAttributes { get; set; } = [];

    [Parameter, EditorRequired]
    public long Id { get; set; }

    [Parameter, EditorRequired]
    public ProfileData Profile { get; set; }

    private async Task Logout()
    {
        await CommandSender.CommandAsync(new LogoutCommand());
    }

    public readonly record struct ProfileData(string Username, string Biography);
}
