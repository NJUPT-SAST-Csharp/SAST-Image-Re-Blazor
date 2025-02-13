using Controller.Shared;
using FluentValidation;
using Model.Account;

namespace Controller.Account;

public sealed class ResetUsernameCommand : ICommandRequest<Result>
{
    public string Username { get; set; } = string.Empty;
}

internal sealed class ResetUsernameCommandValidator : AbstractValidator<ResetUsernameCommand>
{
    public ResetUsernameCommandValidator(II18nText i18n)
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .Length(2, 16)
            .Must(x => x.IsValid())
            .WithMessage(i18n.T("Username can contain only digits, letters, and underscores"));
    }
}

internal sealed class ResetUsernameCommandHandler(IAccountApi api)
    : ICommandRequestHandler<ResetUsernameCommand, Result>
{
    public async Task<Result> Handle(
        ResetUsernameCommand request,
        CancellationToken cancellationToken
    )
    {
        var result = await api.ResetUsernameAsync(
            new ResetUsernameRequest(request.Username),
            cancellationToken
        );

        return new(result.IsSuccessful);
    }
}
