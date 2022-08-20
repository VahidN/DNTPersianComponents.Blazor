using System.Collections.Generic;

namespace DNTPersianComponents.Blazor;

/// <summary>
///     Defines location of the Iran's provinces
/// </summary>
public class IranProvinceNameLocations
{
    /// <summary>
    ///     Provides the SVG path data of the Iran's province.
    /// </summary>
    public string PathData { set; get; } = default!;

    /// <summary>
    ///     Provides name of the Iran's province.
    /// </summary>
    public IEnumerable<Dictionary<string, object>> TextsAttributes { set; get; } =
        new List<Dictionary<string, object>>();
}