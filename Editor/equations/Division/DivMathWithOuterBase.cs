using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Editor
{
    public abstract class DivMathWithOuterBase: DivMath
    {
        protected RowContainer outerEquation;

        public DivMathWithOuterBase(EquationContainer parent)
            : base(parent)
        {            
            outerEquation = new RowContainer(this);
            outerEquation.HAlignment = Editor.HAlignment.Right;
            //insideEquation.HAlignment = Editor.HAlignment.Right;
            childEquations.Add(outerEquation);
        }

        public override XElement Serialize()
        {
            XElement thisElement = new XElement(GetType().Name);
            thisElement.Add(insideEquation.Serialize());
            thisElement.Add(outerEquation.Serialize());
            return thisElement;
        }

        public override void DeSerialize(XElement xElement)
        {
            var elements = xElement.Elements().ToArray();
            insideEquation.DeSerialize(elements[0]);
            outerEquation.DeSerialize(elements[1]);
            CalculateSize();
        }       

        public override void CalculateSize()
        {
            CalculateHeight();
            CalculateWidth();
        }

        protected override void CalculateWidth()
        {
            Width = Math.Max(insideEquation.Width, outerEquation.Width) + divMathSign.Width + LeftGap;
        }

        protected override void CalculateHeight()
        {            
            Height = outerEquation.Height + insideEquation.Height + ExtraHeight;
            divMathSign.Height = insideEquation.FirstRow.Height + ExtraHeight;
        }

        public override double Left
        {
            get { return base.Left; }
            set
            {
                base.Left = value;
                divMathSign.Left = value + LeftGap;
                insideEquation.Right = Right;
                outerEquation.Right = Right;
            }
        }
    }
}
