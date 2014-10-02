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
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace Editor
{
    /// <summary>
    /// Interaction logic for UnicodeSelectorWindow.xaml
    /// </summary>
    public partial class UnicodeSelectorWindow : Window
    {
        Dictionary<string, ObservableCollection<UnicodeListItem>> categories = new Dictionary<string, ObservableCollection<UnicodeListItem>>();
        //Dictionary<int, Dictionary<string, ObservableCollection<UnicodeListItem>>> fontCache = new Dictionary<int, Dictionary<string, ObservableCollection<UnicodeListItem>>>();
        ObservableCollection<UnicodeListItem> recentList = new ObservableCollection<UnicodeListItem>();
        ObservableCollection<UnicodeListItem> allList = new ObservableCollection<UnicodeListItem>();
        
        public UnicodeSelectorWindow()
        {
            InitializeComponent();
            DataContext = this;
            symbolListBox.FontFamily = FontFactory.GetFontFamily(FontType.STIXGeneral);
            symbolListBox.FontSize = 18;
            recentListBox.FontFamily = FontFactory.GetFontFamily(FontType.STIXGeneral);
            recentListBox.FontSize = 16;
            //List<string> names = Enum.GetNames(typeof(FontType)).ToList();
            //names.Sort();
            recentListBox.ItemsSource = recentList;            
            //fontBox.ItemsSource = names;
            //fontBox.SelectedIndex = 0;
            SetupCategories();
        }

        private void SetupCategories()
        {
            SetupCategory("Mathematical Operators & Number Forms", 0x2150, 0x218F);
            SetupCategory("Mathematical Operators & Number Forms", 0x2200, 0x22FF); //Mathematical Operators    
            SetupCategory("Miscellaneous Mathematical", 0x27C0, 0x27EF); // Symbols-A
            SetupCategory("Basic Latin", 0x021, 0x7E);
            SetupCategory("Latin-1 Supplement", 0x0A1, 0xAC);
            SetupCategory("Latin-1 Supplement", 0x0AE, 0xFF);
            SetupCategory("Latin Extended", 0x100, 0x17F); //A
            SetupCategory("Latin Extended", 0x180, 0x237); //B
            SetupCategory("Punctuation & Diacritical Marks", 0x2000, 0x206F); //General Punctuation
            SetupCategory("Punctuation & Diacritical Marks", 0x2B0, 0x2FF); //Spacing Modifier Letters
            SetupCategory("Punctuation & Diacritical Marks", 0x300, 0x36F); //Combining Diacritical Marks
            SetupCategory("Greek and Coptic", 0x370, 0x3FF);
            SetupCategory("Cyrillic", 0x400, 0x4FF);
            SetupCategory("Currency Symbols & Phonetic Extensions", 0x20A0, 0x20CF); //Currency Symbols
            SetupCategory("Currency Symbols & Phonetic Extensions", 0x1D00, 0x1D7F); //Phonetic Extensions
            SetupCategory("Currency Symbols & Phonetic Extensions", 0x1D80, 0x1DBF); //Phonetic Extensions Supplement
            SetupCategory("Latin Extended", 0x1E00, 0x1EFF); //Latin Extended Additional
            SetupCategory("Punctuation & Diacritical Marks", 0x20D0, 0x20FF); //Combining Diacritical Marks for Symbols
            SetupCategory("Letterlike Symbols", 0x2100, 0x214F);            
            SetupCategory("Arrows", 0x2190, 0x21FF);            
            SetupCategory("Miscellaneous", 0x2300, 0x23FF); //Miscellaneous Technical
            SetupCategory("Miscellaneous", 0x2400, 0x243F); //Control Pictures
            SetupCategory("Enclosed Alphanumerics", 0x2460, 0x24FF);
            SetupCategory("Shapes", 0x2700, 0x27BF); //Dingbats 
            SetupCategory("Shapes", 0x2500, 0x257F); //Box Drawing
            SetupCategory("Shapes", 0x25A0, 0x25FF); //Geometric Shapes
            SetupCategory("Miscellaneous", 0x2600, 0x26FF); //Miscellaneous Symbols
            SetupCategory("Arrows", 0x27F0, 0x27FF);
            SetupCategory("Arrows", 0x2900, 0x297F);
            SetupCategory("Miscellaneous Mathematical", 0x2980, 0x29FF); // Symbols-B
            SetupCategory("Supplemental Mathematical Operators", 0x2A00, 0x2AFF);
            SetupCategory("Miscellaneous", 0x2B12, 0x2B54); //Miscellaneous Symbols and Arrows
            SetupCategory("Miscellaneous", 0xFB00, 0xFB4F); //Alphabetic Presentation Forms
            //SetupCategory("Mathematical Alphanumeric Symbols", 0x1D400, 0x1D7FF);            

            categories.Add("All", allList);
            categoryBox.ItemsSource = categories.Keys;
            categoryBox.SelectedIndex = 0;            
        }

        private void SetupCategory(string categoryName, int start, int end)
        {
            ObservableCollection<UnicodeListItem> list = new ObservableCollection<UnicodeListItem>();
            FontFamily family = FontFactory.GetFontFamily(FontType.STIXGeneral);
            for (int i = start; i <= end; i++)
            {
                if (TypefaceContainsCharacter(FontFactory.GetTypeface(FontType.STIXGeneral, FontStyles.Normal, FontWeights.Normal), Convert.ToChar(i)))
                {
                    UnicodeListItem item = new UnicodeListItem { /*FontFamily = family, HexString = "0x" + i.ToString("X4"),*/ CodePoint = i, UnicodeText = string.Format("{0}", Convert.ToChar(i)) };
                    list.Add(item);
                    allList.Add(item);
                }
            }
            if (categories.Keys.Contains(categoryName))
            {
                ObservableCollection<UnicodeListItem> oldList = categories[categoryName];
                foreach (UnicodeListItem item in list)
                {
                    oldList.Add(item);
                }
            }
            else
            {
                categories.Add(categoryName, list);
            }
        }

        private static bool TypefaceContainsCharacter(Typeface typeface, char characterToCheck)
        {   
            ushort glyphIndex;
            int unicodeValue = Convert.ToUInt16(characterToCheck);
            GlyphTypeface glyph;
            typeface.TryGetGlyphTypeface(out glyph);
            if (glyph != null && glyph.CharacterToGlyphMap.TryGetValue(unicodeValue, out glyphIndex))
            {
                return true;
            }
            return false;
        }

        private void fontBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void categoryBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {            
            symbolListBox.ItemsSource = categories[(string)categoryBox.SelectedValue];
            symbolListBox.SelectedIndex = 0;
        }

        bool useRecentList = false;
        private void insertButton_Click(object sender, RoutedEventArgs e)
        {
            InsertSymbol();
        }

        void InsertSymbol()
        {
            UnicodeListItem item = null;
            if (useRecentList)
            {
                item = recentListBox.SelectedItem as UnicodeListItem;
            }
            else
            {
                item = symbolListBox.SelectedItem as UnicodeListItem;
            }
            if (item != null)
            {
                CommandDetails commandDetails = new CommandDetails { UnicodeString = item.UnicodeText, CommandType = Editor.CommandType.Text };
                ((MainWindow)Application.Current.MainWindow).HandleToolBarCommand(commandDetails);
                if (!useRecentList)
                {
                    if (recentList.Contains(item))
                    {
                        recentList.Remove(item);
                    }
                    recentList.Insert(0, item);
                    if (recentList.Count > 20)
                    {
                        recentList.RemoveAt(recentList.Count - 1);
                    }
                }
            }
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void symbolList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UnicodeListItem item = symbolListBox.SelectedItem as UnicodeListItem;
            characterCodeChanged(item);
        }

        private void recentListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UnicodeListItem item = recentListBox.SelectedItem as UnicodeListItem;
            characterCodeChanged(item);
        }

        private void characterCodeChanged(UnicodeListItem item)
        {
            if (item != null)
            {
                int numberBase = int.Parse((string)((ComboBoxItem)codeFormatComboBox.SelectedItem).Tag);
                string numberString = Convert.ToString(item.CodePoint, numberBase);
                if (numberBase == 16)
                {
                    numberString = numberString.ToUpper().PadLeft(4, '0');
                }
                else if (numberBase == 8)
                {
                    numberString = numberString.PadLeft(6, '0');
                }
                characterCode.Text = numberString;
            }
        }

        private void codeFormatComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UnicodeListItem item = symbolListBox.SelectedItem as UnicodeListItem;
            if (item != null)
            {
                int numberBase = int.Parse((string)((ComboBoxItem)codeFormatComboBox.SelectedItem).Tag);
                string numberString = Convert.ToString(item.CodePoint, numberBase);
                if (numberBase == 16)
                {
                    numberString = numberString.ToUpper().PadLeft(4, '0');
                }
                else if (numberBase == 8)
                {
                    numberString = numberString.PadLeft(6, '0');
                }
                characterCode.Text = numberString;
            }
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (recentListBox.IsMouseOver)
            {
                useRecentList = true;
            }
            else if (symbolListBox.IsMouseOver)
            {
                useRecentList = false;
            }
        }

        

        private void characterListBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (NativeMethods.doubleClickTimer.Enabled)
            {
                InsertSymbol();
                NativeMethods.doubleClickTimer.Enabled = false;
            }
            else
            {
                NativeMethods.doubleClickTimer.Enabled = true;
            }            
        }
    }

    public class UnicodeListItem
    {        
        public int CodePoint { get; set; }
        public string UnicodeText { get; set; }
    }

    public static class NativeMethods
    {
        [DllImport("user32.dll")]
        static extern uint GetDoubleClickTime();//for emulating double-click events on elements that don't support it
        internal static System.Timers.Timer doubleClickTimer = new System.Timers.Timer((int)GetDoubleClickTime()) { AutoReset = false };
    }
}
