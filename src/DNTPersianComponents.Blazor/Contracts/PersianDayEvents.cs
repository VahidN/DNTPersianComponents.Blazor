using System.Collections.Generic;
using DNTPersianUtils.Core;

namespace DNTPersianComponents.Blazor
{
    /// <summary>
    /// Defines related events of a given Persian day
    /// </summary>
    public class PersianDayEvents
    {
        /// <summary>
        /// The current Persian day
        /// </summary>
        public PersianDay PersianDay { set; get; } = default!;

        /// <summary>
        /// Defines related events of a given Persian day
        /// </summary>
        public IReadOnlyList<DayEvent>? DayEvents { set; get; }
    }
}