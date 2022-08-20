using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DNTPersianUtils.Core;
using Microsoft.AspNetCore.Components;

namespace DNTPersianComponents.Blazor.WasmSample.Client.Pages;

public partial class CustomPersianCalendar : ComponentBase
{
    private void OnDayClicked(PersianDay day)
    {
        Console.WriteLine($"Selected day: {day.Year}/{day.Month}/{day.Day}");
    }

    private void OnSelectedMonthChanged(int month)
    {
        Console.WriteLine($"Selected month: {month}");
    }

    private void OnSelectedYearChanged(int year)
    {
        Console.WriteLine($"Selected year: {year}");
    }

    private async Task<IReadOnlyList<PersianDayEvents>> OnPrepareEventsAsync(EventDate eventDate)
    {
        Console.WriteLine($"Events of: {eventDate.PersianYear}/{eventDate.PersianMonth}");

        //TODO: Query events of the selected date from the DB
        await Task.Delay(500);

        if (eventDate.PersianYear == 1400 && eventDate.PersianMonth == 4)
        {
            var events = new List<PersianDayEvents>
                         {
                             new()
                             {
                                 PersianDay = new PersianDay
                                              {
                                                  Day = 1,
                                                  Month = 4,
                                                  Year = 1400,
                                              },
                                 DayEvents = new List<DayEvent>
                                             {
                                                 new()
                                                 {
                                                     Description = "شروع روز",
                                                     CssClass = "badge bg-success",
                                                 },
                                                 new()
                                                 {
                                                     Description = "پیاده روی پیاده روی پیاده روی پیاده روی پیاده روی",
                                                     StartTime = new TimeSpan(17, 1, 0),
                                                     EndTime = new TimeSpan(18, 1, 0),
                                                 },
                                                 new()
                                                 {
                                                     Description = "نوشیدن قهوه",
                                                     StartTime = new TimeSpan(9, 15, 0),
                                                     CssClass = "badge bg-warning",
                                                     Clicked = (day, @event) =>
                                                                   Console
                                                                       .WriteLine($"{day.Year}/{day.Month}/{day.Day} @ {@event.StartTime} was clicked!"),
                                                 },
                                             }.AsReadOnly(),
                             },
                             new()
                             {
                                 PersianDay = new PersianDay
                                              {
                                                  Day = 15,
                                                  Month = 4,
                                                  Year = 1400,
                                              },
                                 DayEvents = new List<DayEvent>
                                             {
                                                 new()
                                                 {
                                                     Description = "خرید هفتگی",
                                                     StartTime = new TimeSpan(19, 1, 0),
                                                     CssClass = "badge bg-primary",
                                                     StartTimeCssClass = "badge bg-warning",
                                                 },
                                             }.AsReadOnly(),
                             },
                         };
            return events.AsReadOnly();
        }

        return new List<PersianDayEvents>().AsReadOnly();
    }
}