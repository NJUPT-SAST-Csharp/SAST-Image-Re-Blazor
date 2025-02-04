using System;

namespace Model.Account;

internal readonly record struct ResetPasswordRequest(string OldPassword, string NewPassword) { }
