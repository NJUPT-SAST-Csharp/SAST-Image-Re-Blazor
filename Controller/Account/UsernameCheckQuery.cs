using Controller.Shared;
using Model.Account;

namespace Controller.Account;

public readonly record struct UsernameCheckQueryResult(bool IsSuccessful) : IResult;

public readonly record struct UsernameCheckQuery(string Username)
    : IQueryRequest<UsernameCheckQueryResult>;

internal sealed class UsernameCheckQueryHandler(IAccountApi api)
    : IQueryRequestHandler<UsernameCheckQuery, UsernameCheckQueryResult>
{
    public async Task<UsernameCheckQueryResult> Handle(
        UsernameCheckQuery query,
        CancellationToken cancellationToken
    )
    {
        var response = await api.CheckUsernameAsync(query.Username, cancellationToken);

        return new UsernameCheckQueryResult(response.Content);
    }
}
