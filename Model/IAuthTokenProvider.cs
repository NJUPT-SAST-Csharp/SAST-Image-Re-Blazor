using System;

namespace Model;

internal interface IAuthTokenProvider
{
    public Task<string?> GetAsync();
}
