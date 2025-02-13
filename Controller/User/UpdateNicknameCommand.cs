using Controller.Shared;
using FluentValidation;
using Model.User;

namespace Controller.User;

public sealed class UpdateNicknameCommand : ICommandRequest<Result>
{
    public string Nickname { get; set; } = string.Empty;

    internal UpdateNicknameRequest ToRequest() => new(Nickname);
}

internal sealed class UpdateNicknameCommandValidator : AbstractValidator<UpdateNicknameCommand>
{
    public UpdateNicknameCommandValidator()
    {
        RuleFor(x => x.Nickname).NotEmpty().Length(1, 16);
    }
}

internal sealed class UpdateNicknameCommandHandler(IUserApi api)
    : ICommandRequestHandler<UpdateNicknameCommand, Result>
{
    public async Task<Result> Handle(
        UpdateNicknameCommand command,
        CancellationToken cancellationToken
    )
    {
        var result = await api.UpdateNicknameAsync(command.ToRequest(), cancellationToken);

        return new(result.IsSuccessful);
    }
}
