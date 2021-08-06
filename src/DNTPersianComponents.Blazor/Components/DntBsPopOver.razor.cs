using System.Reflection.Metadata;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace DNTPersianComponents.Blazor
{
    /// <summary>
    /// A custom PopOver component
    /// </summary>
    public partial class DntBsPopOver : ComponentBase
    {
        private bool _isVisible;

        /// <summary>
        /// The ChildContent to be rendered
        /// </summary>
        [Parameter] public RenderFragment? PopOverBody { get; set; }

        /// <summary>
        /// The ChildContent to be rendered
        /// </summary>
        [Parameter] public RenderFragment? PopOverHeader { get; set; }

        /// <summary>
        /// Additional user attributes
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, object> AdditionalAttributes { get; set; } = new Dictionary<string, object>(StringComparer.Ordinal);

        /// <summary>
        /// The left position of the popover. Its default value is `20px`.
        /// </summary>
        [Parameter] public string PopOverLeft { set; get; } = "20px";

        /// <summary>
        /// The top position of the popover. Its default value is `20px`.
        /// </summary>
        [Parameter] public string PopOverTop { set; get; } = "20px";

        /// <summary>
        /// The z-index of the popover. Its default value is `2021`.
        /// </summary>
        [Parameter] public int ZIndex { set; get; } = 2021;

        /// <summary>
        /// The header's label
        /// </summary>
        [Parameter] public string HeaderLabel { get; set; } = default!;

        /// <summary>
        /// The close button's title. Its default value is `Close`.
        /// </summary>
        [Parameter] public string CloseTitle { get; set; } = "Close";

        /// <summary>
        /// Is this popover visible? It can be defined as `@bind-IsVisible="@ShowPopOver"`.
        /// </summary>
        [Parameter]
        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                if (_isVisible == value)
                {
                    return;
                }

                _isVisible = value;
                if (IsVisibleChanged.HasDelegate)
                {
                    _ = IsVisibleChanged.InvokeAsync(_isVisible);
                }
            }
        }

        /// <summary>
        /// Fires when a month is selected.
        /// </summary>
        [Parameter] public EventCallback<bool> IsVisibleChanged { set; get; }

        internal void ClosePopOver()
        {
            IsVisible = false;
        }
    }
}