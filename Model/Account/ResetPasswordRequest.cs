using System;

namespace Model.Account;

public readonly record struct ResetPasswordRequest(string OldPassword, string NewPassword) { }
