namespace Controller.Notifiers;

public interface IRequestExceptionNotifier
{
    public void Notify(string message);
}
