using System;
using System.Xml.Linq;

namespace Editor
{
    class SignSimple : EquationContainer
    {
        protected RowContainer mainEquation;
        protected StaticSign sign;            

        public SignSimple(EquationContainer parent, SignCompositeSymbol symbol, bool useUpright)
            : base(parent)
        {
            ActiveChild = mainEquation = new RowContainer(this);
            sign = new StaticSign(this, symbol, useUpright);  
            childEquations.AddRange(new EquationBase[] {mainEquation, sign});            
        }

        public override XElement Serialize()
        {
            XElement thisElement = new XElement(GetType().Name);
            XElement parameters = new XElement("parameters");
            parameters.Add(new XElement(sign.Symbol.GetType().Name, sign.Symbol));
            parameters.Add(new XElement(typeof(bool).FullName, sign.UseItalicIntegralSign));
            thisElement.Add(parameters);            
            thisElement.Add(mainEquation.Serialize());
            return thisElement;
        }

        public override void DeSerialize(XElement xElement)
        {
            mainEquation.DeSerialize(xElement.Element(mainEquation.GetType().Name));
            CalculateSize();
        }
        
        protected override void CalculateWidth()
        {
            Width = sign.Width + mainEquation.Width;
        }

        protected override void CalculateHeight()
        {
            Height = Math.Max(sign.RefY, mainEquation.RefY) + Math.Max(sign.RefY, mainEquation.Height - mainEquation.RefY);
        }

        public override double Top
        {
            get { return base.Top; }
            set
            {
                base.Top = value;
                sign.MidY = MidY;
                mainEquation.MidY = MidY;
            }
        }
        
        public override double Left
        {
            get { return base.Left; }
            set
            {
                base.Left = value;
                sign.Left = value;
                mainEquation.Left = sign.Right;
            }
        }

        public override double RefY
        {
            get
            {
                return Math.Max(sign.RefY, mainEquation.RefY);
            }
        }
    }
}