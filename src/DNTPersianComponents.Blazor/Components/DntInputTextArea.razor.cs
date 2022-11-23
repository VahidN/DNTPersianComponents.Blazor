using DNTPersianUtils.Core;
using Microsoft.AspNetCore.Components;

namespace DNTPersianComponents.Blazor;

/// <summary>
///     A custom InputTextArea
/// </summary>
public partial class DntInputTextArea
{
    /// <summary>
    ///     The InputText's margin bottom. Its default value is `3`.
    /// </summary>
    [Parameter]
    public int InputRowMarginBottom { get; set; } = 3;

    /// <summary>
    ///     The InputText's column width. Its default value is `9`.
    /// </summary>
    [Parameter]
    public int InputTextColumnWidth { get; set; } = 9;

    /// <summary>
    ///     The label's column width of the custom InputText. Its default value is `3`.
    /// </summary>
    [Parameter]
    public int LabelColumnWidth { get; set; } = 3;

    /// <summary>
    ///     The label name of the custom InputText
    /// </summary>
    [Parameter]
    public string LabelName { get; set; } = default!;

    /// <summary>
    ///     Input field's icon from https://icons.getbootstrap.com/.
    /// </summary>
    [Parameter]
    public string FieldIcon { set; get; } = default!;

    private BlazorFieldId<string?> ValueField => new(ValueExpression);
    private string? Direction { set; get; }

    /// <summary>
    ///     Method invoked when the component has received parameters from its parent.
    /// </summary>
    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        SetDirection(CurrentValueAsString);
    }

    private void OnInput(ChangeEventArgs e)
    {
        var value = e.Value as string;
        CurrentValueAsString = value;
        SetDirection(value);
    }

    /// <summary>
    ///     Method invoked after each time the component has been rendered.
    /// </summary>
    /// <param name="firstRender">Set to true if this is the first time</param>
    protected override void OnAfterRender(bool firstRender)
    {
        base.OnAfterRender(firstRender);
        if (firstRender)
        {
            SetDirection(CurrentValueAsString);
        }
    }

    private void SetDirection(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            Direction = null;
        }
        else
        {
            Direction = value.ContainsFarsi() ? "rtl" : "ltr";
        }
    }
}