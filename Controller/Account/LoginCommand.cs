using System.Security.Claims;
using Controller.Exceptions;
using Controller.Shared;
using FluentValidation;
using Model.Account;

namespace Controller.Account;

public readonly record struct LoginCommandResult(ClaimsPrincipal User, bool IsSuccessful = true)
    : IResult { }

public sealed class LoginCommand : ICommandRequest<LoginCommandResult>
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    internal LoginRequest ToRequest() => new(Username, Password);
}

internal sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator(II18nText i18n)
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .Length(2, 16)
            .Must(x => x.IsValid())
            .WithMessage(i18n.T("Username can contain only digits, letters, and underscores"));

        RuleFor(x => x.Password)
            .NotEmpty()
            .Length(6, 20)
            .Must(x => x.IsValid())
            .WithMessage(i18n.T("Password can contain only digits, letters, and underscores"));
    }
}

internal sealed class LoginCommandHandler(
    IAccountApi account,
    ICommandSender sender,
    II18nText i18n
) : ICommandRequestHandler<LoginCommand, LoginCommandResult>
{
    public async Task<LoginCommandResult> Handle(
        LoginCommand command,
        CancellationToken cancellationToken
    )
    {
        var loginResult = await account.LoginAsync(command.ToRequest(), cancellationToken);

        if (loginResult.IsSuccessful == false)
            throw new RequestErrorCodeException(i18n.T("Username or Password is incorrect"));

        var authResult = await sender.CommandAsync(new AuthCommand(loginResult.Content));

        return new LoginCommandResult(authResult.User);
    }
}
