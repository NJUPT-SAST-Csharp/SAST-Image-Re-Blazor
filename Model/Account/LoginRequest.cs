using System;

namespace Model.Account;

internal readonly record struct LoginRequest(string Username, string Password) { }
