using System;
using System.Text.Json;
using System.Threading.Tasks;
using DNTPersianComponents.Blazor.WasmSample.Client.ViewModels;
using Microsoft.AspNetCore.Components;

namespace DNTPersianComponents.Blazor.ServerSample.Pages;

public partial class CurrencyInput : ComponentBase
{
    private CurrencyInputViewModel Model { get; } = new();

    private async Task DoSave()
    {
        await Task.Delay(2000);
        Console.WriteLine(JsonSerializer.Serialize(Model));
    }
}