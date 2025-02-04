using System;
using System.Collections.Concurrent;

namespace Controller;

public interface IRequestExceptionNotify
{
    public void Notify(string message);
}
