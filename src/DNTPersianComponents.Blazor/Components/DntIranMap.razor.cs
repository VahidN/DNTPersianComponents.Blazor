using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using DNTPersianUtils.Core.IranCities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace DNTPersianComponents.Blazor;

/// <summary>
///     A custom IranMap component
/// </summary>
public partial class DntIranMap
{
    private IranProvince? _hoveredProvince;

    private IranProvince? _selectedProvince;
    private string MapId { set; get; } = $"iranMap{Guid.NewGuid()}";

    private IranProvinceValue<ScreenPosition> ClickedPositions { set; get; } = new();

    private IranProvinceValue<bool> PopOverIsVisible { set; get; } = new();

    /// <summary>
    ///     Represents an instance of a JavaScript runtime to which calls may be dispatched.
    /// </summary>
    [Inject]
    internal IJSRuntime? JSRuntime { set; get; }

    /// <summary>
    ///     When you click at a province, a popover will be displayed.
    ///     This property which is a custom dictionary, defines the titles of the opened popovers.
    /// </summary>
    [SuppressMessage("Microsoft.Usage", "CA2227:Collection properties should be read only",
                        Justification = "It's a Blazor's [Parameter].")]
    [Parameter]
    public IranProvinceValue<string> IranProvincesTitles { set; get; } = new();

    /// <summary>
    ///     When you click at a province, a popover will be displayed.
    ///     This property defined the body template of the opened popover.
    /// </summary>
    [Parameter]
    public RenderFragment<IranProvince>? IranProvincesBody { get; set; }

    /// <summary>
    ///     Fires when a province is selected
    /// </summary>
    [Parameter]
    public IranProvince? SelectedProvince
    {
        get => _selectedProvince;
        set
        {
            if (value.Equals(_selectedProvince))
            {
                return;
            }

            _selectedProvince = value;
            if (SelectedProvinceChanged.HasDelegate)
            {
                _ = SelectedProvinceChanged.InvokeAsync(value);
            }
        }
    }

    /// <summary>
    ///     Fires when a province is selected
    /// </summary>
    [Parameter]
    public EventCallback<IranProvince?> SelectedProvinceChanged { set; get; }

    /// <summary>
    ///     Fires when a province is hovered over by mouse.
    /// </summary>
    [Parameter]
    public IranProvince? HoveredProvince
    {
        get => _hoveredProvince;
        set
        {
            if (value.Equals(_hoveredProvince))
            {
                return;
            }

            _hoveredProvince = value;
            if (HoveredProvinceChanged.HasDelegate)
            {
                _ = HoveredProvinceChanged.InvokeAsync(value);
            }
        }
    }

    /// <summary>
    ///     Fires when a province is hovered over by mouse.
    /// </summary>
    [Parameter]
    public EventCallback<IranProvince?> HoveredProvinceChanged { set; get; }

    /// <summary>
    ///     The list of styles of Iran's provinces.
    /// </summary>
    [SuppressMessage("Microsoft.Usage", "CA2227:Collection properties should be read only",
                        Justification = "It's a Blazor's [Parameter].")]
    [Parameter]
    public IranProvinceValue<string> IranProvincesStyles { set; get; } = new();

    /// <summary>
    ///     The list of CSS classes of Iran's provinces.
    /// </summary>
    [SuppressMessage("Microsoft.Usage", "CA2227:Collection properties should be read only",
                        Justification = "It's a Blazor's [Parameter].")]
    [Parameter]
    public IranProvinceValue<string> IranProvincesCss { set; get; } = new();


    /// <summary>
    ///     The map's font-family. Its default value is `Samim`.
    /// </summary>
    [Parameter]
    public string FontFamily { get; set; } = "Samim";

    /// <summary>
    ///     The map's font-size. Its default value is `11px`.
    /// </summary>
    [Parameter]
    public string FontSize { get; set; } = "11px";

    /// <summary>
    ///     The map's margin. Its default value is `3`.
    /// </summary>
    [Parameter]
    public int Margin { get; set; } = 3;

    /// <summary>
    ///     The fill color of the map's border. Its default value is `none`.
    /// </summary>
    [Parameter]
    public string BorderFill { get; set; } = "none";

    /// <summary>
    ///     The stroke-width of the map's border. Its default value is `2`.
    /// </summary>
    [Parameter]
    public int BorderStrokeWidth { get; set; } = 2;

    /// <summary>
    ///     The stroke color of the map's border. Its default value is `black`.
    /// </summary>
    [Parameter]
    public string BorderStroke { get; set; } = "black";

    /// <summary>
    ///     The fill color of the map's seas. Its default value is `#6caed8`.
    /// </summary>
    [Parameter]
    public string SeaFill { get; set; } = "#6caed8";

    /// <summary>
    ///     The fill color of the map's lakes. Its default value is `#6caed8`.
    /// </summary>
    [Parameter]
    public string LakeFill { get; set; } = "#6caed8";

    /// <summary>
    ///     The fill color of the map's islands. Its default value is `white`.
    /// </summary>
    [Parameter]
    public string IslandFill { get; set; } = "white";

    /// <summary>
    ///     The fill color of the map's sea names. Its default value is `white`.
    /// </summary>
    [Parameter]
    public string SeaNamesFill { get; set; } = "white";

    /// <summary>
    ///     The fill color of the map's province names. Its default value is `black`.
    /// </summary>
    [Parameter]
    public string ProvinceNamesFill { get; set; } = "black";

    /// <summary>
    ///     The style of the map's provinces names.
    /// </summary>
    [SuppressMessage("Microsoft.Usage", "CA2227:Collection properties should be read only",
                        Justification = "It's a Blazor's [Parameter].")]
    [Parameter]
    public IranProvinceValue<string> ProvinceNamesStyles { get; set; } = new();

    /// <summary>
    ///     The stroke style of a province. Its default value is `black`.
    /// </summary>
    [Parameter]
    public string ProvinceStroke { set; get; } = "black";

    /// <summary>
    ///     The stroke width of a province. Its default value is `2`.
    /// </summary>
    [Parameter]
    public int ProvinceStrokeWidth { set; get; } = 2;

    /// <summary>
    ///     The fill color of a province. Its default value is `white`.
    /// </summary>
    [Parameter]
    public string ProvinceFill { set; get; } = "white";

    /// <summary>
    ///     The style of `map-sea-names`.
    /// </summary>
    [Parameter]
    public string MapSeaNamesStyle { get; set; } = default!;

    /// <summary>
    ///     The CSS class of `map-sea-names`.
    /// </summary>
    [Parameter]
    public string MapSeaNamesCss { get; set; } = default!;

    /// <summary>
    ///     The CSS class of `main-svg`.
    /// </summary>
    [Parameter]
    public string MainSvgStyle { get; set; } = default!;

    /// <summary>
    ///     Should we display the names of the provinces?
    ///     Its default value is `true`.
    /// </summary>
    [Parameter]
    public bool ShowProvincesNames { get; set; } = true;

    private string ProvinceStyle =>
        $"stroke: {ProvinceStroke};  stroke-width: {ProvinceStrokeWidth};  fill: {ProvinceFill}; cursor: pointer;";

    private string ProvinceNameStyle => $"fill:{ProvinceNamesFill}; cursor: pointer;";

    private async Task ProvinceClickedAsync(IranProvince province, MouseEventArgs args)
    {
        if (IranProvincesTitles.Count > 0)
        {
            var position = await JSRuntime.GetMouseClickedPositionAsync(args.ClientX, args.ClientY);
            ClickedPositions[province] = position;
            PopOverIsVisible[province] = true;
        }

        SelectedProvince = province;
    }

    private void ProvinceOnMouseOver(IranProvince province)
    {
        HoveredProvince = province;
    }

    private void ProvinceOnMouseOut()
    {
        HoveredProvince = null;
    }

    /// <summary>
    ///     Method invoked after each time the component has been rendered.
    /// </summary>
    /// <param name="firstRender">Set to true if this is the first time</param>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JSRuntime.AddScreenPositionScriptsAsync();
        }
    }
}