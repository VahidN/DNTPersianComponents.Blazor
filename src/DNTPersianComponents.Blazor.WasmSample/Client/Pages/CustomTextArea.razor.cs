﻿using System;
using System.Text.Json;
using System.Threading.Tasks;
using DNTPersianComponents.Blazor.WasmSample.Client.ViewModels;
using Microsoft.AspNetCore.Components;

namespace DNTPersianComponents.Blazor.WasmSample.Client.Pages;

public partial class CustomTextArea : ComponentBase
{
    private RegisterViewModel Model { set; get; } = new();

    private async Task DoRegister()
    {
        await Task.Delay(2000);
        Console.WriteLine(JsonSerializer.Serialize(Model));
    }

    protected override void OnInitialized()
    {
        Model = new RegisterViewModel
                {
                    UserName = "وحيد",
                    Description = "توضيحات",
                };
    }
}