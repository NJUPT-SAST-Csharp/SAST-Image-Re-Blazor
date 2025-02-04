using System;
using Controller.Shared;
using Model.Account;

namespace Controller.Account;

public readonly record struct LoginCommandResult(JwtToken Token);

public sealed record LoginCommand(string Username, string Password)
    : ICommandRequest<LoginCommandResult>
{
    internal LoginRequest ToRequest() => new(Username, Password);
}

internal sealed class LoginCommandHandler(IAccountAPI account)
    : ICommandRequestHandler<LoginCommand, LoginCommandResult>
{
    public async Task<LoginCommandResult> Handle(
        LoginCommand command,
        CancellationToken cancellationToken
    )
    {
        var response = await account.LoginAsync(command.ToRequest(), cancellationToken);
        if (response is null || response.IsSuccessful == false)
        {
            throw new InvalidOperationException("Login failed");
        }
        return new LoginCommandResult(response.Content);
    }
}
