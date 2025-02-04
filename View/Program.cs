using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Controller;
using Masa.Blazor;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Model;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using View;
using View.Misc;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddViewTransition();

builder
    .Services.AddAuthorizationCore()
    .AddCascadingAuthenticationState()
    .AddSingleton<AuthenticationStateProvider, Authenticator>();

builder.Services.AddMediatRControllers().AddControllerLayerI18nText<I18nTextAdapter>();
builder.Services.AddHttpClients("http://localhost:5265/api");
builder.Services.AddSnackbarsService().AddRequestExceptionNotify();

await builder
    .Services.AddMasaBlazor()
    .AddI18nForWasmAsync($"{builder.HostEnvironment.BaseAddress}/i18n");

await builder.Build().RunAsync();
