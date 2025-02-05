using Controller.Notifiers;

namespace View.Misc;

public sealed class AuthStateChangedNotifier(Authenticator provider) : IAuthStateChangedNotifier
{
    public void Notify()
    {
        provider.Refresh();
    }
}
