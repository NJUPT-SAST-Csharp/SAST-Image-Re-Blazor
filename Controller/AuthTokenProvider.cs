using System;
using Blazored.LocalStorage;
using Controller.Account;
using Controller.Shared;
using Model;
using Model.Account;

namespace Controller;

public sealed class AuthTokenProvider(ILocalStorageService provider, ICommandSender sender)
    : IAuthTokenProvider
{
    public const string LocalStorageKey = "auth";

    async Task<string?> IAuthTokenProvider.GetAsync()
    {
        var state = await provider.GetItemAsync<AuthState>(LocalStorageKey);

        if (state is null)
        {
            return null;
        }
        if (state.ExpiresAt < DateTime.UtcNow)
        {
            await sender.CommandAsync(new AuthCommand());
            state = await provider.GetItemAsync<AuthState>(LocalStorageKey);
            return state?.AccessToken;
        }

        return state.AccessToken;
    }
}
