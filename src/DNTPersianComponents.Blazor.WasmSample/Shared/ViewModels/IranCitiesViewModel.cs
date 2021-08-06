using System.ComponentModel.DataAnnotations;

namespace DNTPersianComponents.Blazor.WasmSample.Client.ViewModels
{
    public class IranCitiesViewModel
    {
        [Required]
        public string UserName { set; get; }

        [Required]
        public string ProvinceName { get; set; }

        [Required]
        public string CountyName { get; set; }

        public string DistrictName { get; set; }

        public string CityName { get; set; }

        public int? CityDivisionCode { get; set; }
    }
}