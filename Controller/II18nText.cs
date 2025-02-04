using System;

namespace Controller;

public interface II18nText
{
    public string T(string? key, params object[] args);
}
