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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Editor
{
    /// <summary>
    /// Interaction logic for ButtonPanel.xaml
    /// </summary>
    public partial class ButtonPanel : UserControl
    {
        public event EventHandler ButtonClick = (x, y) => { };

        List<CommandDetails> commandDetails;
        public ButtonPanel(List<CommandDetails> listCommandDetails, int columns, int buttonMargin)
        {
            InitializeComponent();
            commandDetails = listCommandDetails;
            mainGrid.Columns = columns;//listButtonDetails.Count < 5 ? listButtonDetails.Count : 5;
            mainGrid.Rows = (int)Math.Ceiling(listCommandDetails.Count / (double)mainGrid.Columns);
            mainGrid.Width = 30 * mainGrid.Columns;
            mainGrid.Height = 30 * mainGrid.Rows;

            for (int i = 0; i < commandDetails.Count; i++)
            {
                EditorToolBarButton b = new EditorToolBarButton(commandDetails[i]);
                b.Margin = new Thickness(buttonMargin);
                b.Click += new RoutedEventHandler(panelButton_Click);
                b.Style = (Style)FindResource("MathToolBarButtonStyle");
                b.SetValue(Grid.ColumnProperty, i % mainGrid.Columns);
                b.SetValue(Grid.RowProperty, i / mainGrid.Columns);
                b.FontFamily = FontFactory.GetFontFamily(FontType.STIXGeneral);
                //b.FontSize = 10;
                if (commandDetails[i].Image != null)
                {
                    b.Content = commandDetails[i].Image;
                }
                else
                {
                    b.Content = commandDetails[i].UnicodeString;
                }
                mainGrid.Children.Add(b);
                if (commandDetails[i].CommandType == CommandType.None) //This is an ugly kludge!
                {
                    b.Visibility = System.Windows.Visibility.Hidden;
                }
            }
        }

        void panelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = System.Windows.Visibility.Collapsed;
            ButtonClick(this, EventArgs.Empty);
        }
    }

    public class EditorToolBarButton : Button
    {
        CommandDetails commandDetails = null;

        public EditorToolBarButton(CommandDetails commandDetails)
        {
            this.commandDetails = commandDetails;
        }

        protected override void OnClick()
        {
            base.OnClick();
            ((MainWindow)Application.Current.MainWindow).HandleToolBarCommand(commandDetails);
        }
    }
}
