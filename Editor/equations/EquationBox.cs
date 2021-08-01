using JetBrains.Annotations;
using PropertyChanged;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Editor
{
    /// <summary>
    /// Represents the coordinates of the box that encapsulates an equation
    /// </summary>
    public abstract class EquationBox : INotifyPropertyChanged
    {
        private static Thickness ZeroMargin = new(0d);
        public virtual Thickness Margin => ZeroMargin;

        private double width;
        public virtual double Width
        {
            get => width;
            set => width = value > 0d ? value : 0d;
        }

        private double height;
        public virtual double Height
        {
            get => height;
            set => height = value > 0d ? value : 0d;
        }

        public Point Location { get; set; } = new(0d, 0d);

        [DependsOn(nameof(Location))]
        public virtual double Left
        {
            get => Location.X;
            set => Location = new Point(value, Location.Y);
        }

        [DependsOn(nameof(Location))]
        public virtual double Top
        {
            get => Location.Y;
            set => Location = new Point(Location.X, value);
        }

        [DependsOn(nameof(Width))]
        public virtual double RefX => Width / 2d;

        [DependsOn(nameof(Height))]
        public virtual double RefY => Height / 2d;

        [DependsOn(nameof(Height), nameof(RefY))]
        public double RefYReverse => Height - RefY;

        [DependsOn(nameof(Location), nameof(RefX))]
        public double MidX
        {
            get => Location.X + RefX;
            set => Left = value - RefX;
        }

        [DependsOn(nameof(Location), nameof(RefY))]
        public double MidY
        {
            get => Location.Y + RefY;
            set => Top = value - RefY;
        }

        [DependsOn(nameof(Location), nameof(Width))]
        public virtual double Right
        {
            get => Location.X + width;
            set => Left = value - width;
        }

        [DependsOn(nameof(Location), nameof(Height))]
        public virtual double Bottom
        {
            get => Location.Y + Height;
            set => Top = value - Height;
        }

        [DependsOn(nameof(Width), nameof(Height))]
        public Size Size => new(Width, Height);

        [DependsOn(nameof(Location), nameof(Size))]
        public Rect Bounds => new(Location, Size);

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "") => 
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #endregion
    }
}
