using System.Linq;
using System.Xml.Linq;

namespace Editor
{
    class DivTriangle : EquationContainer 
    {
        RowContainer insideEquation = null;
        DivTriangleSign divTriangleSign;
        bool isFixed;

        double ExtraHeight 
        {
            get { return FontSize * .2; }
        }

        double VerticalGap
        {
            get { return FontSize * .1; }
        }

        double LeftGap
        {
            get { return FontSize * .1; }
        }

        public DivTriangle(EquationContainer parent, bool isFixed)
            : base(parent)
        {
            this.isFixed = isFixed;
            divTriangleSign = new DivTriangleSign(this);
            ActiveChild = insideEquation = new RowContainer(this);
            childEquations.Add(insideEquation);
            childEquations.Add(divTriangleSign);
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

        void AdjustVertical()
        {
            divTriangleSign.Bottom = Bottom;
            insideEquation.Top = Top; //Bottom - VerticalGap;            
        }

        public override void CalculateSize()
        {
            CalculateHeight();
            CalculateWidth();
        }

        protected override void CalculateWidth()
        {
            Width = insideEquation.Width + divTriangleSign.Width + LeftGap * 2;
        }

        protected override void CalculateHeight()
        {            
            if (isFixed)
            {
                 divTriangleSign.Height = insideEquation.LastRow.Height + ExtraHeight;
            }
            else
            {
                 divTriangleSign.Height = insideEquation.Height + ExtraHeight;
            }
            Height = insideEquation.Height + ExtraHeight;
        }

        public override double Left
        {
            get { return base.Left; }
            set
            {
                base.Left = value;
                divTriangleSign.Left = value + LeftGap;
                insideEquation.Left = divTriangleSign.Right + LeftGap;
            }
        }

        public override double RefY
        {
            get
            {
                return insideEquation.LastRow.MidY - Top;
            }
        }
    }
}
