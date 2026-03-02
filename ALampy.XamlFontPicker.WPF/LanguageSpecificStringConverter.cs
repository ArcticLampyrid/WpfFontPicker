using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace ALampy.XamlFontPicker.WPF
{
    internal class LanguageSpecificStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return GetValue((IDictionary<XmlLanguage, string>)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }

        public static string GetValue(IDictionary<XmlLanguage, string> strings)
        {
            if (strings == null)
                return null;
            var localXmlLanguage = XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.Name);
            if (!strings.TryGetValue(localXmlLanguage, out var result))
                result = strings.Values.FirstOrDefault();
            return result;
        }
    }
}
