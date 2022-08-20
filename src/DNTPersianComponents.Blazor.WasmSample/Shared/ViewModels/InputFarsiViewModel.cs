using System.ComponentModel.DataAnnotations;

namespace DNTPersianComponents.Blazor.WasmSample.Client.ViewModels;

public class InputFarsiViewModel
{
    [Required] public string Description { set; get; }

    [Required] public string Text { set; get; }
}