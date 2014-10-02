using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows;
using System.Xml.Linq;

namespace Editor
{
    public class SquareRoot : EquationContainer
    {
        protected RowContainer insideEquation = null;
        protected RadicalSign radicalSign;
        protected double ExtraHeight 
        {
            get { return FontSize * .15; }
        }

        protected double LeftGap
        {
            get { return FontSize * .1; }
        }

        public SquareRoot(EquationContainer parent)
            : base(parent)
        {            
            radicalSign = new RadicalSign(this);
            ActiveChild = insideEquation = new RowContainer(this);
            childEquations.Add(insideEquation);
            childEquations.Add(radicalSign);
        }

        public override XElement Serialize()
        {
            XElement thisElement = new XElement(GetType().Name);
            thisElement.Add(insideEquation.Serialize());            
            return thisElement;
        }

        public override void DeSerialize(XElement xElement)
        {
            insideEquation.DeSerialize(xElement.Elements().First());            
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

        private void AdjustVertical()
        {
            insideEquation.Bottom = Bottom;
            radicalSign.Top = Top;
        }

        protected override void CalculateWidth()
        {
            Width = insideEquation.Width + radicalSign.Width + LeftGap;
        }

        protected override void CalculateHeight()
        {
            Height = insideEquation.Height + ExtraHeight;
            radicalSign.Height = Height;
            AdjustVertical();
        }

        public override double Left
        {
            get { return base.Left; }
            set
            {
                base.Left = value;
                radicalSign.Left = value + LeftGap;
                insideEquation.Left = radicalSign.Right;
            }
        }

        public override double RefY
        {
            get
            {
                return insideEquation.RefY + ExtraHeight;
            }
        }
    }
}
