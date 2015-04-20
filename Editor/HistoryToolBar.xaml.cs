using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for HistoryToolBar.xaml
    /// </summary>
    public partial class HistoryToolBar : UserControl
    {
        int maxSymbols = 30;
        ObservableCollection<string> recentList = new ObservableCollection<string>();
        Dictionary<string, int> usedCount = new Dictionary<string, int>();

        public HistoryToolBar()
        {
            this.DataContext = this;
            InitializeComponent();
            recentListBox.ItemsSource = recentList;
            var data = ConfigManager.GetConfigurationValue(KeyName.symbols);
            if (data.Length > 0)
            {
                string[] list = data.Split(',');
                foreach (var s in list)
                {
                    recentList.Add(s);
                    usedCount.Add(s, 0);
                }
            }
            recentListBox.FontFamily = FontFactory.GetFontFamily(FontType.STIXGeneral);
        }

        public void AddItem(string symbol)
        {
            if (usedCount.ContainsKey(symbol))
            {
                usedCount[symbol] += 1;
            }
            else
            {
                if (usedCount.Count >= maxSymbols)
                {
                    int min = int.MaxValue;
                    string s = usedCount.First().Key;
                    foreach (var pair in usedCount)
                    {
                        if (pair.Value < min)
                        {
                            min = pair.Value;
                            s = pair.Key;
                        }
                    }
                    recentList.Remove(s);
                    usedCount.Remove(s);
                }
                recentList.Insert(0, symbol);
                usedCount.Add(symbol, 1);
            }            
        }

        private void symbolClick(object sender, MouseButtonEventArgs e)
        {
            string item = ((TextBlock)sender).DataContext as string;
            CommandDetails commandDetails = new CommandDetails { UnicodeString = item, CommandType = Editor.CommandType.Text };
            ((MainWindow)Application.Current.MainWindow).HandleToolBarCommand(commandDetails);
        }

        public void Save()
        {
            string data = "";
            foreach (var s in recentList)
            {
                data += s + ",";
            }
            data = data.Trim(',');
            ConfigManager.SetConfigurationValue(KeyName.symbols, data);
        }
    }
}
