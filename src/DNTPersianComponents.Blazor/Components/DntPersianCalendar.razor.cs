using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using DNTPersianUtils.Core;
using Microsoft.AspNetCore.Components;

namespace DNTPersianComponents.Blazor;

/// <summary>
///     A custom Persian calendar component
/// </summary>
public partial class DntPersianCalendar
{
    private readonly PersianDay _today = DateTime.Now.ToPersianDay();
    private IReadOnlyList<PersianDayEvents>? _currentEvents;
    private int _selectedMonth;
    private int _selectedYear;

    private IEnumerable<PersianDay?> CalendarCells { set; get; } = new List<PersianDay?>();

    private bool IsLoading { set; get; }

    private IEnumerable<string> PersianDayNamesOfWeek =>
        UseShortPersianDayNamesOfWeek ? PersianCulture.ShortPersianDayWeekNames : PersianCulture.PersianDayNamesOfWeek;

    /// <summary>
    ///     The margin bottom. Its default value is `2`.
    /// </summary>
    [Parameter]
    public int MarginBottom { get; set; } = 2;

    /// <summary>
    ///     The selected month.
    /// </summary>
    [Parameter]
    public int SelectedMonth
    {
        get => _selectedMonth;
        set
        {
            var hasChanged = !EqualityComparer<int>.Default.Equals(value, _selectedMonth);
            if (hasChanged)
            {
                _selectedMonth = value;
                SelectedMonthChanged?.Invoke(value);
            }
        }
    }

    /// <summary>
    ///     Fires when a month is selected.
    /// </summary>
    [Parameter]
    public Action<int>? SelectedMonthChanged { set; get; }

    /// <summary>
    ///     The selected Year.
    /// </summary>
    [Parameter]
    public int SelectedYear
    {
        get => _selectedYear;
        set
        {
            var hasChanged = !EqualityComparer<int>.Default.Equals(value, _selectedYear);
            if (hasChanged)
            {
                _selectedYear = value;
                SelectedYearChanged?.Invoke(value);
            }
        }
    }

    /// <summary>
    ///     Fires when a year is selected.
    /// </summary>
    [Parameter]
    public Action<int>? SelectedYearChanged { set; get; }

    /// <summary>
    ///     Fires when a day is selected.
    /// </summary>
    [Parameter]
    public Action<PersianDay>? DayClicked { set; get; }

    /// <summary>
    ///     The minimum year of the year's dropdown.
    ///     Its default value is `1350`.
    /// </summary>
    [Parameter]
    public int FromYear { get; set; } = 1350;

    /// <summary>
    ///     The maximum year of the year's dropdown.
    ///     Its default value is `1450`.
    /// </summary>
    [Parameter]
    public int ToYear { get; set; } = 1450;

    /// <summary>
    ///     Should we use "ش" instead of "شنبه".
    /// </summary>
    [Parameter]
    public bool UseShortPersianDayNamesOfWeek { set; get; }

    /// <summary>
    ///     PreviousMonth button's icon. Its default value is `bi-arrow-right` from https://icons.getbootstrap.com/.
    /// </summary>
    [Parameter]
    public string ShowPreviousMonthIcon { get; set; } = "bi-arrow-right";

    /// <summary>
    ///     NextMonth button's icon. Its default value is `bi-arrow-left` from https://icons.getbootstrap.com/.
    /// </summary>
    [Parameter]
    public string ShowNextMonthIcon { get; set; } = "bi-arrow-left";

    /// <summary>
    ///     PreviousYear button's icon. Its default value is `bi-arrow-right` from https://icons.getbootstrap.com/.
    /// </summary>
    [Parameter]
    public string ShowPreviousYearIcon { get; set; } = "bi-arrow-right";

    /// <summary>
    ///     NextYear button's icon. Its default value is `bi-arrow-left` from https://icons.getbootstrap.com/.
    /// </summary>
    [Parameter]
    public string ShowNextYearIcon { get; set; } = "bi-arrow-left";

    /// <summary>
    ///     Its default value is `text-secondary`.
    /// </summary>
    [Parameter]
    public string SpinnerColor { set; get; } = "text-secondary";

    /// <summary>
    ///     Day's buttons custom css class. Its default value is `btn`.
    /// </summary>
    [Parameter]
    public string DayButtonsCss { set; get; } = "btn";

    /// <summary>
    ///     Today's button custom css class. Its default value is `btn btn-secondary`.
    /// </summary>
    [Parameter]
    public string TodayButtonCss { set; get; } = "btn btn-secondary";

    /// <summary>
    ///     Holiday's custom css class. Its default value is `badge bg-danger`.
    /// </summary>
    [Parameter]
    public string HolidayCss { set; get; } = "badge bg-danger";

    /// <summary>
    ///     Table's custom css class. Its default value is `table table-striped table-hover`.
    /// </summary>
    [Parameter]
    public string TableCss { set; get; } = "table table-striped table-hover";

    /// <summary>
    ///     thead's custom css class. Its default value is `table-secondary`.
    /// </summary>
    [Parameter]
    public string TableHeadCss { set; get; } = "table-secondary";

    /// <summary>
    ///     Next and previous buttons custom css class. Its default value is `btn btn-secondary`.
    /// </summary>
    [Parameter]
    public string NextPreviousButtonsCss { set; get; } = "btn btn-secondary";

    /// <summary>
    ///     Today's highlighter custom css class. Its default value is `table-primary`.
    /// </summary>
    [Parameter]
    public string TodayHighlightCss { set; get; } = "table-primary";

    /// <summary>
    ///     Should we display a footer with a button point to today?
    /// </summary>
    [Parameter]
    public bool ShowTodayButton { get; set; }

    /// <summary>
    ///     Should we display the year and month selection dropdowns?
    ///     Its default value is `true`.
    /// </summary>
    [Parameter]
    public bool ShowYearMonthDropdowns { get; set; } = true;

    /// <summary>
    ///     Should we display the related holidays of the current month?
    /// </summary>
    [Parameter]
    public bool ShowHolidays { get; set; }

    /// <summary>
    ///     Holiday's text custom css class. Its default value is `text-danger`.
    /// </summary>
    [Parameter]
    public string HolidayTextCss { set; get; } = "text-danger";

    /// <summary>
    ///     Returns the list of related events of the given Persian month.
    /// </summary>
    [Parameter]
    public Func<EventDate, Task<IReadOnlyList<PersianDayEvents>>>? PrepareEventsList { set; get; }

    /// <summary>
    ///     Method invoked when the component has received parameters from its parent.
    /// </summary>
    protected override Task OnParametersSetAsync() => CreateCalendarAsync();

    private Task OnSelectedMonthChangedAsync(ChangeEventArgs args)
    {
        var value = args.Value?.ToString();
        if (int.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out var month))
        {
            SelectedMonth = month;
            return CreateCalendarAsync();
        }

        return Task.CompletedTask;
    }

    private Task OnSelectedYearChangedAsync(ChangeEventArgs args)
    {
        var value = args.Value?.ToString();
        if (int.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out var year))
        {
            SelectedYear = year;
            return CreateCalendarAsync();
        }

        return Task.CompletedTask;
    }

    private void OnDayClicked(PersianDay day)
    {
        DayClicked?.Invoke(day);
    }

    private async Task CreateCalendarAsync()
    {
        try
        {
            IsLoading = true;

            if (SelectedMonth <= 0)
            {
                SelectedMonth = _today.Month;
            }

            if (SelectedYear <= 0)
            {
                SelectedYear = _today.Year;
            }

            CalendarCells = SelectedYear.CreatePersianMonthCalendar(SelectedMonth);

            if (PrepareEventsList is not null)
            {
                _currentEvents = await PrepareEventsList.Invoke(new EventDate(SelectedYear, SelectedMonth));
            }
        }
        finally
        {
            IsLoading = false;
        }
    }

    private Task ShowPreviousMonthAsync()
    {
        if (SelectedMonth == 1)
        {
            SelectedMonth = 12;
            return ShowPreviousYearAsync();
        }

        SelectedMonth--;
        return CreateCalendarAsync();
    }

    private Task ShowNextMonthAsync()
    {
        if (SelectedMonth == 12)
        {
            SelectedMonth = 1;
            return ShowNextYearAsync();
        }

        SelectedMonth++;
        return CreateCalendarAsync();
    }

    private Task ShowPreviousYearAsync()
    {
        if (SelectedYear <= FromYear)
        {
            return Task.CompletedTask;
        }

        SelectedYear--;
        return CreateCalendarAsync();
    }

    private Task ShowNextYearAsync()
    {
        if (SelectedYear >= ToYear)
        {
            return Task.CompletedTask;
        }

        SelectedYear++;
        return CreateCalendarAsync();
    }

    private Task ShowTodayAsync()
    {
        SelectedYear = _today.Year;
        SelectedMonth = _today.Month;
        return CreateCalendarAsync();
    }

    private string GetTodayHighlighterCss(PersianDay? day) =>
        day != null && _today.Equals(day) ? TodayHighlightCss : string.Empty;

    private string GetHolidayTextCss(PersianDay? day, int cellNumber)
    {
        if (day is null)
        {
            return string.Empty;
        }

        return day.Holidays is not null || cellNumber == 6 ? HolidayTextCss : string.Empty;
    }

    private IReadOnlyList<DayEvent> GetDayEvents(PersianDay? day)
    {
        if (day is null || _currentEvents is null)
        {
            return new List<DayEvent>().AsReadOnly();
        }

        return _currentEvents.Where(events => events.PersianDay.Equals(day) && events.DayEvents is not null)
                             .SelectMany(events => events.DayEvents!)
                             .OrderBy(de => de.StartTime).ThenBy(de => de.Description)
                             .ToList()
                             .AsReadOnly();
    }

    private static string GetTimeRangeText(DayEvent? dayEvent)
    {
        var result = string.Empty;

        if (dayEvent is null)
        {
            return result;
        }

        if (dayEvent.StartTime.HasValue)
        {
            result += dayEvent.StartTime.Value.ToString("hh\\:mm", CultureInfo.InvariantCulture);
        }

        if (dayEvent.EndTime.HasValue)
        {
            result += $" - {dayEvent.EndTime.Value.ToString("hh\\:mm", CultureInfo.InvariantCulture)}";
        }

        return result.ToPersianNumbers();
    }
}