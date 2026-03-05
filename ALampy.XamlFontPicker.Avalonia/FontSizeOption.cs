using System.Collections.Generic;
using System.Globalization;

namespace ALampy.XamlFontPicker.Avalonia
{
    public class FontSizeOption
    {
        private const double PixelsPerPoint = 96.0 / 72.0;

        public FontSizeOption(double pointSize)
        {
            PointSize = pointSize;
            PixelSize = pointSize * PixelsPerPoint;
        }

        public double PointSize { get; }

        public double PixelSize { get; }

        public override string ToString()
        {
            return PointSize.ToString("0.##", CultureInfo.CurrentCulture);
        }

        public static IList<FontSizeOption> GetDefaultSizeOptions()
        {
            return new List<FontSizeOption>
            {
                new FontSizeOption(72),
                new FontSizeOption(48),
                new FontSizeOption(36),
                new FontSizeOption(28),
                new FontSizeOption(26),
                new FontSizeOption(24),
                new FontSizeOption(22),
                new FontSizeOption(20),
                new FontSizeOption(18),
                new FontSizeOption(16),
                new FontSizeOption(14),
                new FontSizeOption(12),
                new FontSizeOption(11),
                new FontSizeOption(10),
                new FontSizeOption(9),
                new FontSizeOption(8)
            };
        }
    }
}
