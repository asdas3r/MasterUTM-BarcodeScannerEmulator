using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MasterUTM.BarcodeScannerEmulator.Infrastructure.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public bool Hidden { get; set; } = false;
        public bool Reversed { get; set; } = false;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool boolVal = (bool)value;

            if (boolVal ^ Reversed)
                return Visibility.Visible;
            else
            {
                if (Hidden)
                    return Visibility.Hidden;
                else
                    return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility visibility = (Visibility)value;

            if (visibility == Visibility.Visible)
                return true;
            else
                return false;
        }
    }
}
