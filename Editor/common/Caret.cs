using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Editor
{
    public sealed class Caret : FrameworkElement, IDisposable
    {
        private Point location;
        public double CaretLength { get; set; } = 18d;

        private readonly bool _isHorizontal = false;

        public static readonly DependencyProperty VisibleProperty = DependencyProperty.Register("Visible", typeof(bool), typeof(Caret), new FrameworkPropertyMetadata(false /* defaultValue */, FrameworkPropertyMetadataOptions.AffectsRender));

        public Caret(bool isHorizontal)
        {
            _isHorizontal = isHorizontal;
            Visible = true;
        }

        protected override void OnRender(DrawingContext dc)
        {
            if (Visible)
            {
                dc.DrawLine(PenManager.GetPen(Math.Max(1, EditorControl.RootFontSize * .8 / EditorControl.rootFontBaseSize)), Location, OtherPoint);
            }
            else if (_isHorizontal)
            {
                dc.DrawLine(PenManager.GetWhitePen(Math.Max(1, EditorControl.RootFontSize *.8 / EditorControl.rootFontBaseSize)), Location, OtherPoint);
            }
        }

        private Point OtherPoint => _isHorizontal ? new Point(Left + CaretLength, Top) : new Point(Left, VerticalCaretBottom);

        public void ToggleVisibility()
        {
            if (!_isDisposed)
            {
                Dispatcher.Invoke(new Action(() => { Visible = !Visible; }));
            }
        }

        private bool Visible
        {
            get => (bool)GetValue(VisibleProperty);
            set
            {
                try
                {
                    SetValue(VisibleProperty, value);
                }
                catch (TaskCanceledException)
                {
                    // when the object got disposed, the SetValue operation will fail
                }
            }
        }

        public Point Location
        {
            get => location;
            set => location = new Point(Math.Floor(value.X) + .5, Math.Floor(value.Y) + .5);
        }

        public double Left
        {
            get => location.X;
            set => location = new Point(Math.Floor(value) + .5, location.Y);
        }

        public double Top
        {
            get => location.Y;
            set => location = new Point(location.X, Math.Floor(value) + .5);
        }

        public double VerticalCaretBottom => location.Y + CaretLength;

        #region IDisposable
        private bool _isDisposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(true);
        }

        ~Caret()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            _isDisposed = true;
        }
        #endregion
    }
}
