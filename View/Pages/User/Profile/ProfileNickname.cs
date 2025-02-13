using Controller.Notifiers;
using Controller.Shared;
using Controller.User;
using Masa.Blazor;
using Microsoft.AspNetCore.Components;
using View.Misc;

namespace View.Pages.User.Profile;

public sealed partial class ProfileNickname
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
        command.Nickname = Value;
    }

    private readonly AutoRestoredState<bool> loading = false;
    private readonly UpdateNicknameCommand command = new();
    private bool dialog = false;

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
            Close();
            Notifier.Notify();
            Value = command.Nickname;
        }
    }
}
