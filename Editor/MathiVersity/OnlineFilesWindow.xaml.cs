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
using Editor.MVStorage;
using System.Collections.ObjectModel;

namespace Editor
{
    /// <summary>
    /// Interaction logic for OnlineFilesWindow.xaml
    /// </summary>
    public partial class OnlineFilesWindow : Window
    {
        MainWindow window = null;
        ObservableCollection<StorageFile> files = new ObservableCollection<StorageFile>();

        public OnlineFilesWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            this.window = mainWindow;
            filesListView.ItemsSource = files;
        }


        private void openFileButton_Click(object sender, RoutedEventArgs e)
        {
            StorageFile sf = (sender as Control).DataContext as StorageFile;
            if (sf != null)
            {
                window.OpenOnlineFile(sf);
                this.Hide();
            }
        }

        private void deleteFileButton_Click(object sender, RoutedEventArgs e)
        {
            StorageFile sf = (sender as Control).DataContext as StorageFile;
            if (sf != null)
            {
                if (MessageBox.Show("Are you sure you want to delete this file.\r\n" + sf.Title, "Please confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    try
                    {
                        MVService.DeleteFile(sf.ID);
                        files.Remove(sf);
                    }
                    catch (Exception exp)
                    {
                        MessageBox.Show(exp.Message, "Error");
                    }
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {            
            //RefreshFileList();
        }
        
        public void RefreshFileList()
        {
            this.Cursor = Cursors.Wait;
            var onlineFiles = MVService.GetFiles();
            files.Clear();
            foreach (var file in onlineFiles)
            {
                files.Add(file);
            }            
            this.Cursor = Cursors.Arrow;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsVisible)
            {
                RefreshFileList();
            }
        }
    }
}
