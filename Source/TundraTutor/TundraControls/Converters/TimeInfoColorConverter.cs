using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace TundraControls.Converters
{
    public class TimeInfoColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string info = (string)value;
            if (string.IsNullOrEmpty(info)) return null;
            else if (info == "X") return new SolidColorBrush(Color.FromRgb(255, 0, 0));
            else return new SolidColorBrush(Color.FromRgb(0, 124, 255));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}