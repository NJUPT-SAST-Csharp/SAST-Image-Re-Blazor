using Controller.Notifiers;
using Controller.Shared;
using Controller.User;
using Masa.Blazor;
using Microsoft.AspNetCore.Components;
using View.Misc;

namespace View.Pages.User.Profile;

public sealed partial class ProfileBiography
{
    [Inject]
    public ICommandSender CommandSender { get; set; } = null!;

    [Inject]
    public IQuerySender QuerySender { get; set; } = null!;

    [Inject]
    public IRequestSuccessNotifier Notifier { get; set; } = null!;

    [Inject]
    public I18n I18n { get; set; } = null!;

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object> InputAttributes { get; set; } = [];

    [Parameter, EditorRequired]
    public string Value { get; set; } = string.Empty;

    [Parameter]
    public bool Editable { get; set; } = false;

    protected override void OnParametersSet()
    {
        command.Biography = Value;
    }

    private bool dialog = false;
    private readonly AutoRestoredState<bool> loading = false;
    private readonly UpdateBiographyCommand command = new();

    private void Close()
    {
        dialog = false;
    }

    private async Task Submit()
    {
        using var _ = loading.CreateScope(true);

        var result = await CommandSender.CommandAsync(command);

        if (result.IsSuccessful)
        {
            Notifier.Notify();
            Value = command.Biography;
            Close();
        }
    }
}
