using System;
using Masa.Blazor.Presets;

namespace View.Misc;

public sealed class GlobalSnackbarsRef
{
    internal PEnqueuedSnackbars Ref { get; set; } = null!;
}

internal static class GlobalSnackbarsRefExtensions
{
    public static IServiceCollection AddSnackbarsService(this IServiceCollection services)
    {
        services.AddSingleton<GlobalSnackbarsRef>();
        return services;
    }
}
