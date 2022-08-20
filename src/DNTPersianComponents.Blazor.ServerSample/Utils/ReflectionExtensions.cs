using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace DNTPersianComponents.Blazor.ServerSample.Utils;

/// <summary>
///     Reflection Extensions
/// </summary>
public static class ReflectionExtensions
{
    /// <summary>
    ///     Gets the display text for an enum value.
    ///     Uses the DisplayAttribute if set on the enum member, so this support localization.
    /// </summary>
    public static string GetDisplayName<TEnum>(this TEnum value)
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        var name = value.ToString();
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentNullException(nameof(value));
        }

        var member = value.GetType().GetMember(name)[0];
        var displayAttribute = member.GetCustomAttribute<DisplayAttribute>();
        return displayAttribute?.GetName() ?? name;
    }
}