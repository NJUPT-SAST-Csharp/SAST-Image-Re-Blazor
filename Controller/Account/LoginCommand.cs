using System;
using System.Security.Claims;
using Controller.Exceptions;
using Controller.Shared;
using Model.Account;
using Refit;

namespace Controller.Account;

public readonly record struct LoginCommandResult(ClaimsPrincipal User, bool IsSuccessful = true)
    : IResult { }

public sealed class LoginCommand : ICommandRequest<LoginCommandResult>
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    internal LoginRequest ToRequest() => new(Username, Password);
}

internal sealed class LoginCommandHandler(IAccountAPI account, ICommandSender sender)
    : ICommandRequestHandler<LoginCommand, LoginCommandResult>
{
    public async Task<LoginCommandResult> Handle(
        LoginCommand command,
        CancellationToken cancellationToken
    )
    {
        var loginResult =
            await account.LoginAsync(command.ToRequest(), cancellationToken)
            ?? throw new ResponseNullOrEmptyException();

        if (loginResult.IsSuccessful == false)
            throw new RequestErrorCodeException("Username or Password is incorrect");

        var authResult = await sender.CommandAsync(new AuthCommand(loginResult.Content));

        return new LoginCommandResult(authResult.User);
    }
}
