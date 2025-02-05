using Controller.Notifiers;
using Masa.Blazor;
using Masa.Blazor.Presets;

namespace View.Misc;

internal sealed class RequestSuccessNotifier(GlobalSnackbarsRef snackbars) : IRequestSuccessNotifier
{
    public void Notify(string message)
    {
        snackbars.Ref.EnqueueSnackbar(
            new SnackbarOptions()
            {
                Type = AlertTypes.Success,
                Content = message,
                Closeable = true,
                Timeout = 5000,
            }
        );
    }

    public void Notify()
    {
        snackbars.Ref.EnqueueSnackbar(
            new SnackbarOptions()
            {
                Type = AlertTypes.Success,
                Closeable = true,
                Timeout = 5000,
            }
        );
    }
}
