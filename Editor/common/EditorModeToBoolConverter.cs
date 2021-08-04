using System;
using System.Globalization;
using System.Windows.Data;

namespace Editor
{
    [ValueConversion(typeof(EditorMode), typeof(bool))]
    public sealed class EditorModeToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            value is EditorMode mode && mode == EditorMode.Text;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            value is bool mode && mode ? EditorMode.Text : EditorMode.Math;
    }
}
