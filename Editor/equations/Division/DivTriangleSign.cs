﻿using System.Windows;
using System.Windows.Media;

namespace Editor
{
    public sealed class DivTriangleSign : EquationBase
    {
        public DivTriangleSign(EquationContainer parent)
            : base(parent)
        {
            IsStatic = true;
        }

        public override double Height
        {
            get
            {
                return base.Height;
            }
            set
            {
                base.Height = value;
                Width = FontSize * .25 + Height * .06;                
            }
        }

        public override double Bottom
        {
            get
            {
                return base.Bottom - StandardPen.Thickness / 2;
            }
            set
            {
                base.Bottom = value;
            }
        }

        public override void DrawEquation(DrawingContext dc)
        {
            dc.DrawPolyline(new Point(ParentEquation.Right, Bottom), new PointCollection{new Point(Left, Bottom), new Point(Right, Top)}, StandardRoundPen);
        }
    }
}
