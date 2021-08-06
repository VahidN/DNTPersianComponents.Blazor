using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;

namespace DNTPersianComponents.Blazor
{
    /// <summary>
    /// An input's caret position helper
    /// </summary>
    public static class CaretInterOp
    {
        /// <summary>
        /// Adds window.getCaretPosition and window.setCaretPosition functions
        /// </summary>
        /// <param name="jsRuntime">Represents an instance of a JavaScript runtime to which calls may be dispatched</param>
		public static ValueTask AddCaretPositionScriptsAsync(this IJSRuntime? jsRuntime)
        {
            if (jsRuntime is null)
            {
                throw new ArgumentNullException(nameof(jsRuntime));
            }

            return jsRuntime.InvokeVoidAsync("eval", @"
			    window.getCaretPosition = function(element) {
					return element.selectionStart;
				};
				window.setCaretPosition = function(element, index) {
					element.focus();
					element.selectionStart = index;
					element.selectionEnd = index;
				};
			");
        }

        /// <summary>
        /// Gets the current caret position of the ElementReference
        /// </summary>
		public static ValueTask<int> GetCaretPositionAsync(
                this IJSRuntime? jsRuntime, ElementReference? referenceToInputControl)
        {
            if (jsRuntime is null)
            {
                throw new ArgumentNullException(nameof(jsRuntime));
            }

            if (referenceToInputControl is null)
            {
                throw new ArgumentNullException(nameof(referenceToInputControl));
            }

            return jsRuntime.InvokeAsync<int>("getCaretPosition", referenceToInputControl);
        }

        /// <summary>
        /// Sets the current caret position of the ElementReference
        /// </summary>
		public static ValueTask SetCaretPositionAsync(
                this IJSRuntime? jsRuntime, ElementReference? referenceToInputControl, int index)
        {
            if (jsRuntime is null)
            {
                throw new ArgumentNullException(nameof(jsRuntime));
            }

            if (referenceToInputControl is null)
            {
                throw new ArgumentNullException(nameof(referenceToInputControl));
            }

            return jsRuntime.InvokeVoidAsync("setCaretPosition", referenceToInputControl, index);
        }
    }
}