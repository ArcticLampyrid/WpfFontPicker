using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ALampy.XamlFontPicker.WPF
{
    public static class PickedFontInfoAccessor
    {
        public static PickedFontInfo From(TextBlock textBlock)
        {
            if (textBlock == null) throw new System.ArgumentNullException(nameof(textBlock));
            return new PickedFontInfo(
                textBlock.FontFamily,
                textBlock.FontStretch,
                textBlock.FontStyle,
                textBlock.FontWeight,
                textBlock.FontSize);
        }

        public static PickedFontInfo From(Control control)
        {
            if (control == null) throw new System.ArgumentNullException(nameof(control));
            return new PickedFontInfo(
                control.FontFamily,
                control.FontStretch,
                control.FontStyle,
                control.FontWeight,
                control.FontSize);
        }

        public static void ApplyTo(TextBlock textBlock, PickedFontInfo fontInfo)
        {
            if (textBlock == null) throw new System.ArgumentNullException(nameof(textBlock));
            if (fontInfo == null) throw new System.ArgumentNullException(nameof(fontInfo));
            textBlock.FontFamily = fontInfo.Family;
            textBlock.FontStretch = fontInfo.Stretch;
            textBlock.FontStyle = fontInfo.Style;
            textBlock.FontWeight = fontInfo.Weight;
            textBlock.FontSize = fontInfo.Size;
        }

        public static void ApplyTo(Control control, PickedFontInfo fontInfo)
        {
            if (control == null) throw new System.ArgumentNullException(nameof(control));
            if (fontInfo == null) throw new System.ArgumentNullException(nameof(fontInfo));
            control.FontFamily = fontInfo.Family;
            control.FontStretch = fontInfo.Stretch;
            control.FontStyle = fontInfo.Style;
            control.FontWeight = fontInfo.Weight;
            control.FontSize = fontInfo.Size;
        }
    }
}
