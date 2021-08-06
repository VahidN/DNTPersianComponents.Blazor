using System;
using Microsoft.AspNetCore.Components;
using DNTPersianUtils.Core;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace DNTPersianComponents.Blazor
{
    /// <summary>
    /// A custom input PersianDate component
    /// </summary>
    public partial class DntInputPersianDate<T>
    {
        private string? _enteredValue;

        private BlazorFieldId<T> ValueField => new(ValueExpression);

        private bool ShowCalendarPopOver { set; get; }
        private int CalendarSelectedYear { set; get; }
        private int CalendarSelectedMonth { set; get; }

        private string? EnteredValue
        {
            set => _enteredValue = value;
            get => UsePersianNumbers ? _enteredValue.ToPersianNumbers() : _enteredValue;
        }

        /// <summary>
        /// The InputText's margin bottom. Its default value is `3`.
        /// </summary>
        [Parameter] public int InputRowMarginBottom { get; set; } = 3;

        /// <summary>
        /// The InputText's column width. Its default value is `9`.
        /// </summary>
        [Parameter] public int InputTextColumnWidth { get; set; } = 9;

        /// <summary>
        /// The label's column width of the custom InputText. Its default value is `3`.
        /// </summary>
        [Parameter] public int LabelColumnWidth { get; set; } = 3;

        /// <summary>
        /// The label name of the custom InputText
        /// </summary>
        [Parameter] public string LabelName { get; set; } = default!;

        /// <summary>
        /// Input field's icon from https://icons.getbootstrap.com/.
        /// </summary>
        [Parameter] public string FieldIcon { set; get; } = default!;

        /// <summary>
        /// The label of the show calendar button.
        /// Its default value is `انتخاب تاریخ از تقویم`.
        /// </summary>
        [Parameter] public string ShowCalendarLabel { set; get; } = "انتخاب تاریخ از تقویم";

        /// <summary>
        /// The show calendar button's icon from https://icons.getbootstrap.com/.
        /// Its default value is `bi-calendar3-week`.
        /// </summary>
        [Parameter] public string ShowCalendarIcon { set; get; } = "bi-calendar3-week";

        /// <summary>
        /// سال شروع قرن، اگر سال وارد شده دو رقمی است.
        /// مقدار پیش‌فرض آن 1400 است
        /// </summary>
        [Parameter] public int BeginningOfCentury { set; get; } = 1400;

        /// <summary>
        /// The mininum year of the year's dropdown.
        /// Its default value is `1350`.
        /// </summary>
        [Parameter] public int CalendarFromYear { get; set; } = 1350;

        /// <summary>
        /// The maximun year of the year's dropdown.
        /// Its default value is `1450`.
        /// </summary>
        [Parameter] public int CalendarToYear { get; set; } = 1450;

        /// <summary>
        /// Should we use "ش" instead of "شنبه".
        /// </summary>
        [Parameter] public bool CalendarUseShortPersianDayNamesOfWeek { set; get; }

        /// <summary>
        /// PreviousMonth button's icon. Its default value is `bi-arrow-right` from https://icons.getbootstrap.com/.
        /// </summary>
        [Parameter] public string CalendarShowPreviousMonthIcon { get; set; } = "bi-arrow-right";

        /// <summary>
        /// NextMonth button's icon. Its default value is `bi-arrow-left` from https://icons.getbootstrap.com/.
        /// </summary>
        [Parameter] public string CalendarShowNextMonthIcon { get; set; } = "bi-arrow-left";

        /// <summary>
        /// PreviousYear button's icon. Its default value is `bi-arrow-right` from https://icons.getbootstrap.com/.
        /// </summary>
        [Parameter] public string CalendarShowPreviousYearIcon { get; set; } = "bi-arrow-right";

        /// <summary>
        /// NextYear button's icon. Its default value is `bi-arrow-left` from https://icons.getbootstrap.com/.
        /// </summary>
        [Parameter] public string CalendarShowNextYearIcon { get; set; } = "bi-arrow-left";

        /// <summary>
        /// Its default value is `text-secondary`.
        /// </summary>
        [Parameter] public string CalendarSpinnerColor { set; get; } = "text-secondary";

        /// <summary>
        /// Day's buttons custom css class. Its default value is `btn`.
        /// </summary>
        [Parameter] public string CalendarDayButtonsCss { set; get; } = "btn";

        /// <summary>
        /// Today's button custom css class. Its default value is `btn btn-secondary`.
        /// </summary>
        [Parameter] public string CalendarTodayButtonCss { set; get; } = "btn btn-secondary";

        /// <summary>
        /// Holiday's custom css class. Its default value is `badge bg-danger`.
        /// </summary>
        [Parameter] public string CalendarHolidayCss { set; get; } = "badge bg-danger";

        /// <summary>
        /// Table's custom css class. Its default value is `table table-striped table-hover`.
        /// </summary>
        [Parameter] public string CalendarTableCss { set; get; } = "table table-striped table-hover";

        /// <summary>
        /// thead's custom css class. Its default value is `table-secondary`.
        /// </summary>
        [Parameter] public string CalendarTableHeadCss { set; get; } = "table-secondary";

        /// <summary>
        /// Next and previous buttons custom css class. Its default value is `btn btn-secondary`.
        /// </summary>
        [Parameter] public string CalendarNextPreviousButtonsCss { set; get; } = "btn btn-secondary";

        /// <summary>
        /// Today's highlighter custom css class. Its default value is `table-primary`.
        /// </summary>
        [Parameter] public string CalendarTodayHighlightCss { set; get; } = "table-primary";

        /// <summary>
        /// Should we display a footer with a button point to today?
        /// </summary>
        [Parameter] public bool CalendarShowTodayButton { get; set; }

        /// <summary>
        /// Should we display the year and month selection dropdowns?
        /// Its default value is `true`.
        /// </summary>
        [Parameter] public bool CalendarShowYearMonthDropdowns { get; set; } = true;

        /// <summary>
        /// Should we display the related hoidays of the current month?
        /// </summary>
        [Parameter] public bool CalendarShowHolidays { get; set; }

        /// <summary>
        /// Holiday's text custom css class. Its default value is `text-danger`.
        /// </summary>
        [Parameter] public string CalendarHolidayTextCss { set; get; } = "text-danger";

        /// <summary>
        /// The left position of the popover. Its default value is `20px`.
        /// </summary>
        [Parameter] public string PopOverLeft { set; get; } = "20px";

        /// <summary>
        /// The top position of the popover. Its default value is `20px`.
        /// </summary>
        [Parameter] public string PopOverTop { set; get; } = "20px";

        /// <summary>
        /// Should we show the Calendar `onfocus` event?
        /// </summary>
        [Parameter] public bool ShowCalendarOnFocus { set; get; }

        /// <summary>
        /// Should we show the entered date with Persian numbers?
        /// </summary>
        [Parameter] public bool UsePersianNumbers { set; get; }

        /// <summary>
        /// Gets or sets the error message used when displaying an a parsing error.
        /// Its default value is `لطفا در ورودی {0} تاریخ شمسی معتبری را وارد نمائید.`
        /// </summary>
        [Parameter] public string ParsingErrorMessage { get; set; } = "لطفا در ورودی {0} تاریخ شمسی معتبری را وارد نمائید.";

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            SanityCheck();
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent.
        /// </summary>
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            HideCalendar();
        }

        /// <summary>
        /// Method invoked after each time the component has been rendered.
        /// </summary>
        /// <param name="firstRender">Set to true if this is the first time</param>
        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
            if (firstRender)
            {
                EnteredValue = CurrentValueAsString;
                ValueField.NotifyFieldChanged(EditContext);
            }
        }

        internal void OnInput(ChangeEventArgs e)
        {
            SetCurrentValue(e.Value as string);
        }

        /// <inheritdoc />
        protected override bool TryParseValueFromString(
                string? value,
                [MaybeNullWhen(false)] out T result,
                [NotNullWhen(false)] out string? validationErrorMessage)
        {
            validationErrorMessage = string.Format(CultureInfo.InvariantCulture, ParsingErrorMessage, DisplayName ?? ValueField.FieldName);
            if (!value.TryParsePersianDateToDateTimeOrDateTimeOffset(out result, BeginningOfCentury))
            {
                return false;
            }

            if (result is null)
            {
                throw new InvalidOperationException(validationErrorMessage);
            }
            validationErrorMessage = null;
            return true;
        }

        /// <inheritdoc />
        protected override string FormatValueAsString(T? value)
        {
            return !string.IsNullOrWhiteSpace(EnteredValue) ? EnteredValue : value.FormatDateToShortPersianDate();
        }

        internal void ShowCalendar()
        {
            var persianDay = CurrentValue.ConvertToPersianDay();
            CalendarSelectedYear = persianDay?.Year ?? 0;
            CalendarSelectedMonth = persianDay?.Month ?? 0;
            ShowCalendarPopOver = true;
        }

        internal void OnFocus()
        {
            if (ShowCalendarOnFocus)
            {
                ShowCalendar();
            }
        }

        private void HideCalendar()
        {
            ShowCalendarPopOver = false;
            CalendarSelectedYear = 0;
            CalendarSelectedMonth = 0;
        }

        internal void OnCalendarDayClicked(PersianDay day)
        {
            SetCurrentValue(day.ToString());
        }

        private void SetCurrentValue(string? value)
        {
            EnteredValue = value;
            CurrentValueAsString = value;
        }

        private void SanityCheck()
        {
            if (!Value.IsDateTimeOrDateTimeOffsetType())
            {
                throw new InvalidOperationException($"The `Value` type is not a supported `date` type. DateTime, DateTime?, DateTimeOffset and DateTimeOffset? are supported.");
            }
        }
    }
}