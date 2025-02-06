using Microsoft.AspNetCore.Components;

namespace View.Pages.User.Main;

public sealed partial class MainProfileInfo
{
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object> InputAttributes { get; set; } = [];

    [Parameter, EditorRequired]
    public long Id { get; set; }
}
