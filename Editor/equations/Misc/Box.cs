using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Windows;
using System.Windows.Media;

namespace Editor
{
    public class Box : EquationContainer
    {
        BoxType boxType;
        RowContainer insideEq = null;
        double paddingFactor = 0.2;
        double Padding { get { return FontSize * paddingFactor;} }
        double TopPadding
        {
            get
            {
                if (boxType == BoxType.All || boxType == BoxType.LeftTop || boxType == BoxType.RightTop)
                {
                    return 0; // paddingFactor* FontSize;
                }
                else
                {
                    return 0;
                }
            }
        }
        double BottomPadding
        {
            get
            {
                if (boxType == BoxType.All || boxType == BoxType.LeftBottom || boxType == BoxType.RightBottom)
                {
                    return paddingFactor * FontSize;
                }
                else
                {
                    return 0;
                }
            }
        }
        double LeftPadding
        {
            get
            {
                if (boxType == BoxType.All || boxType == BoxType.LeftTop || boxType == BoxType.LeftBottom)
                {
                    return paddingFactor * FontSize;
                }
                else
                {
                    return 0;
                }
            }
        }
        double RightPadding
        {
            get
            {
                if (boxType == BoxType.All || boxType == BoxType.RightTop || boxType == BoxType.RightBottom)
                {
                    return paddingFactor * FontSize;
                }
                else
                {
                    return 0;
                }
            }
        }

        Point LeftTop { get { return new Point(Left + LeftPadding / 2, Top + TopPadding / 2); } }
        Point RightTop { get { return new Point(Right - RightPadding / 2, Top + TopPadding / 2); } }
        Point LeftBottom { get { return new Point(Left + LeftPadding / 2, Bottom - BottomPadding / 2); } }
        Point RightBottom { get { return new Point(Right - RightPadding / 2, Bottom - BottomPadding / 2); } }

        public Box(EquationContainer parent, BoxType boxType)
            :base (parent)
        {
            this.boxType = boxType;
            ActiveChild = insideEq = new RowContainer(this);
            childEquations.Add(insideEq);
        }

        public override XElement Serialize()
        {
            XElement thisElement = new XElement(GetType().Name);
            XElement parameters = new XElement("parameters");
            parameters.Add(new XElement(boxType.GetType().Name, boxType));
            thisElement.Add(parameters);
            thisElement.Add(insideEq.Serialize());
            return thisElement;
        }

        public override void DeSerialize(XElement xElement)
        {
            insideEq.DeSerialize(xElement.Element(insideEq.GetType().Name));
            CalculateSize();
        }

        public override void DrawEquation(System.Windows.Media.DrawingContext dc)
        {
            base.DrawEquation(dc);
            switch (boxType)
            {
                case BoxType.All:
                    dc.DrawPolyline(LeftTop, new PointCollection{RightTop, RightBottom, LeftBottom, LeftTop, RightTop}, StandardMiterPen);
                    break;
                case BoxType.LeftBottom:
                    dc.DrawPolyline(LeftTop, new PointCollection { LeftBottom, RightBottom }, StandardMiterPen);
                    break;
                case BoxType.LeftTop:
                    dc.DrawPolyline(RightTop, new PointCollection { LeftTop, LeftBottom }, StandardMiterPen);
                    break;
                case BoxType.RightBottom:
                    dc.DrawPolyline(RightTop, new PointCollection { RightBottom, LeftBottom }, StandardMiterPen);
                    break;
                case BoxType.RightTop:
                    dc.DrawPolyline(LeftTop, new PointCollection { RightTop, RightBottom }, StandardMiterPen);
                    break;
            }
        }

        public override double Top
        {
            get { return base.Top; }
            set
            {
                base.Top = value;
                insideEq.Top = value + TopPadding;        
            }
        }

        public override double Left
        {
            get { return base.Left; }
            set
            {
                base.Left = value;
                insideEq.Left = value + LeftPadding;
            }
        }

        protected override void CalculateWidth()
        {
            Width = insideEq.Width + LeftPadding + RightPadding; 
        }

        protected override void CalculateHeight()
        {
            Height = insideEq.Height + TopPadding + BottomPadding;            
        }

        public override double RefY
        {
            get
            {
                return insideEq.RefY + TopPadding;
            }
        }
    }
}
