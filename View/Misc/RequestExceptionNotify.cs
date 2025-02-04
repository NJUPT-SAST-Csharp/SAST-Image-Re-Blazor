using Controller;
using Masa.Blazor;
using Masa.Blazor.Presets;

namespace View.Misc;

internal sealed class RequestExceptionNotify(
    GlobalSnackbarsRef snackbars,
    IServiceProvider services
) : IRequestExceptionNotify
{
    public void Notify(string message)
    {
        using var scope = services.CreateScope();

        snackbars.Ref.EnqueueSnackbar(
            new SnackbarOptions()
            {
                Type = AlertTypes.Error,
                Content = scope.ServiceProvider.GetRequiredService<I18n>().T(message),
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
