using Controller.Account;
using Controller.Notifiers;
using Controller.Shared;
using Controller.User;
using Masa.Blazor;
using Masa.Blazor.Components.Form;
using Microsoft.AspNetCore.Components;
using View.Misc;

namespace View.Pages.User.Profile;

public sealed partial class ProfileUsername
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
        command.Username = Value;
    }

    private MForm form = new();
    private bool dialog = false;
    private readonly AutoRestoredState<bool> loading = false;
    private readonly UpdateUsernameCommand command = new();

    private void Close()
    {
        dialog = false;
    }

    private async Task<bool> Check(string value)
    {
        UsernameCheckQuery query = new(value);
        var result = await QuerySender.QueryAsync(query);
        return result.IsSuccessful;
    }

    private async Task Submit()
    {
        using var _ = loading.CreateScope(true);

        if (await Check(command.Username) == false)
        {
            form.ParseFormValidation(
                [
                    new ValidationResult(
                        nameof(command.Username),
                        I18n.T("That username has been occupied"),
                        ValidationResultTypes.Error
                    ),
                ]
            );

            return;
        }

        var result = await CommandSender.CommandAsync(command);

        if (result.IsSuccessful)
        {
            Close();
            Notifier.Notify();
            Value = command.Username;
        }
    }
}
