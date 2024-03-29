using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DNTPersianUtils.Core.IranCities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace DNTPersianComponents.Blazor;

/// <summary>
///     A custom InputIranCities component
/// </summary>
public partial class DntInputIranCities
{
    private int? _cityDivisionCode;
    private string? _selectedCity;
    private string? _selectedCounty;
    private string? _selectedDistrict;
    private string? _selectedProvince;

    /// <summary>
    ///     Additional user attributes
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object> AdditionalAttributes { get; set; } =
        new Dictionary<string, object>(StringComparer.Ordinal);

    /// <summary>
    ///     The label name of the custom InputText
    /// </summary>
    [Parameter]
    public string LabelName { get; set; } = default!;

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
    ///     Fires when a province is selected
    /// </summary>
    [Parameter]
    public string? SelectedProvince
    {
        get => _selectedProvince;
        set
        {
            if (string.Equals(_selectedProvince, value, StringComparison.Ordinal))
            {
                return;
            }

            _selectedProvince = value;
            HideCounties();
            HideDistricts();
            HideCities();
            Counties = Iran.FindCountiesOfSelectedProvince(_selectedProvince);
            NotifySelectedProvinceChanged(_selectedProvince);
        }
    }

    /// <summary>
    ///     Fires when a province is selected
    /// </summary>
    [Parameter]
    public EventCallback<string?> SelectedProvinceChanged { set; get; }

    /// <summary>
    ///     Specifies the field for which validation messages should be displayed.
    /// </summary>
    [Parameter]
    public Expression<Func<string>> SelectedProvinceExpression { get; set; } = default!;

    private BlazorFieldId<string> SelectedProvinceField => new(SelectedProvinceExpression);

    private IEnumerable<string> Counties { get; set; } = Enumerable.Empty<string>();

    /// <summary>
    ///     Fires when a county is selected
    /// </summary>
    [Parameter]
    public string? SelectedCounty
    {
        get => _selectedCounty;
        set => SetSelectedCounty(value);
    }

    /// <summary>
    ///     Fires when a county is selected
    /// </summary>
    [Parameter]
    public EventCallback<string?> SelectedCountyChanged { set; get; }

    /// <summary>
    ///     Specifies the field for which validation messages should be displayed.
    /// </summary>
    [Parameter]
    public Expression<Func<string>> SelectedCountyExpression { get; set; } = default!;

    private BlazorFieldId<string> SelectedCountyField => new(SelectedCountyExpression);

    private IEnumerable<string> Districts { get; set; } = Enumerable.Empty<string>();

    /// <summary>
    ///     Fires when a district is selected
    /// </summary>
    [Parameter]
    public string? SelectedDistrict
    {
        get => _selectedDistrict;
        set => SetSelectedDistrict(value);
    }

    /// <summary>
    ///     Fires when a district is selected
    /// </summary>
    [Parameter]
    public EventCallback<string?> SelectedDistrictChanged { set; get; }

    /// <summary>
    ///     Specifies the field for which validation messages should be displayed.
    /// </summary>
    [Parameter]
    public Expression<Func<string>> SelectedDistrictExpression { get; set; } = default!;

    private BlazorFieldId<string> SelectedDistrictField => new(SelectedDistrictExpression);

    private IEnumerable<City> Cities { get; set; } = Enumerable.Empty<City>();

    /// <summary>
    ///     Fires when a city is selected
    /// </summary>
    [Parameter]
    public string? SelectedCity
    {
        get => _selectedCity;
        set => SetSelectedCity(value);
    }

    /// <summary>
    ///     Fires when a city is selected
    /// </summary>
    [Parameter]
    public EventCallback<string?> SelectedCityChanged { set; get; }

    /// <summary>
    ///     Specifies the field for which validation messages should be displayed.
    /// </summary>
    [Parameter]
    public Expression<Func<string>> SelectedCityExpression { get; set; } = default!;

    private BlazorFieldId<string> SelectedCityField => new(SelectedCityExpression);

    /// <summary>
    ///     Gets the select city.
    /// </summary>
    [Parameter]
    public int? CityDivisionCode
    {
        get => _cityDivisionCode;
        set => SetCityDivisionCode(value);
    }

    /// <summary>
    ///     Fires when a city is selected
    /// </summary>
    [Parameter]
    public EventCallback<int?> CityDivisionCodeChanged { set; get; }

    /// <summary>
    ///     Specifies the field for which validation messages should be displayed.
    /// </summary>
    [Parameter]
    public Expression<Func<int?>> CityDivisionCodeExpression { get; set; } = default!;

    [CascadingParameter] internal EditContext? CascadedEditContext { get; set; }

    private BlazorFieldId<int?> CityField => new(CityDivisionCodeExpression);
    private IEnumerable<string> Provinces { get; set; } = Enumerable.Empty<string>();

    private void SetSelectedCounty(string? value)
    {
        if (string.Equals(_selectedCounty, value, StringComparison.Ordinal))
        {
            return;
        }

        _selectedCounty = value;
        HideDistricts();
        HideCities();
        Districts = Iran.FindDistrictsOfSelectedCounty(_selectedProvince, _selectedCounty);
        NotifySelectedCountyChanged(_selectedCounty);
    }

    private void SetSelectedDistrict(string? value)
    {
        if (string.Equals(_selectedDistrict, value, StringComparison.Ordinal))
        {
            return;
        }

        _selectedDistrict = value;
        HideCities();
        Cities = Iran.FindCitiesOfSelectedDistrict(_selectedProvince, _selectedCounty, _selectedDistrict);
        NotifySelectedDistrictChanged(_selectedDistrict);
    }

    private void SetSelectedCity(string? value)
    {
        if (string.Equals(_selectedCity, value, StringComparison.Ordinal))
        {
            return;
        }

        _selectedCity = value;
        SetCityDivisionCode(FindSelectedCityCode());
        NotifySelectedCityChanged(_selectedCity);
    }

    private void SetCityDivisionCode(int? value)
    {
        if (_cityDivisionCode == value)
        {
            return;
        }

        _cityDivisionCode = value;
        NotifyCityDivisionCodeChanged(_cityDivisionCode);
    }

    private int? FindSelectedCityCode()
    {
        return Cities?.FirstOrDefault(city =>
                                          city.CityName.Equals(_selectedCity, StringComparison.OrdinalIgnoreCase))
                     ?.CityDivisionCode;
    }

    /// <summary>
    ///     Method invoked when the component is ready to start.
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();
        SanityCheck();
        InitData();
    }

    private void InitData()
    {
        Provinces = Iran.IranProvinces;
    }

    private void SanityCheck()
    {
        if (CascadedEditContext is null)
        {
            throw new InvalidOperationException($"{GetType()} requires a cascading parameter " +
                                                $"of type {nameof(EditContext)}. For example, you can use {GetType().FullName} inside " +
                                                $"an {nameof(EditForm)}.");
        }
    }

    private void NotifyCityDivisionCodeChanged(int? city)
    {
        if (CityDivisionCodeChanged.HasDelegate)
        {
            _ = CityDivisionCodeChanged.InvokeAsync(city);
        }

        CityField.NotifyFieldChanged(CascadedEditContext);
    }

    private void NotifySelectedProvinceChanged(string? value)
    {
        if (SelectedProvinceChanged.HasDelegate)
        {
            _ = SelectedProvinceChanged.InvokeAsync(value);
        }

        SelectedProvinceField.NotifyFieldChanged(CascadedEditContext);
    }

    private void NotifySelectedCountyChanged(string? value)
    {
        if (SelectedCountyChanged.HasDelegate)
        {
            _ = SelectedCountyChanged.InvokeAsync(value);
        }

        SelectedCountyField.NotifyFieldChanged(CascadedEditContext);
    }

    private void NotifySelectedDistrictChanged(string? value)
    {
        if (SelectedDistrictChanged.HasDelegate)
        {
            _ = SelectedDistrictChanged.InvokeAsync(value);
        }

        SelectedDistrictField.NotifyFieldChanged(CascadedEditContext);
    }

    private void NotifySelectedCityChanged(string? value)
    {
        if (SelectedCityChanged.HasDelegate)
        {
            _ = SelectedCityChanged.InvokeAsync(value);
        }

        SelectedCityField.NotifyFieldChanged(CascadedEditContext);
    }

    private void HideCities()
    {
        Cities = Enumerable.Empty<City>();
        SetSelectedCity(string.Empty);
    }

    private void HideDistricts()
    {
        Districts = Enumerable.Empty<string>();
        SetSelectedDistrict(string.Empty);
    }

    private void HideCounties()
    {
        Counties = Enumerable.Empty<string>();
        SetSelectedCounty(string.Empty);
        SetCityDivisionCode(0);
    }
}