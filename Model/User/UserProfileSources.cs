namespace Model.User;

public static class UserProfileSources
{
    internal static string BaseAddress { get; set; } = string.Empty;

    public static string Header(long id) => $"{BaseAddress}/users/{id}/header";

    public static string Avatar(long id) => $"{BaseAddress}/users/{id}/avatar";
}
