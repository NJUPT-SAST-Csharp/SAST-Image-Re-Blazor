using System;

namespace Model.Account;

public readonly record struct RegisterRequest(string Username, string Password, int Code) { }
