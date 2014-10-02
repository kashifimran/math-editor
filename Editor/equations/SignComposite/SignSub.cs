using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Xml.Linq;
using System.Windows.Input;

namespace Editor
{
    class SignSub : EquationContainer
    {
        RowContainer mainEquation;
        StaticSign sign;
        RowContainer subEquation;
        double SubOverlap { get { return FontSize * .5; } }
        double maxUpperHalf = 0;
        double gapFactor = .06;
        double Gap { get { return FontSize * gapFactor; } }
        double LeftMinus { get; set; }
        double MainLeft { get { return Left + LeftMinus; } }

        public SignSub(EquationContainer parent, SignCompositeSymbol symbol, bool useUpright)
            : base(parent)
        {   
            ActiveChild = mainEquation = new RowContainer(this);
            this.SubLevel++;
            subEquation = new RowContainer(this);
            subEquation.ApplySymbolGap = false;            
            sign = new StaticSign(this, symbol, useUpright);
            subEquation.FontFactor = SubFontFactor;
            childEquations.AddRange(new EquationBase[] { mainEquation, sign, subEquation });            
        }

        public override XElement Serialize()
        {
            XElement thisElement = new XElement(GetType().Name);
            XElement parameters = new XElement("parameters");
            parameters.Add(new XElement(sign.Symbol.GetType().Name, sign.Symbol));
            parameters.Add(new XElement(typeof(bool).FullName, sign.UseItalicIntegralSign));
            thisElement.Add(parameters);
            thisElement.Add(mainEquation.Serialize());
            thisElement.Add(subEquation.Serialize());
            return thisElement;
        }

        public override void DeSerialize(XElement xElement)
        {
            XElement[] elements = xElement.Elements(typeof(RowContainer).Name).ToArray();
            mainEquation.DeSerialize(elements[0]);
            subEquation.DeSerialize(elements[1]);
            CalculateSize();
        }

        protected override void CalculateWidth()
        {            
            if (sign.Symbol.ToString().ToLower().Contains("integral"))
            {
                LeftMinus = sign.OverhangTrailing;
            }
            Width = sign.Width + subEquation.Width + mainEquation.Width + Gap + LeftMinus;
        }

        protected override void CalculateHeight()
        {
            maxUpperHalf = Math.Max(mainEquation.RefY, sign.RefY);
            double maxLowerHalf = Math.Max(mainEquation.RefYReverse, sign.RefYReverse + subEquation.Height - SubOverlap);
            Height = maxLowerHalf + maxUpperHalf;
            sign.MidY = MidY;
            mainEquation.MidY = MidY;
            subEquation.Top = sign.Bottom - SubOverlap;
        }

        public override double Height
        {
            get { return base.Height; }
            set
            {
                base.Height = value;
            }
        }

        public override double RefY
        {
            get
            {
                return maxUpperHalf;
            }
        }

        public override double Top
        {
            get { return base.Top; }
            set
            {
                base.Top = value;
                sign.MidY = MidY;
                mainEquation.MidY = MidY;
                subEquation.Top = sign.Bottom - SubOverlap;
            }
        }
        
        public override bool ConsumeMouseClick(System.Windows.Point mousePoint)
        {
            if (subEquation.Bounds.Contains(mousePoint))
            {
                ActiveChild = subEquation;
            }
            else
            {
                ActiveChild = mainEquation;
            }
            return ActiveChild.ConsumeMouseClick(mousePoint);
        }

        public override double Left
        {
            get { return base.Left; }
            set
            {
                base.Left = value;
                sign.Left = value;
                subEquation.Left = sign.Right + LeftMinus;
                mainEquation.Left = subEquation.Right + Gap;
            }
        }

        public override bool ConsumeKey(Key key)
        {
            if (ActiveChild.ConsumeKey(key))
            {
                CalculateSize();
                return true;
            }
            if (key == Key.Down)
            {
                if (ActiveChild == mainEquation)
                {
                    ActiveChild = subEquation;
                    return true;
                }
            }
            else if (key == Key.Up)
            {
                if (ActiveChild == subEquation)
                {
                    ActiveChild = mainEquation;
                    return true;
                }
            }
            return false;
        }
    }
}