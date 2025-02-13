namespace Model.Account;

internal readonly record struct RegisterRequest(
    string Username,
    string Nickname,
    string Password,
    int Code
) { }
