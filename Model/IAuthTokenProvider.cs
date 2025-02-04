using System;

namespace Model;

public interface IAuthTokenProvider
{
    public Task<string?> GetAsync();
}
