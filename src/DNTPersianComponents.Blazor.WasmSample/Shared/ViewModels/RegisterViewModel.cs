using System.ComponentModel.DataAnnotations;

namespace DNTPersianComponents.Blazor.WasmSample.Client.ViewModels;

public class RegisterViewModel
{
    [Required] public string UserName { set; get; }

    [Required] public string Description { set; get; }
}