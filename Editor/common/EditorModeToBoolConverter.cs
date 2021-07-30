using System;
using System.Globalization;
using System.Windows.Data;

namespace Editor
{
    public sealed class EditorModeToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            EditorMode mode = (EditorMode)value;
            if (mode == EditorMode.Math)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
