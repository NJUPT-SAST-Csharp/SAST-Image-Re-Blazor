using Blazored.LocalStorage;
using Controller.Shared;

namespace Controller.Account;

public readonly record struct LogoutCommand : ICommandRequest<Result> { }

internal sealed class LogoutCommandHandler(ILocalStorageService localStorage)
    : ICommandRequestHandler<LogoutCommand, Result>
{
    public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        await localStorage.RemoveItemAsync(AuthTokenProvider.LocalStorageKey, cancellationToken);

        return Result.Success;
    }
}
