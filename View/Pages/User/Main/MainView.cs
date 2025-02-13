using Controller.Shared;
using Controller.User;
using Masa.Blazor;
using Microsoft.AspNetCore.Components;
using View.Misc;

namespace View.Pages.User.Main;

public sealed partial class MainView
{
    [Inject]
    public I18n I18n { get; set; } = null!;

    [Inject]
    public IQuerySender QuerySender { get; set; } = null!;

    [Inject]
    public NavigationManager Nav { get; set; } = null!;

    [Inject]
    public Authenticator Authenticator { get; set; } = null!;

    [Parameter]
    public long Id { get; set; }

    private UserProfileQueryResult profile;
    private readonly AutoRestoredState<bool> loading = false;

    protected override async Task OnInitializedAsync()
    {
        using var _ = loading.CreateScope(true);

        var response = await QuerySender.QueryAsync(new UserProfileQuery(Id));
        if (response.IsSuccessful)
        {
            profile = response;
        }
        else
        {
            Nav.NavigateTo("/404");
        }
    }
}
