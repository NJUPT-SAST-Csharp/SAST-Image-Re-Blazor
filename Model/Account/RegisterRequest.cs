using System;

namespace Model.Account;

internal readonly record struct RegisterRequest(string Username, string Password, int Code) { }
