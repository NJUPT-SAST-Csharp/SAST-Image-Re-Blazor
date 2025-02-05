using Controller.Shared;
using Controller.User;
using Masa.Blazor;
using Microsoft.AspNetCore.Components;
using View.Misc;

namespace View.Pages.User.Main;

public sealed partial class MainView
{
    [Parameter]
    public long Id { get; set; }

    [Inject]
    public I18n I18n { get; set; } = null!;

    [Inject]
    public IQuerySender QuerySender { get; set; } = null!;

    [Inject]
    public NavigationManager Nav { get; set; } = null!;

    private string username = string.Empty;
    private string biography = string.Empty;

    private readonly AutoRestoredState<bool> loading = false;

    protected override async Task OnInitializedAsync()
    {
        using var _ = loading.CreateScope(true);

        var response = await QuerySender.QueryAsync(new UserProfileQuery(Id));
        if (response.IsSuccessful)
        {
            username = response.Username;
            biography = response.Biography;
        }
        else
        {
            Nav.NavigateTo("/404", true);
        }
    }
}
