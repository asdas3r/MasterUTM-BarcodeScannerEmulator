using System;
using System.Globalization;
using System.Windows.Data;

namespace MasterUTM.BarcodeScannerEmulator.Infrastructure.Converters
{
    public class RadioButtonBoolToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int bindingIntValue = (int)value;
            if (bindingIntValue == int.Parse((string)parameter))
                return true;
            else
                return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return parameter;
        }
    }
}
