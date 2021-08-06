using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using DNTPersianUtils.Core;
using Microsoft.JSInterop;

namespace DNTPersianComponents.Blazor
{
    /// <summary>
    /// A custom CurrencyInput component with thousands separator
    /// </summary>
    public partial class DntInputCurrency<T>
    {
        private ElementReference ReferenceToInputControl { set; get; } = default!;

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
        /// Should we show the entered number with Persian numbers?
        /// </summary>
        [Parameter] public bool UsePersianNumbers { set; get; }

        /// <summary>
        /// Should we show the entered number as a human readable text?
		/// Its default value is `true`.
        /// </summary>
		[Parameter] public bool ShowNumberToText { set; get; } = true;

        /// <summary>
        /// The number to word language
        /// </summary>
        [Parameter] public Language NumberToTextLanguage { set; get; }

        /// <summary>
        /// Represents an instance of a JavaScript runtime to which calls may be dispatched.
        /// </summary>
        [Inject] internal IJSRuntime? JSRuntime { set; get; }

        private BlazorFieldId<T> ValueField => new(ValueExpression);

        private string HumanReadableInteger { set; get; } = default!;

        private string NumberToTextDirection => NumberToTextLanguage == Language.Persian ? "rtl" : "ltr";

        /// <inheritdoc />
        protected override bool TryParseValueFromString(
            string? value,
            [MaybeNullWhen(false)] out T result,
            [NotNullWhen(false)] out string? validationErrorMessage)
        {
            if (ulong.TryParse(
                    value.ToEnglishNumbers(),
                    NumberStyles.Any | NumberStyles.AllowThousands, CultureInfo.InvariantCulture,
                    out var number))
            {
                validationErrorMessage = null;
                result = (T)Convert.ChangeType(number, typeof(T), CultureInfo.InvariantCulture);
                if (ShowNumberToText)
                {
                    HumanReadableInteger = number.NumberToText(NumberToTextLanguage);
                }
                return true;
            }

            validationErrorMessage = string.Format(
                                                CultureInfo.InvariantCulture,
                                                ParsingErrorMessage,
                                                DisplayName ?? FieldIdentifier.FieldName);
            result = default;
            HumanReadableInteger = string.Empty;
            return false;
        }

        /// <inheritdoc />
        protected override string? FormatValueAsString(T? value)
        {
            var formattedValue = string.Format(CultureInfo.InvariantCulture, "{0:n0}", value);
            return UsePersianNumbers ? formattedValue.ToPersianNumbers() : formattedValue;
        }

        internal async Task OnInputAsync(ChangeEventArgs e)
        {
            //TODO: everywhere we have CurrentValueAsString => set caret pos??
            var caretPosition = await JSRuntime.GetCaretPositionAsync(ReferenceToInputControl);

            var value = e.Value as string;
            var valueLengthBeforeFormatting = value?.Length ?? 0;
            CurrentValueAsString = value.ToEnglishNumbers();
            var valueLengthAfterFormatting = CurrentValueAsString?.Length ?? 0;

            await JSRuntime.SetCaretPositionAsync(ReferenceToInputControl,
                        caretPosition + valueLengthAfterFormatting - valueLengthBeforeFormatting);
        }

        /// <summary>
        /// Method invoked after each time the component has been rendered.
        /// </summary>
        /// <param name="firstRender">Set to true if this is the first time</param>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                await JSRuntime.AddCaretPositionScriptsAsync();
                ShowInitialHumanReadableInteger();
                ValueField.NotifyFieldChanged(EditContext);
            }
        }

        private void ShowInitialHumanReadableInteger()
        {
            if (ShowNumberToText)
            {
                HumanReadableInteger = CurrentValue.ConvertNumberToText(NumberToTextLanguage);
            }
        }
    }
}