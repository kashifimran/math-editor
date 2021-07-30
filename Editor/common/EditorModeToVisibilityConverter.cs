using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Editor
{
    public class EditorModeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            EditorMode mode = (EditorMode)value;
            if (mode == EditorMode.Math)
            {
                return Visibility.Collapsed;
            }
            else
            {
                return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
