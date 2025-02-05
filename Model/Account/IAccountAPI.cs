using System;
using Refit;

namespace Model.Account;

internal interface IAccountApi
{
    [Post("/account/login")]
    public Task<IApiResponse<JwtToken>> LoginAsync(
        [Body] LoginRequest request,
        CancellationToken cancellationToken = default
    );

    [Post("/account/refresh")]
    public Task<IApiResponse<JwtToken>> RefreshTokenAsync(
        [Body] RefreshTokenRequest request,
        CancellationToken cancellationToken = default
    );

    [Post("/account/register")]
    public Task<IApiResponse<JwtToken>> RegisterAsync(
        [Body] RegisterRequest request,
        CancellationToken cancellationToken = default
    );

    [Post("/account/reset/username")]
    public Task<IApiResponse> ResetUsernameAsync(
        [Body] ResetUsernameRequest request,
        CancellationToken cancellationToken = default
    );

    [Post("/account/reset/password")]
    public Task<IApiResponse> ResetPasswordAsync(
        [Body] ResetPasswordRequest request,
        CancellationToken cancellationToken = default
    );

    [Get("/account/username/check")]
    public Task<IApiResponse<bool>> CheckUsernameAsync(
        [Query] string username,
        CancellationToken cancellationToken = default
    );
}
