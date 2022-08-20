using DNTPersianUtils.Core.IranCities;
using Microsoft.AspNetCore.Components;

namespace DNTPersianComponents.Blazor.WasmSample.Client.Pages;

public partial class ShowIranMap : ComponentBase
{
    private IranProvince? SelectedProvince { set; get; }

    private IranProvince? HoveredProvince { set; get; }

    private IranProvinceValue<string> IranProvincesStyles { get; } = new()
                                                                     {
                                                                         {
                                                                             IranProvince.AzerbaijanEast,
                                                                             "fill: yellow;"
                                                                         },
                                                                     };

    private IranProvinceValue<string> ProvinceNamesStyles { get; } = new()
                                                                     {
                                                                         { IranProvince.AzerbaijanEast, "fill: red;" },
                                                                     };

    private IranProvinceValue<string> IranProvincesTitles { get; } = new()
                                                                     {
                                                                         {
                                                                             IranProvince.AzerbaijanEast,
                                                                             "این عنوان سفارشی آذربایجان شرقی است"
                                                                         },
                                                                     };
}