using System;
using Controller.Account;
using Masa.Blazor;
using Microsoft.AspNetCore.Components;

namespace View.Pages.Account.Register;

public sealed partial class RegisterView
{
    [Inject]
    public I18n I18n { get; set; } = null!;

    [Inject]
    public NavigationManager Nav { get; set; } = null!;

    private RegisterCommand command = new();
    private bool loading = false;

    private async Task Submit() { }
}
