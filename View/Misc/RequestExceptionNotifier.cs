using Controller.Notifiers;
using Masa.Blazor;
using Masa.Blazor.Presets;

namespace View.Misc;

internal sealed class RequestExceptionNotifier(GlobalSnackbarsRef snackbars)
    : IRequestExceptionNotifier
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

internal static class RequestNotifiersExtensions
{
    public static IServiceCollection AddRequestNotifiers(this IServiceCollection services)
    {
        services.AddSingleton<IRequestExceptionNotifier, RequestExceptionNotifier>();
        services.AddSingleton<IRequestSuccessNotifier, RequestSuccessNotifier>();
        return services;
    }
}
