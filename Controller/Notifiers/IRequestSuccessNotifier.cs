namespace Controller.Notifiers;

public interface IRequestSuccessNotifier
{
    public void Notify(string message);

    public void Notify();
}
