using Controller.Shared;
using FluentValidation;
using Model.Account;

namespace Controller.User;

public sealed class UpdateUsernameCommand : ICommandRequest<Result>
{
    public string Username { get; set; } = string.Empty;
}

internal sealed class UpdateUsernameCommandValidator : AbstractValidator<UpdateUsernameCommand>
{
    public UpdateUsernameCommandValidator(II18nText i18n)
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .Length(2, 16)
            .Must(x => x.IsValid())
            .WithMessage(i18n.T("Username can contain only digits, letters, and underscores"));
    }
}

internal sealed class UpdateUsernameCommandHandler(IAccountApi api)
    : ICommandRequestHandler<UpdateUsernameCommand, Result>
{
    public async Task<Result> Handle(
        UpdateUsernameCommand request,
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
