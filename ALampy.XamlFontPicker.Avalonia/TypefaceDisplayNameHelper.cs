using System.Collections.Generic;
using System.Linq;
using Avalonia.Media;

namespace ALampy.XamlFontPicker.Avalonia
{
    internal static class TypefaceDisplayNameHelper
    {
        public static string GetDisplayName(FontWeight weight, FontStyle style, FontStretch stretch)
        {
            var parts = new List<string>
            {
                weight.ToString(),
                style.ToString(),
                stretch.ToString()
            };

            var result = string.Join(" ", parts.Where(p => p != "Normal"));
            return string.IsNullOrEmpty(result) ? "Regular" : result;
        }
    }
}
