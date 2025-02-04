using System;

namespace Model.Account;

public readonly record struct LoginRequest(string Username, string Password) { }
