using Controller.Notifiers;
using Controller.Shared;
using Controller.User;
using Masa.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Model.User;
using View.Misc;

namespace View.Pages.User.Main;

public sealed partial class MainHeader
{
    [Inject]
    public ICommandSender CommandSender { get; set; } = null!;

    [Inject]
    public IRequestSuccessNotifier Notifier { get; set; } = null!;

    [Inject]
    public Authenticator Authenticator { get; set; } = null!;

    [Inject]
    public I18n I18n { get; set; } = null!;

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object> InputAttributes { get; set; } = [];

    [Parameter, EditorRequired]
    public long Id { get; set; }

    public string Header => UserProfileSources.Header(Id) + $"?t={DateTime.UtcNow.Ticks}";

    private readonly AutoRestoredState<bool> loading = false;
    private AuthenticationState authState = null!;

    protected override async Task OnInitializedAsync()
    {
        var state = await Authenticator.GetAuthenticationStateAsync();
        authState = state;
    }

    private async Task Upload(InputFileChangeEventArgs e)
    {
        using var _ = loading.CreateScope(true);

        UpdateHeaderCommand command = new(e.File.OpenReadStream());
        await CommandSender.CommandAsync(command);

        Notifier.Notify();
        StateHasChanged();
    }
}
