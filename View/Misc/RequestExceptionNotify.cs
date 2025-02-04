using Controller;
using Masa.Blazor;
using Masa.Blazor.Presets;

namespace View.Misc;

internal sealed class RequestExceptionNotify(GlobalSnackbarsRef snackbars) : IRequestExceptionNotify
{
    public void Notify(string message)
    {
        snackbars.Ref.EnqueueSnackbar(
            new SnackbarOptions()
            {
                Type = AlertTypes.Error,
                Content = message,
                Closeable = true,
                Timeout = 5000,
            }
        );
    }
}

internal static class RequestExceptionNotifyExtensions
{
    public static IServiceCollection AddRequestExceptionNotify(this IServiceCollection services)
    {
        services.AddSingleton<IRequestExceptionNotify, RequestExceptionNotify>();
        return services;
    }
}
