using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows;

namespace Editor
{
    public class DivSlanted : DivBase
    {
        double ExtraWidth { get { return FontSize * .1; } }
        double ExtraHeight { get { return FontSize * .1; } }

        double centerX;
        double slantXTop;
        double slantXBottom;

        public DivSlanted(EquationContainer parent)
            : base(parent, false)
        {
        }

        public DivSlanted(EquationContainer parent, bool isSmall)
            : base(parent, isSmall)
        {
        }

        public override void DrawEquation(DrawingContext dc)
        {
            base.DrawEquation(dc);
            dc.DrawLine(StandardPen, new Point(Left + centerX + slantXTop, Top), new Point(Left + centerX - slantXBottom, Bottom));            
        }

        public override double Left
        {
            get { return base.Left; }
            set
            {
                base.Left = value;
                topEquation.Right = Left + centerX - ExtraWidth / 2;
                bottomEquation.Left = Left + centerX + ExtraWidth / 2;
            }
        }
        public override double Top
        {
            get { return base.Top; }
            set
            {
                base.Top = value;
                topEquation.Top = this.Top;
                bottomEquation.Bottom = Bottom;
            }
        }

        public override void CalculateSize()
        {
            CalculateHeight();
            CalculateWidth();
        }

        protected override void CalculateWidth()
        {
            double width = topEquation.Width + bottomEquation.Width + ExtraWidth;
            Rect rect = new Rect(0, 0, width, Height);
            slantXTop = Math.Sin(Math.PI / 5) * (topEquation.Height + ExtraHeight/2);
            slantXBottom = Math.Sin(Math.PI / 5) * (bottomEquation.Height + ExtraHeight/2);
            rect.Union(new Point(topEquation.Width + slantXTop + ExtraWidth/2, Top));
            rect.Union(new Point(bottomEquation.Width + slantXBottom + ExtraWidth/2, Bottom));            
            Width = rect.Width;
            centerX = rect.Width - Math.Max(slantXTop, bottomEquation.Width) - ExtraWidth/2;
        }

        protected override void CalculateHeight()
        {
            Height = topEquation.Height + bottomEquation.Height + ExtraHeight;
        }

        //public override double RefY
        //{
        //    get
        //    {
        //        return topEquation.Height + ExtraHeight / 2;
        //    }
        //}
    }
}
