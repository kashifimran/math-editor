using System;
using System.Globalization;
using System.Windows;

namespace Editor
{
    public partial class MatrixInputWindow : Window
    {
        public event Action<int, int> ProcessRequest = (x, y) => { };

        public MatrixInputWindow()
        {
            InitializeComponent();
        }

        public MatrixInputWindow(int rows, int columns)
        {
            InitializeComponent();
            rowsUpDown.Text = rows.ToString(CultureInfo.CurrentUICulture);
            columnsUpDown.Text = columns.ToString(CultureInfo.CurrentUICulture);
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            // WPF does not provide a NumericUpDown control out of the box
            // The button click will be ignored, when the number cannot be parsed.
            // TODO: provide user feedback when the input is invalid or implement a proper NumericUpDownControl

            if (int.TryParse(rowsUpDown.Text, NumberStyles.Integer, CultureInfo.CurrentUICulture, out var rows)
                && int.TryParse(columnsUpDown.Text, NumberStyles.Integer, CultureInfo.CurrentUICulture, out var columns))
            {
                ProcessRequest(rows, columns);
                Close();
            }
        }
    }
}
