using Controller.Exceptions;
using Controller.Shared;
using Model.User;
using Refit;

namespace Controller.User;

public readonly record struct UpdateHeaderCommand(Stream File) : ICommandRequest<Result> { }

internal sealed class UpdateHeaderCommandHandler(IUserApi api, II18nText i18n)
    : ICommandRequestHandler<UpdateHeaderCommand, Result>
{
    public async Task<Result> Handle(UpdateHeaderCommand command, CancellationToken ct)
    {
        const int MaxSize = 1024 * 1024 * 10;
        if (command.File.Length > MaxSize)
        {
            throw new InvalidRequestException(i18n.T("Header size should be less than 10MB"));
        }

        StreamPart file = new(command.File, "header", "image/*", "header");

        var result = await api.UpdateHeaderAsync(file);

        return new(result.IsSuccessful);
    }
}
