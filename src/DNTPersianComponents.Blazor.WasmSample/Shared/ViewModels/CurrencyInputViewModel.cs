using System.ComponentModel.DataAnnotations;

namespace DNTPersianComponents.Blazor.WasmSample.Client.ViewModels
{
    public class CurrencyInputViewModel
    {
        [Required]
        public string Name { set; get; }

        [Required]
        [Range(0, 9000000000)]
        public long Price { set; get; } = 1234;
    }
}