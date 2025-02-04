using System;
using Controller;
using Masa.Blazor;

namespace View.Misc;

public sealed class I18nTextAdapter(I18n i18N) : II18nText
{
    public string T(string? key, params object[] args)
    {
        return i18N.T(key, args);
    }
}
