using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Editor
{
    /// <summary>
    /// Interaction logic for CustomZoomWindow.xaml
    /// </summary>
    public partial class CustomZoomWindow : Window
    {
        int maxPercentage = 9999;
        MainWindow mainWindow = null;
        public CustomZoomWindow(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            InitializeComponent();
            numberBox.Focus();
        }

        private bool AreAllValidNumericChars(string str)
        {
            foreach (char c in str)
            {
                if (!Char.IsNumber(c)) return false;
            }

            return true;
        }

        private void numberBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !AreAllValidNumericChars(e.Text) || numberBox.Text.Length > 3;
            base.OnPreviewTextInput(e);
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int number = int.Parse(numberBox.Text);
                if (number <= 0 || number > maxPercentage)
                {
                    MessageBox.Show("Zoom percentage must be between 1 and " + maxPercentage + ".");
                    return;
                }
                mainWindow.SetFontSizePercentage(number);
                this.Close();
            }
            catch 
            {
                MessageBox.Show("Zoom percentage must be a number between 1 and " + maxPercentage + ".");
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
