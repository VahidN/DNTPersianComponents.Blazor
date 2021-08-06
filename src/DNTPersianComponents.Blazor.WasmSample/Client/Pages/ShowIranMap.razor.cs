using Microsoft.AspNetCore.Components;
using DNTPersianUtils.Core.IranCities;

namespace DNTPersianComponents.Blazor.WasmSample.Client.Pages
{
    public partial class ShowIranMap : ComponentBase
    {
        private IranProvince? SelectedProvince { set; get; }

        private IranProvince? HoveredProvince { set; get; }

        private IranProvinceValue<string> IranProvincesStyles { set; get; } = new()
        {
            { IranProvince.AzerbaijanEast, "fill: yellow;" }
        };

        private IranProvinceValue<string> ProvinceNamesStyles { set; get; } = new()
        {
            { IranProvince.AzerbaijanEast, "fill: red;" }
        };

        private IranProvinceValue<string> IranProvincesTitles { set; get; } = new()
        {
            { IranProvince.AzerbaijanEast, "این عنوان سفارشی آذربایجان شرقی است" },
        };
    }
}