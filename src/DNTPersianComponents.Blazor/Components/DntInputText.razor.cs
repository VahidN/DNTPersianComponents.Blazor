using DNTPersianUtils.Core;
using Microsoft.AspNetCore.Components;

namespace DNTPersianComponents.Blazor
{
    /// <summary>
    /// A custom InputText
    /// </summary>
    public partial class DntInputText
    {
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
        /// The input type. Its default value is `text`.
        /// </summary>
        [Parameter] public string InputType { set; get; } = "text";

        /// <summary>
        /// Allows the browser to choose a correct autocomplete for the this fields.
        /// Make sure `sync` is on for the Chrome, otherwise you won't see the `Suggest Strong Password` option in Chrome.
        /// </summary>
        [Parameter] public string AutoComplete { get; set; } = "off";

        private BlazorFieldId<string?> ValueField => new(ValueExpression);
        private string Direction { set; get; } = "ltr";

        /// <summary>
        /// Method invoked when the component has received parameters from its parent.
        /// </summary>
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
        }

        internal void OnInput(ChangeEventArgs e)
        {
            var value = e.Value as string;
            CurrentValueAsString = value;
            Direction = value.ContainsFarsi() ? "rtl" : "ltr";
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
                Direction = CurrentValueAsString.ContainsFarsi() ? "rtl" : "ltr";
                ValueField.NotifyFieldChanged(EditContext);
            }
        }
    }
}