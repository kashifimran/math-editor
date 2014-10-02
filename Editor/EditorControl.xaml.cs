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
using System.Globalization;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;

namespace Editor
{
    /// <summary>
    /// Interaction logic for EditorControl.xaml
    /// </summary>
    public partial class EditorControl : UserControl, IDisposable
    {
        System.Threading.Timer timer;
        int blinkPeriod = 600;

        bool _disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    timer.Dispose();
                }
                // Indicate that the instance has been disposed.
                _disposed = true;
            }
        }

        public event EventHandler ZoomChanged = (x, y) => { };

        bool showOverbar = true;

        public bool Dirty { get; set; }
        
        EquationRoot equationRoot;
        Caret vCaret = new Caret(false);
        Caret hCaret = new Caret(true);

        public static double rootFontBaseSize = 40;
        static double rootFontSize = rootFontBaseSize;
        double fontSize = rootFontBaseSize;

        public static double RootFontSize 
        {
            get { return rootFontSize; }
        }

        public EditorControl()
        {
            InitializeComponent();
            mainGrid.Children.Add(vCaret);
            mainGrid.Children.Add(hCaret);
            equationRoot = new EquationRoot(vCaret, hCaret);
            equationRoot.FontSize = fontSize;
            timer = new System.Threading.Timer(blinkCaret, null, blinkPeriod, blinkPeriod);
        }

        public void SetFontSizePercentage(int percentage)
        {            
            equationRoot.FontSize = fontSize * percentage / 100;
            rootFontSize = equationRoot.FontSize;
            AdjustView();
        }

        public void ShowOverbar(bool show)
        {
            showOverbar = show;
            if (!show)
            {
                hCaret.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                hCaret.Visibility = System.Windows.Visibility.Visible;
            }
        }

        void blinkCaret(Object state)
        {
            vCaret.ToggleVisibility();
            hCaret.ToggleVisibility();
        }

        public void HandleUserCommand(CommandDetails commandDetails)
        {
            equationRoot.HandleUserCommand(commandDetails);
            AdjustView();
            Dirty = true;
        }

        public void SaveFile(Stream stream, string fileName)
        {
            //equationRoot.SaveFile(stream);
            try
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    equationRoot.SaveFile(memoryStream);
                    memoryStream.Position = 0;
                    ZipStream(memoryStream, stream, System.IO.Path.GetFileNameWithoutExtension(fileName) + ".xml");
                }
            }
            catch
            {
                MessageBox.Show("Could not save file. Make sure the specified path is correct.", "Error");
            }
            Dirty = false;
        }

        public void ZipStream(MemoryStream memStreamIn, Stream outputStream, string zipEntryName)
        {
            ZipOutputStream zipStream = new ZipOutputStream(outputStream);
            zipStream.SetLevel(5); //0-9, 9 being the highest level of compression
            ZipEntry newEntry = new ZipEntry(zipEntryName);
            newEntry.DateTime = DateTime.Now;
            zipStream.PutNextEntry(newEntry);
            StreamUtils.Copy(memStreamIn, zipStream, new byte[4096]);
            zipStream.CloseEntry();
            zipStream.IsStreamOwner = false;	// False stops the Close also Closing the underlying stream.
            zipStream.Close();			// Must finish the ZipOutputStream before using outputMemStream.            
        }

        public void LoadFile(Stream stream)
        {
            //equationRoot.LoadFile(stream);
            try
            {
                ZipInputStream zipInputStream = new ZipInputStream(stream);
                ZipEntry zipEntry = zipInputStream.GetNextEntry();
                MemoryStream outputStream = new MemoryStream();
                if (zipEntry != null)
                {
                    byte[] buffer = new byte[4096];
                    StreamUtils.Copy(zipInputStream, outputStream, buffer);
                }
                outputStream.Position = 0;
                using (outputStream)
                {
                    equationRoot.LoadFile(outputStream);
                }
            }
            catch
            {
                stream.Position = 0;
                equationRoot.LoadFile(stream);
                //MessageBox.Show("Cannot open the specified file. The file is not in correct format.", "Error");
            }
            AdjustView();
            Dirty = false;
        }

        bool isDragging = false;

        private void EditorControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (equationRoot.ConsumeMouseClick(Mouse.GetPosition(this)))
            {
                InvalidateVisual();
            }
            this.Focus();
            lastMouseLocation = e.GetPosition(this);
            isDragging = true;
        }

        private void EditorControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
        }

        private void EditorControl_MouseEnter(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }

        private void EditorControl_MouseLeave(object sender, MouseEventArgs e)
        {
            StatusBarHelper.ShowCoordinates("");
        }

        Point lastMouseLocation = new Point();

        private void EditorControl_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePosition = e.GetPosition(this);
            StatusBarHelper.ShowCoordinates((int)mousePosition.X + ", " + (int)mousePosition.Y);
            if (isDragging)
            {
                if (Math.Abs(lastMouseLocation.X - mousePosition.X) > 2 /*SystemParameters.MinimumHorizontalDragDistance*/ ||
                    Math.Abs(lastMouseLocation.Y - mousePosition.Y) > 2 /*SystemParameters.MinimumVerticalDragDistance*/ )
                {
                    equationRoot.HandleMouseDrag(mousePosition);
                    lastMouseLocation = mousePosition;
                    InvalidateVisual();
                }
            }
        }

        public void DeleteSelection()
        {
            equationRoot.RemoveSelection(true);
            InvalidateVisual();
        }
        
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            //equationRoot.DrawEquation(drawingContext);
            ScrollViewer scrollViewer = Parent as ScrollViewer;
            equationRoot.DrawVisibleRows(drawingContext, scrollViewer.VerticalOffset, scrollViewer.ViewportHeight + scrollViewer.VerticalOffset);
        }

        public void EditorControl_TextInput(object sender, TextCompositionEventArgs e)
        {
            ConsumeText(e.Text.Replace('-', '\u2212'));
        }

        public void ConsumeText(string text)
        {
            equationRoot.ConsumeText(text);
            AdjustView();
            Dirty = true;
        }

        private void EditorControl_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void EditorControl_KeyDown(object sender, KeyEventArgs e)
        {
            bool handled = false;
            if (e.Key == Key.Tab)
            {
                equationRoot.ConsumeText("    ");
                handled = true;
            }
            else if (equationRoot.ConsumeKey(e.Key))
            {
                handled = true;
            }
            if (handled)
            {
                e.Handled = true;
                AdjustView();
                Dirty = true;
            }
        }

        void AdjustView()
        {
            DetermineSize();
            AdjustScrollViewer();
            this.InvalidateVisual();
        }

        void DetermineSize()
        {
            this.MinWidth = equationRoot.Width + 50;
            this.MinHeight = equationRoot.Height + 20;
        }

        void AdjustScrollViewer()
        {
            ScrollViewer scrollViewer = Parent as ScrollViewer;
            //Vector offsetPoint = VisualTreeHelper.GetOffset(this);           

            if (scrollViewer != null)
            {
                double left = scrollViewer.HorizontalOffset;
                double top = scrollViewer.VerticalOffset;
                double right = scrollViewer.ViewportWidth + scrollViewer.HorizontalOffset;
                double bottom = scrollViewer.ViewportHeight + scrollViewer.VerticalOffset;
                double hOffset = 0;
                double vOffset = 0;
                bool rightDone = false;
                bool bottomDone = false;
                while (vCaret.Left > right - 8)
                {
                    hOffset += 8;
                    right += 8;
                    rightDone = true;
                }
                while (vCaret.VerticalCaretBottom > bottom - 10)
                {
                    vOffset += 10;
                    bottom += 10;
                    bottomDone = true;
                }
                while (vCaret.Left < left + 8 && !rightDone)
                {
                    hOffset -= 8;
                    left -= 8;
                }
                while (vCaret.Top < top + 10 && !bottomDone)
                {
                    vOffset -= 10;
                    top -= 10;
                }
                scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset + hOffset);
                scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset + vOffset);
            }
        }

        public void Undo()
        {
            UndoManager.Undo();
            AdjustView();
            Dirty = true;
            equationRoot.AdjustCarets();
        }

        public void Redo()
        {
            UndoManager.Redo();
            AdjustView();
            Dirty = true;
            equationRoot.AdjustCarets();
        }

        public void ExportImage(string filePath)
        {
            equationRoot.SaveImageToFile(filePath);
            /*//clip 1
            DrawingVisual drawingVisual = new DrawingVisual();
            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                RenderTargetBitmap bmp = new RenderTargetBitmap(100, 100, 96, 96, PixelFormats.Default);
                bmp.Render(drawingVisual);

            }
            //clip 2
            var image = Clipboard.GetImage();
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image));
                encoder.Save(fileStream);
            }

            //clip 3

            Rect rect = new Rect(this.RenderSize);
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)rect.Right,
              (int)rect.Bottom, 96d, 96d, System.Windows.Media.PixelFormats.Default);
            rtb.Render(this);
            //endcode as PNG
            BitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(rtb));

            //save to memory stream
            System.IO.MemoryStream ms = new System.IO.MemoryStream();

            pngEncoder.Save(ms);
            ms.Close();
            System.IO.File.WriteAllBytes("logo.png", ms.ToArray());
            Console.WriteLine("Done");
            */
        }

        public void ZoomOut()
        {
            ZoomChanged(this, EventArgs.Empty);
            equationRoot.ZoomOut(4);
            rootFontSize = equationRoot.FontSize;
            AdjustView();
        }

        public void ZoomIn()
        {
            ZoomChanged(this, EventArgs.Empty);
            equationRoot.ZoomIn(4);
            rootFontSize = equationRoot.FontSize;
            AdjustView();
        }

        private void ZoomOutHandler(object sender, ExecutedRoutedEventArgs e)
        {
            ZoomOut();
        }

        private void ZoomInHandler(object sender, ExecutedRoutedEventArgs e)
        {
            ZoomIn();
        }

        public void Copy(bool cut)
        {
            equationRoot.Copy(cut);
            if (cut)
            {
                AdjustView();
            }
        }

        public void Paste()
        {
            if (equationRoot.PasteFromClipBoard())
            {
                AdjustView();
                Dirty = true;
            }
        }

        public void SelectAll()
        {
            equationRoot.SelectAll();
            InvalidateVisual();
        }

        public void ChangeFont(FontType fontType)
        {
            equationRoot.ChangeFont(fontType);
            InvalidateVisual();
        }

        public void ChangeFormat(string operation, string argument, bool applied)
        {
            equationRoot.ModifySelection(operation, argument, applied, true);
            AdjustView();
            Dirty = true;
        }

        public void Clear()
        {
            equationRoot = new EquationRoot(vCaret, hCaret);
            equationRoot.FontSize = fontSize;
            rootFontSize = fontSize;
            Dirty = false;
            AdjustView();
        }
    }
}
