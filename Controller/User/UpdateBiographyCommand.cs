using Controller.Shared;
using FluentValidation;
using Model.User;

namespace Controller.User;

public sealed class UpdateBiographyCommand : ICommandRequest<Result>
{
    public string Biography { get; set; } = string.Empty;

    internal UpdateBiographyRequest ToRequest() => new(Biography);
}

internal sealed class UpdateBiographyCommandValidator : AbstractValidator<UpdateBiographyCommand>
{
    public UpdateBiographyCommandValidator()
    {
        RuleFor(x => x.Biography).MaximumLength(50);
    }
}

internal sealed class UpdateBiographyCommandHandler(IUserApi api)
    : ICommandRequestHandler<UpdateBiographyCommand, Result>
{
    public async Task<Result> Handle(
        UpdateBiographyCommand request,
        CancellationToken cancellationToken
    )
    {
        var result = await api.UpdateBiographyAsync(request.ToRequest(), cancellationToken);

        return new(result.IsSuccessful);
    }
}
