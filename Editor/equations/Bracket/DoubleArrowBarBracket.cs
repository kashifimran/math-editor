using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Windows.Media;
using System.Windows;

namespace Editor
{
    public class DoubleArrowBarBracket : EquationContainer
    {
        RowContainer leftEquation = null;
        RowContainer rightEquation = null;
        BracketSign leftArrowSign;
        BracketSign rightArrowSign;
        double ExtraHeight { get; set; }
        double MidSpace { get; set; }

        public DoubleArrowBarBracket(EquationContainer parent)
            : base(parent)
        {
            ExtraHeight = FontSize * 0.2;
            MidSpace = FontSize * 0.5;
            leftArrowSign = new BracketSign(this, BracketSignType.LeftAngle);
            rightArrowSign = new BracketSign(this, BracketSignType.RightAngle);
            ActiveChild = leftEquation = new RowContainer(this);
            rightEquation = new RowContainer(this);
            childEquations.AddRange(new EquationBase[] { leftEquation, leftArrowSign, rightArrowSign, rightEquation });
        }

        public override void DrawEquation(DrawingContext dc)
        {
            base.DrawEquation(dc);
            dc.DrawLine(ThinPen, new Point(leftEquation.Right + MidSpace / 2, Top + ExtraHeight / 2), new Point(leftEquation.Right + MidSpace / 2, Bottom - ExtraHeight / 2));
        }

        public override XElement Serialize()
        {
            XElement thisElement = new XElement(GetType().Name);
            thisElement.Add(leftEquation.Serialize());
            thisElement.Add(rightEquation.Serialize());
            return thisElement;
        }

        public override void DeSerialize(XElement xElement)
        {
            leftEquation.DeSerialize(xElement.Elements().First());
            rightEquation.DeSerialize(xElement.Elements().Last());
            CalculateSize();
        }

        public override double Top
        {
            get { return base.Top; }
            set
            {
                base.Top = value;
                AdjustVertical();
            }
        }

        public override double Left
        {
            get { return base.Left; }
            set
            {
                base.Left = value;
                leftArrowSign.Left = value;
                leftEquation.Left = leftArrowSign.Right;
                rightEquation.Left = leftEquation.Right + MidSpace;
                rightArrowSign.Left = rightEquation.Right;
            }
        }

        public override double FontSize
        {
            get
            {
                return base.FontSize;
            }
            set
            {
                MidSpace = value * 0.5;
                base.FontSize = value;                
            }
        }

        private void AdjustVertical()
        {
            leftEquation.MidY = MidY;
            rightEquation.MidY = MidY;
            leftArrowSign.Top = Top;// +ExtraHeight * .25;
            rightArrowSign.Top = Top;// leftArrowSign.Top;
        }

        public override void CalculateSize()
        {
            CalculateHeight();
            CalculateWidth();
        }

        protected override void CalculateWidth()
        {
            Width = leftEquation.Width + leftArrowSign.Width + rightArrowSign.Width + rightEquation.Width + MidSpace;
        }

        protected override void CalculateHeight()
        {
            Height = Math.Max(leftEquation.Height, rightEquation.Height) + ExtraHeight;
            leftArrowSign.Height = Height - ExtraHeight * 0.5;
            rightArrowSign.Height = leftArrowSign.Height;
        }

        public override double RefY
        {
            get
            {
                return Math.Max(leftEquation.RefY, rightEquation.RefY);
            }
        }
    }
}
