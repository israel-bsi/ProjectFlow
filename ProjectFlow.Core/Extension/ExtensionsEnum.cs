﻿using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ProjectFlow.Core.Extension;

public static class ExtensionsEnum
{
    public static string GetDisplayName(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var displayAttribute = field?.GetCustomAttribute<DisplayAttribute>();
        return displayAttribute?.Name ?? value.ToString();
    }
}