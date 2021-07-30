using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Editor
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            try
            {
                //EditorMode mode = (EditorMode)Enum.Parse(typeof(EditorMode), ConfigManager.GetConfigurationValue(KeyName.default_mode));
                //FontType font = (FontType)Enum.Parse(typeof(FontType), ConfigManager.GetConfigurationValue(KeyName.default_font));
                var mode = ConfigManager.GetConfigurationValue(KeyName.default_mode);
                var fontName = ConfigManager.GetConfigurationValue(KeyName.default_font);

                var modes = editorModeCombo.Items;
                foreach (ComboBoxItem item in modes)
                {
                    if ((string)item.Tag == mode)
                    {
                        editorModeCombo.SelectedItem = item;
                    }
                }
                var fonts = equationFontCombo.Items;
                foreach (ComboBoxItem item in fonts)
                {
                    if ((string)item.Tag == fontName)
                    {
                        equationFontCombo.SelectedItem = item;
                    }
                }
            }
            catch { }
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            ConfigManager.SetConfigurationValues(new Dictionary<KeyName, string>() {
                {KeyName.default_mode, ((ComboBoxItem)editorModeCombo.SelectedItem).Tag.ToString()},
                {KeyName.default_font, ((ComboBoxItem)equationFontCombo.SelectedItem).Tag.ToString()}
            });
            this.Close();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
