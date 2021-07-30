using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Editor
{
    /// <summary>
    /// Interaction logic for CustomZoomWindow.xaml
    /// </summary>
    public partial class CodepointWindow : Window
    {
        string numberBase = "10";

        public CodepointWindow()
        {
            InitializeComponent();
            numberBox.Focus();
        }

        private bool ConvertToNumber(string str, out uint number)
        {
            try
            {
                switch (numberBase)
                {
                    case "8":
                        number = Convert.ToUInt32(str, 8);
                        return true;
                    case "10":
                        number = uint.Parse(str);
                        return true;
                    case "16":
                        number = Convert.ToUInt32(str, 16);
                        return true;
                }
            }
            catch { }
            number = 0;
            return false;
        }

        private void numberBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            uint temp;
            e.Handled = !ConvertToNumber(e.Text, out temp);
            base.OnPreviewTextInput(e);
        }

        private void insertButton_Click(object sender, RoutedEventArgs e)
        {
            uint number;
            if (ConvertToNumber(numberBox.Text, out number))
            {
                try
                {
                    CommandDetails commandDetails = new CommandDetails { UnicodeString = Convert.ToChar(number).ToString(), CommandType = Editor.CommandType.Text };
                    ((MainWindow)Application.Current.MainWindow).HandleToolBarCommand(commandDetails);
                }
                catch
                {
                    MessageBox.Show("The given value is invalid.", "Input error");
                }
            }
            else
            {
                MessageBox.Show("The entered value is invalid.", "Input error");
            }            
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        
        private void codeFormatComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            numberBase = (string)((ComboBoxItem)codeFormatComboBox.SelectedItem).Tag;
            if (!string.IsNullOrEmpty(numberBox.Text))
            {
                try
                {
                    uint number;
                    if (ConvertToNumber(numberBox.Text, out number))
                    {                        
                        numberBox.Text = Convert.ToString(number, int.Parse(numberBase));
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                catch
                {
                    MessageBox.Show("The number is not in correct format");
                    numberBox.Text = "";
                }
            }
        }
    }
}
