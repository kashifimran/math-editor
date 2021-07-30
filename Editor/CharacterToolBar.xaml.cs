using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Editor
{
    /// <summary>
    /// Interaction logic for MathToolbar.xaml
    /// </summary>
    public partial class CharacterToolBar : UserControl
    {
        public event EventHandler CommandCompleted = (x, y) => { };
        Dictionary<object, ButtonPanel> buttonPanelMapping = new Dictionary<object, ButtonPanel>();
        ButtonPanel visiblePanel = null;

        public CharacterToolBar()
        {
            InitializeComponent();
        }

        private void toolBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (visiblePanel != null)
            {
                visiblePanel.Visibility = System.Windows.Visibility.Collapsed;
            }
            if (buttonPanelMapping[sender].Visibility != System.Windows.Visibility.Visible)
            {
                buttonPanelMapping[sender].Visibility = System.Windows.Visibility.Visible;
                visiblePanel = buttonPanelMapping[sender];
            }
        }

        public void HideVisiblePanel()
        {
            if (visiblePanel != null)
            {
                visiblePanel.Visibility = System.Windows.Visibility.Collapsed;
                visiblePanel = null;
            }
        }

        private void toolBarButton_MouseEnter(object sender, MouseEventArgs e)
        {
            ChangeActivePanel(sender);
        }


        private void toolBarButton_GotFocus(object sender, RoutedEventArgs e)
        {
            ChangeActivePanel(sender);
        }

        void ChangeActivePanel(object sender)
        {
            if (visiblePanel != null)
            {
                visiblePanel.Visibility = System.Windows.Visibility.Collapsed;
                buttonPanelMapping[sender].Visibility = System.Windows.Visibility.Visible;
                visiblePanel = buttonPanelMapping[sender];
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            CreateSymbolsPanel();
            CreateGreekCapitalPanel();
            CreateGreekSmallPanel();
            CreateArrowsPanel();            
        }

        void CreatePanel(List<CommandDetails> list, Button toolBarButton, int columns, int margin)
        {
            ButtonPanel bp = new ButtonPanel(list, columns, margin);
            bp.ButtonClick += (x, y) => { CommandCompleted(this, EventArgs.Empty); visiblePanel = null; };
            mainToolBarPanel.Children.Add(bp);
            Canvas.SetTop(bp, mainToolBarPanel.Height);
            Vector offset = VisualTreeHelper.GetOffset(toolBarButton);
            Canvas.SetLeft(bp, offset.X + 2);
            bp.Visibility = System.Windows.Visibility.Collapsed;
            buttonPanelMapping.Add(toolBarButton, bp);
        }


        void CreateTextPanel(string[] items, Button toolBarButton, int columns)
        {
            List<CommandDetails> list = new List<CommandDetails>();
            foreach (string s in items)
            {
                list.Add(new CommandDetails { UnicodeString = s, CommandType = CommandType.Text });
            }
            CreatePanel(list, toolBarButton, columns, 0);
        }

        void CreateSymbolsPanel()
        {
            string[] items = {  "\u00d7", "-", "\u2013", "\u2012", "\u2014", "\u00b7", "\u00f7", "\u00b1", 
                             "\u00bd", "\u00bc", "\u00be", "\u2200", "\u2202", "\u2203", "\u2204", 
                             "\u2205", "\u2208", "\u2209", "\u220B", "\u220C", "\u220F", "\u2210", 
                             "\u2211", "\u2217", "\u221A", "\u221D", "\u221E", "\u2227", "\u2228", 
                             "\u2229", "\u222A", "\u2234", "\u2235", "\u2237", "\u2238", "\u2264", 
                             "\u2265", "\u226e", "\u226f", "\u25B3", "\u25B7", "\u25BD", "\u25c8", 
                             "\u25C9", "\u25CE", "\u25E0", "\u25E1", "\u25EC" };

            CreateTextPanel(items, symbolsButton, 4);
        }

        void CreateGreekCapitalPanel()
        {
            string[] items = { "\u0391", "\u0392", "\u0393", "\u0394", "\u0395", "\u0396", "\u0397", 
                             "\u0398", "\u0399", "\u039A", "\u039B", "\u039C", "\u039D", "\u039E",
                             "\u039F", "\u03A0", "\u03A1", "\u03A3", "\u03A4", "\u03A5", "\u03A6",
                             "\u03A7", "\u03A8", "\u03A9" };

            CreateTextPanel(items, greekCapitalButton, 4);
        }

        void CreateGreekSmallPanel()
        {
            string[] items = { "\u03B1", "\u03B2", "\u03B3", "\u03B4", "\u03B5", "\u03B6", "\u03B7",
                             "\u03B8", "\u03B9", "\u03BA", "\u03BB", "\u03BC", "\u03BD", "\u03BE", 
                             "\u03BF", "\u03C0", "\u03C1", "\u03C2", "\u03C3", "\u03C4", "\u03C5", 
                             "\u03C6", "\u03C7", "\u03C8", "\u03C9" };

            CreateTextPanel(items, greekSmallButton, 4);
        }

        void CreateArrowsPanel()
        {
            string[] items = {
                                 "\u2190", "\u2191", "\u2192", "\u2193", "\u2194", "\u2195", "\u2196", 
                                 "\u2197", "\u2198", "\u2199", "\u219A", "\u219B", "\u219C", "\u219D", 
                                 "\u219E", "\u219F", "\u21A0", "\u21A1", "\u21A2", "\u21A3", "\u21A4", 
                                 "\u21A5", "\u21A6", "\u21A7", "\u21A8", "\u21A9", "\u21AA", "\u21AB", 
                                 "\u21AC", "\u21AD", "\u21AE", "\u21AF", "\u21B0", "\u21B1", "\u21B2",
                                 "\u21B3", "\u21B4", "\u21B5", "\u21B6", "\u21B7", "\u21B8", "\u21B9",
                                 "\u21BA", "\u21BB", "\u21BC", "\u21BD", "\u21BE", "\u21BF", "\u21C0",
                                 "\u21C1", "\u21C2", "\u21C3", "\u21C4", "\u21C5", "\u21C6", "\u21C7",
                                 "\u21C8", "\u21C9", "\u21CA", "\u21CB", "\u21CC", "\u21CD", "\u21CE", 
                                 "\u21CF", "\u21D0", "\u21D1", "\u21D2", "\u21D3", "\u21D4", "\u21D5", 
                                 "\u21D6", "\u21D7", "\u21D8", "\u21D8", "\u21D9", "\u21DA", "\u21DB",
                                 "\u21DC",
                             };

            CreateTextPanel(items, arrowsButton, 6);
        } 
    }
}
