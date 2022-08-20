using System;
using DNTPersianUtils.Core;

namespace DNTPersianComponents.Blazor;

/// <summary>
///     Defines an events of a given day
/// </summary>
public class DayEvent
{
    /// <summary>
    ///     The description of the event
    /// </summary>
    public string Description { set; get; } = default!;

    /// <summary>
    ///     The description's CSS class. Its default value is `badge bg-info`.
    /// </summary>
    public string CssClass { set; get; } = "badge bg-info";

    /// <summary>
    ///     The start time of the event
    /// </summary>
    public TimeSpan? StartTime { set; get; }

    /// <summary>
    ///     The StartTime's CSS class. Its default value is `badge bg-info`.
    /// </summary>
    public string StartTimeCssClass { set; get; } = "badge bg-info";

    /// <summary>
    ///     The end time of the event
    /// </summary>
    public TimeSpan? EndTime { set; get; }

    /// <summary>
    ///     What should happen if someone clicked at this event?
    /// </summary>
    public Action<PersianDay, DayEvent>? Clicked { set; get; }
}