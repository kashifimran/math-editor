using System.Windows;

namespace Editor
{
    /// <summary>
    /// Represents the coordinates of the box that encapsulates an equation
    /// </summary>
    public abstract class EquationBox
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

        private Point location = new(0d, 0d);
        public Point Location
        {
            get => location;
            set
            {
                Left = value.X;
                Top = value.Y;
            }
        }

        public virtual double Left
        {
            get => location.X;
            set => location.X = value;
        }

        public virtual double Top
        {
            get => location.Y;
            set => location.Y = value;
        }

        public virtual double RefX => width / 2d;
        public virtual double RefY => height / 2d;
        public double RefYReverse => height - RefY;

        public double MidX
        {
            get => location.X + RefX;
            set => Left = value - RefX;
        }

        public double MidY
        {
            get => location.Y + RefY;
            set => Top = value - RefY;
        }

        public virtual double Right
        {
            get => location.X + width;
            set => Left = value - width;
        }

        public virtual double Bottom
        {
            get => location.Y + height;
            set => Top = value - height;
        }

        public Size Size => new(width, height);
        public Rect Bounds => new(location, Size);
    }
}
