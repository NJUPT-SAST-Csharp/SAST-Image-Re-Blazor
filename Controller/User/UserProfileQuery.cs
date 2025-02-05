using Controller.Exceptions;
using Controller.Shared;
using Model.User;

namespace Controller.User;

public readonly record struct UserProfileQueryResult(
    long Id,
    string Username,
    string Biography,
    bool IsSuccessful = true
) : IResult
{
    internal UserProfileQueryResult(UserProfileDto dto)
        : this(dto.Id, dto.Username, dto.Biography) { }

    public static UserProfileQueryResult Fail => new(0, string.Empty, string.Empty, false);
}

public readonly record struct UserProfileQuery(long Id) : IQueryRequest<UserProfileQueryResult> { }

internal sealed class UserProfileQueryHandler(IUserApi api)
    : IQueryRequestHandler<UserProfileQuery, UserProfileQueryResult>
{
    public async Task<UserProfileQueryResult> Handle(
        UserProfileQuery query,
        CancellationToken cancellationToken
    )
    {
        var response = await api.GetProfileAsync(query.Id);

        if (response.IsSuccessful == false)
        {
            throw new RequestErrorCodeException(response.Error.Message);
        }

        return new(response.Content);
    }
}
