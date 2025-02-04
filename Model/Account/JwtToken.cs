using System;

namespace Model.Account;

public readonly record struct JwtToken(string AccessToken, string RefreshToken, long ExpireIn);
