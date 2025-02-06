using Controller;
using Controller.Notifiers;
using Masa.Blazor;
using Masa.Blazor.Presets;

namespace View.Misc;

internal sealed class RequestSuccessNotifier(GlobalSnackbarsRef snackbars, II18nText i18n)
    : IRequestSuccessNotifier
{
    public void Notify(string message)
    {
        snackbars.Ref.EnqueueSnackbar(
            new SnackbarOptions()
            {
                Type = AlertTypes.Success,
                Content = message,
                Closeable = true,
                Timeout = 1000,
            }
        );
    }

    public void Notify()
    {
        snackbars.Ref.EnqueueSnackbar(
            new SnackbarOptions()
            {
                Type = AlertTypes.Success,
                Content = i18n.T("Success"),
                Closeable = true,
                Timeout = 1000,
            }
        );
    }
}
