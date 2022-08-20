using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace DNTPersianComponents.Blazor;

/// <summary>
///     A mouse cursor's position helper
/// </summary>
public static class MouseInterOp
{
    /// <summary>
    ///     Adds window.getMouseClickedPosition function
    /// </summary>
    /// <param name="jsRuntime">Represents an instance of a JavaScript runtime to which calls may be dispatched</param>
    public static ValueTask AddScreenPositionScriptsAsync(this IJSRuntime? jsRuntime)
    {
        if (jsRuntime is null)
        {
            throw new ArgumentNullException(nameof(jsRuntime));
        }

        return jsRuntime.InvokeVoidAsync("eval", @"
					window.getMouseClickedPosition = function(clientX, clientY) {
						let posx = clientX + document.body.scrollLeft + document.documentElement.scrollLeft;
						let posy = clientY + document.body.scrollTop + document.documentElement.scrollTop;
						return { 'PosX': posx.toFixed(1) + 'px', 'PosY': posy.toFixed(1) + 'px' };
					};
			");
    }

    /// <summary>
    ///     Gets the current clicked position of the ElementReference
    /// </summary>
    public static ValueTask<ScreenPosition> GetMouseClickedPositionAsync(
        this IJSRuntime? jsRuntime, double clientX, double clientY)
    {
        if (jsRuntime is null)
        {
            throw new ArgumentNullException(nameof(jsRuntime));
        }

        return jsRuntime.InvokeAsync<ScreenPosition>("getMouseClickedPosition", clientX, clientY);
    }
}