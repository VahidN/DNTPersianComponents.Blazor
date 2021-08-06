using Microsoft.AspNetCore.Components;
using DNTPersianUtils.Core;

namespace DNTPersianComponents.Blazor
{
    /// <summary>
    /// A custom InputNumber
    /// </summary>
    public partial class DntInputNumber<T>
    {
        private string? _enteredValue;

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

        private BlazorFieldId<T> ValueField => new(ValueExpression);

        /// <summary>
        /// Method invoked when the component has received parameters from its parent.
        /// </summary>
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
        }

        private string? EnteredValue
        {
            set => _enteredValue = value;
            get => UsePersianNumbers ? _enteredValue.ToPersianNumbers() : _enteredValue;
        }

        internal void OnInput(ChangeEventArgs e)
        {
            SetCurrentValue(e.Value as string);
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
                ValueField.NotifyFieldChanged(EditContext); //TODO: add it everywhere for activating validation on edit forms
            }
        }

        private void SetCurrentValue(string? value)
        {
            EnteredValue = value; //TODO: set caret pos??
            CurrentValueAsString = value.ToEnglishNumbers(); //TODO: add enter a number from popover like banks for security
        }
    }
}