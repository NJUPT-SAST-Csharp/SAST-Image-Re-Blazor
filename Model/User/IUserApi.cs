using Refit;

namespace Model.User;

internal interface IUserApi
{
    [Get("/users/{id}/profile")]
    public Task<IApiResponse<UserProfileDto>> GetProfileAsync(long id);

    [Multipart]
    [Post("/users/avatar")]
    public Task<IApiResponse> UpdateAvatarAsync(StreamPart avatar);

    [Multipart]
    [Post("/users/header")]
    public Task<IApiResponse> UpdateHeaderAsync(StreamPart header);

    [Post("/users/biography")]
    public Task<IApiResponse> UpdateBiographyAsync([Body] UpdateBiographyRequest request);
}
