using Refit;

namespace Model.User;

internal interface IUserApi
{
    [Get("/users/{id}/profile")]
    public Task<IApiResponse<UserProfileDto>> GetProfileAsync(
        long id,
        CancellationToken cancellationToken = default
    );

    [Multipart]
    [Post("/users/avatar")]
    public Task<IApiResponse> UpdateAvatarAsync(
        StreamPart avatar,
        CancellationToken cancellationToken = default
    );

    [Multipart]
    [Post("/users/header")]
    public Task<IApiResponse> UpdateHeaderAsync(
        StreamPart header,
        CancellationToken cancellationToken = default
    );

    [Post("/users/biography")]
    public Task<IApiResponse> UpdateBiographyAsync(
        [Body] UpdateBiographyRequest request,
        CancellationToken cancellationToken = default
    );

    [Post("/users/nickname")]
    public Task<IApiResponse> UpdateNicknameAsync(
        [Body] UpdateNicknameRequest request,
        CancellationToken cancellationToken = default
    );
}
