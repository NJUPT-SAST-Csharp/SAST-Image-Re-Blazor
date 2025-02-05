using Controller.Account;
using Controller.Shared;
using Masa.Blazor;
using Masa.Blazor.Components.Form;
using Microsoft.AspNetCore.Components;

namespace View.Pages.Account.Register;

public sealed partial class RegisterView
{
    [Inject]
    public I18n I18n { get; set; } = null!;

    [Inject]
    public NavigationManager Nav { get; set; } = null!;

    [Inject]
    public ICommandSender CommandSender { get; set; } = null!;

    [Inject]
    public IQuerySender QuerySender { get; set; } = null!;

    private readonly RegisterCommand command = new();
    private bool loading = false;

    private MForm form = new();

    private async Task<bool> Check()
    {
        UsernameCheckQuery query = new(command.Username);
        var result = await QuerySender.QueryAsync(query);
        return result.IsSuccessful;
    }

    private async Task Submit()
    {
        loading = true;

        if (await Check() == false)
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

            loading = false;
            return;
        }

        var result = await CommandSender.CommandAsync(command);

        if (result.IsSuccessful)
        {
            // TODO
        }

        loading = false;
    }
}
