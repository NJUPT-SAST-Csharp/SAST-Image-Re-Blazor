namespace Model.User;

internal readonly record struct UserProfileDto(
    long Id,
    string Username,
    string Nickname,
    string Biography
) { }
