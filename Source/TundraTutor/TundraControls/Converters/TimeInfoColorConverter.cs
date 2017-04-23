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
            int type = (int)value;
            switch (type) { 
                case 1:
                    return new SolidColorBrush(Color.FromRgb(255, 0, 0));
                case 2:
                    return new SolidColorBrush(Color.FromRgb(0, 124, 255));
                case 3:
                    return new SolidColorBrush(Color.FromArgb(50, 255, 0, 0));
                default:
                    return new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
                    };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}