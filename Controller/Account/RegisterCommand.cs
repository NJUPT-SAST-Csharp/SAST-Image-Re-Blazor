using System.Net;
using System.Security.Claims;
using Controller.Exceptions;
using Controller.Shared;
using FluentValidation;
using Model.Account;

namespace Controller.Account;

public readonly record struct RegisterCommandResult(ClaimsPrincipal User, bool IsSuccessful = true)
    : IResult;

public sealed class RegisterCommand : ICommandRequest<RegisterCommandResult>
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;

    internal RegisterRequest ToRequest() => new(Username, Password, int.Parse(Code));
}

internal sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator(II18nText i18n, IQuerySender sender)
    {
        RuleFor(x => x.Password).NotEmpty().Length(6, 20);
        RuleFor(x => x.Username).NotEmpty().Length(2, 16);
        RuleFor(x => x.Code)
            .NotEmpty()
            .Length(6)
            .Must(x => int.TryParse(x, out _))
            .WithMessage(i18n.T("Registration Code invalid"));
    }
}

internal sealed class RegisterCommandHandler(IAccountAPI api, ICommandSender sender)
    : ICommandRequestHandler<RegisterCommand, RegisterCommandResult>
{
    public async Task<RegisterCommandResult> Handle(
        RegisterCommand command,
        CancellationToken cancellationToken
    )
    {
        var registerResult =
            await api.RegisterAsync(command.ToRequest(), cancellationToken)
            ?? throw new ResponseNullOrEmptyException();

        if (registerResult.StatusCode == HttpStatusCode.Conflict)
            throw new RequestErrorCodeException("That username has been occupied");

        if (registerResult.StatusCode == HttpStatusCode.BadRequest)
            throw new RequestErrorCodeException("Register failed");

        var authResult = await sender.CommandAsync(new AuthCommand(registerResult.Content));
        return new RegisterCommandResult(authResult.User);
    }
}
