using Controller.Exceptions;
using Controller.Shared;
using Model.User;
using Refit;

namespace Controller.User;

public readonly record struct UpdateAvatarCommand(Stream File) : ICommandRequest<Result> { }

internal sealed class UpdateAvatarCommandHandler(IUserApi api, II18nText i18n)
    : ICommandRequestHandler<UpdateAvatarCommand, Result>
{
    public async Task<Result> Handle(UpdateAvatarCommand command, CancellationToken ct)
    {
        const int MaxSize = 1024 * 1024 * 3;
        if (command.File.Length > MaxSize)
        {
            throw new InvalidRequestException(i18n.T("Avatar size should be less than 3MB"));
        }

        StreamPart file = new(command.File, "avatar");

        var result = await api.UpdateAvatarAsync(file);

        return new(result.IsSuccessful);
    }
}
