﻿using System;
using System.Text.Json;
using System.Threading.Tasks;
using DNTPersianComponents.Blazor.WasmSample.Client.ViewModels;
using Microsoft.AspNetCore.Components;

namespace DNTPersianComponents.Blazor.WasmSample.Client.Pages;

public partial class InputFarsi : ComponentBase
{
    private InputFarsiViewModel Model { get; } = new();

    private async Task DoRegister()
    {
        await Task.Delay(1000);
        Console.WriteLine(JsonSerializer.Serialize(Model));
    }
}