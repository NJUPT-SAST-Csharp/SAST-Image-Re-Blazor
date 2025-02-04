using System;

namespace Model.Account;

internal readonly record struct RefreshTokenRequest(string RefreshToken) { }
