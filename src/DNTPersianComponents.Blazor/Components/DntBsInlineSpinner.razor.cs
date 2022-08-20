using Microsoft.AspNetCore.Components;

namespace DNTPersianComponents.Blazor;

/// <summary>
///     An inline spinner
/// </summary>
public partial class DntBsInlineSpinner
{
    /// <summary>
    ///     Its default value is `text-primary`.
    /// </summary>
    [Parameter]
    public string CurrentColor { set; get; } = "text-primary";

    /// <summary>
    ///     Is spinner still visible?
    /// </summary>
    [Parameter]
    public bool IsVisible { get; set; }

    /// <summary>
    ///     Its default value is `1`.
    /// </summary>
    [Parameter]
    public int Margin { get; set; } = 1;

    /// <summary>
    ///     Its default value is `1`.
    /// </summary>
    [Parameter]
    public int Width { get; set; } = 1;

    /// <summary>
    ///     Its default value is `1`.
    /// </summary>
    [Parameter]
    public int Height { get; set; } = 1;

    /// <summary>
    ///     Its default value is `Loading...`.
    /// </summary>
    [Parameter]
    public string LoadingText { get; set; } = "Is loading...";
}