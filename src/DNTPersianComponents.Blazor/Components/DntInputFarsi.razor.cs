using System;
using System.Threading.Tasks;
using DNTPersianUtils.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace DNTPersianComponents.Blazor
{
    /// <summary>
    /// A custom FarsiInput component
    /// </summary>
    public partial class DntInputFarsi
    {
        private bool IsShiftPressed { set; get; }
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
        /// The input type. Its default value is `text`.
        /// </summary>
        [Parameter] public string InputType { set; get; } = "text";

        /// <summary>
        /// Allows the browser to choose a correct autocomplete for the this fields.
        /// Make sure `sync` is on for the Chrome, otherwise you won't see the `Suggest Strong Password` option in Chrome.
        /// </summary>
        [Parameter] public string AutoComplete { get; set; } = "off";

        /// <summary>
        /// The input's direction. Its default value is `rtl`.
        /// </summary>
        [Parameter] public string Direction { set; get; } = "rtl";

        /// <summary>
        /// Should we convert English numbers to Persian numbers? Its default value is `true`.
        /// </summary>
        [Parameter] public bool ConvertEnglishNumbers { set; get; } = true;

        /// <summary>
        /// Represents an instance of a JavaScript runtime to which calls may be dispatched.
        /// </summary>
        [Inject] internal IJSRuntime? JSRuntime { set; get; }

        private BlazorFieldId<string?> ValueField => new(ValueExpression);

        /// <summary>
        /// Method invoked when the component has received parameters from its parent.
        /// </summary>
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
        }

        /// <summary>
        /// Method invoked after each time the component has been rendered.
        /// </summary>
        /// <param name="firstRender">Set to true if this is the first time</param>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JSRuntime.AddCaretPositionScriptsAsync();
            }
        }

        internal async Task OnInputAsync(ChangeEventArgs e)
        {
            var caretPosition = await JSRuntime.GetCaretPositionAsync(ReferenceToInputControl);
            var value = (e.Value as string).ConvertTypedTextToPersian(caretPosition, IsShiftPressed, ConvertEnglishNumbers);
            CurrentValueAsString = value;
            await JSRuntime.SetCaretPositionAsync(ReferenceToInputControl, caretPosition);
        }

        internal void KeyboardEventHandler(KeyboardEventArgs args)
        {
            IsShiftPressed = args.ShiftKey && string.Equals(args.Type, "keydown", StringComparison.Ordinal);
        }
    }
}