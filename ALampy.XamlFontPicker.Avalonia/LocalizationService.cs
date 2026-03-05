using System;
using System.Globalization;
using Avalonia;
using Avalonia.Styling;

namespace ALampy.XamlFontPicker.Avalonia
{
    public static class LocalizationService
    {
        public static string GetString(string key, string fallback)
        {
            var resources = Application.Current?.Resources;
            if (resources == null)
                return fallback;

            if (resources.TryGetResource(key, ThemeVariant.Default, out var value) && value is string str)
                return str;

            return fallback;
        }
    }
}
