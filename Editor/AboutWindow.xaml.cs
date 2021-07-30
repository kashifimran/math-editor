using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Editor
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
            versionLabel.Content = "Math Editor v." + Assembly.GetEntryAssembly().GetName().Version.ToString();
        }

        private void LinkTextBlock_MouseUp(object sender, MouseButtonEventArgs e)
        {
            BrowserHelper.Open(((TextBlock)sender).Text);
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
