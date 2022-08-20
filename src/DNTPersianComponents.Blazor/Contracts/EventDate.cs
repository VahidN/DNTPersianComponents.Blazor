namespace DNTPersianComponents.Blazor;

/// <summary>
///     The Event's Date
/// </summary>
public class EventDate
{
    /// <summary>
    ///     The Event's Date
    /// </summary>
    public EventDate()
    {
    }

    /// <summary>
    ///     The Event's Date
    /// </summary>
    public EventDate(int persianYear, int persianMonth)
    {
        PersianYear = persianYear;
        PersianMonth = persianMonth;
    }

    /// <summary>
    ///     The Event's Year
    /// </summary>
    public int PersianYear { set; get; }

    /// <summary>
    ///     The Event's Month
    /// </summary>
    public int PersianMonth { set; get; }
}