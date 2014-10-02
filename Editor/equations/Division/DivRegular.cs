using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media;

namespace Editor
{
    public class DivRegular : DivBase
    {        
        double ExtraWidth { get { return FontSize * .2; } }
        double ExtraHeight { get { return FontSize * .2; } }

        protected int barCount = 1;

        public DivRegular(EquationContainer parent)
            : base(parent, false)
        {
        }
        
        public DivRegular(EquationContainer parent, bool isSmall)
            : base(parent, isSmall)
        {            
        }

        public override void DrawEquation(DrawingContext dc)
        {
            base.DrawEquation(dc);                        
            if (barCount == 1)
            {
                dc.DrawLine(StandardPen, new Point(Left + ExtraWidth * .5, MidY), new Point(Right - ExtraWidth * .5, MidY));
            }
            else if (barCount == 2)
            {
                dc.DrawLine(StandardPen, new Point(Left + ExtraWidth * .5, MidY - ExtraHeight / 2), new Point(Right - ExtraWidth * .5, MidY - ExtraHeight/2));
                dc.DrawLine(StandardPen, new Point(Left + ExtraWidth * .5, MidY + ExtraHeight / 2), new Point(Right - ExtraWidth * .5, MidY + ExtraHeight/2));
            }
            if (barCount == 3)
            {
                dc.DrawLine(StandardPen, new Point(Left + ExtraWidth * .5, MidY), new Point(Right - ExtraWidth * .5, MidY));
                dc.DrawLine(StandardPen, new Point(Left + ExtraWidth * .5, MidY - ExtraHeight), new Point(Right - ExtraWidth * .5, MidY - ExtraHeight));
                dc.DrawLine(StandardPen, new Point(Left + ExtraWidth * .5, MidY + ExtraHeight), new Point(Right - ExtraWidth * .5, MidY + ExtraHeight));
            }
            //dc.DrawLine(new Pen(Brushes.Purple, 1), new Point(Left, MidY), new Point(Right, MidY));            
        }

        public override double Left
        {
            get { return base.Left; }
            set
            {
                base.Left = value;
                topEquation.MidX = this.MidX;
                bottomEquation.MidX = this.MidX;
            }
        }

        public override double Top
        {
            get { return base.Top; }
            set
            {
                base.Top = value;
                topEquation.Top = value;
                bottomEquation.Bottom = Bottom;
            }
        }

        protected override void CalculateWidth()
        {
            Width = Math.Max(topEquation.Width, bottomEquation.Width) + ExtraWidth;
        }

        protected override void CalculateHeight()
        {
            double height = topEquation.Height + bottomEquation.Height + ExtraHeight * 1.6;
            height += (barCount - 1) * ExtraHeight;             
            Height = height;
        }

        public override double RefY
        {
            get
            {
                return topEquation.Height + ExtraHeight * ((barCount + 1.0) / 2);
            }
        }
    }
}
