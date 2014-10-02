using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Editor
{
    public class DivMath : EquationContainer
    {
        protected RowContainer insideEquation = null;
        protected DivMathSign divMathSign;
        protected double ExtraHeight 
        {
            get { return FontSize * .3; }
        }

        protected double VerticalGap
        {
            get { return FontSize * .1; }
        }

        protected double LeftGap
        {
            get { return FontSize * .1; }
        }

        public DivMath(EquationContainer parent)
            : base(parent)
        {
            divMathSign = new DivMathSign(this);
            ActiveChild = insideEquation = new RowContainer(this);            
            childEquations.Add(insideEquation);
            childEquations.Add(divMathSign);
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

        protected virtual void AdjustVertical()
        {
            insideEquation.Top = Top + VerticalGap;
            divMathSign.Top = Top + VerticalGap;
        }

        public override void CalculateSize()
        {
            CalculateHeight();
            CalculateWidth();
        }

        protected override void CalculateWidth()
        {
            Width = insideEquation.Width + divMathSign.Width + LeftGap;
        }

        protected override void CalculateHeight()
        {
            divMathSign.Height = insideEquation.FirstRow.Height + ExtraHeight;
            Height = Math.Max(insideEquation.Height + ExtraHeight, divMathSign.Height);            
        }

        public override double Left
        {
            get { return base.Left; }
            set
            {
                base.Left = value;
                divMathSign.Left = value + LeftGap;
                insideEquation.Left = divMathSign.Right;
            }
        }

        public override double RefY
        {
            get
            {
                return insideEquation.FirstRow.RefY + VerticalGap;
            }
        }
    }
}
