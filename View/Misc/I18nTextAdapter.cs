using System;
using Controller;
using Masa.Blazor;

namespace View.Misc;

public sealed class I18nTextAdapter(IServiceProvider provider) : II18nText
{
    public string T(string? key, params object[] args)
    {
        using var scope = provider.CreateScope();

        return scope.ServiceProvider.GetRequiredService<I18n>().T(key, args);
    }
}
