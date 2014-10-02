using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Xml.Linq;
using System.Windows.Input;

namespace Editor
{
    class SignBottom : EquationContainer
    {
        RowContainer mainEquation;
        RowContainer bottomEquation;
        StaticSign sign;
        double HGap { get { return FontSize * .02; } }
        double VGap { get { return FontSize * .05; } }

        public SignBottom(EquationContainer parent, SignCompositeSymbol symbol, bool useUpright)
            : base(parent)
        {
            ActiveChild = mainEquation = new RowContainer(this);
            SubLevel++;
            bottomEquation = new RowContainer(this);
            bottomEquation.ApplySymbolGap = false;
            sign = new StaticSign(this, symbol, useUpright);
            bottomEquation.FontFactor = SubFontFactor;
            childEquations.AddRange(new EquationBase[] { mainEquation, bottomEquation, sign });
        }

        public override XElement Serialize()
        {
            XElement thisElement = new XElement(GetType().Name);            
            XElement parameters = new XElement("parameters");
            parameters.Add(new XElement(sign.Symbol.GetType().Name, sign.Symbol));
            parameters.Add(new XElement(typeof(bool).FullName, sign.UseItalicIntegralSign));
            thisElement.Add(parameters);
            thisElement.Add(mainEquation.Serialize());
            thisElement.Add(bottomEquation.Serialize());            
            return thisElement;
        }

        public override void DeSerialize(XElement xElement)
        {   
            XElement[] elements = xElement.Elements(typeof(RowContainer).Name).ToArray();
            mainEquation.DeSerialize(elements[0]);
            bottomEquation.DeSerialize(elements[1]);
            CalculateSize();
        }

        protected override void CalculateWidth()
        {
            double maxLeft = Math.Max(sign.Width, bottomEquation.Width);
            Width = maxLeft + mainEquation.Width + HGap;
            sign.MidX = Left + maxLeft / 2;
            bottomEquation.MidX = sign.MidX;
            mainEquation.Left = Math.Max(bottomEquation.Right, sign.Right) + HGap;
        }

        protected override void CalculateHeight()
        {
            double upperHalf = Math.Max(mainEquation.RefY, sign.RefY);
            double lowerHalf = Math.Max(sign.RefY + VGap + bottomEquation.Height, mainEquation.Height - mainEquation.RefY);
            Height = upperHalf + lowerHalf;
            AdjustVertical();
        }

        void AdjustVertical()
        {
            if (mainEquation.RefY > sign.RefY)
            {
                sign.MidY = MidY;
                mainEquation.MidY = MidY;
                bottomEquation.Top = sign.Bottom + VGap;
            }
            else
            {
                bottomEquation.Bottom = Bottom;
                sign.Bottom = bottomEquation.Top - VGap;
                mainEquation.MidY = sign.MidY;
            }
        }

        public override double Top
        {
            get
            {
                return base.Top;
            }
            set
            {
                base.Top = value;
                AdjustVertical();
            }
        }


        public override bool ConsumeMouseClick(System.Windows.Point mousePoint)
        {
            if (bottomEquation.Bounds.Contains(mousePoint))
            {
                ActiveChild = bottomEquation;
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
                double maxLeft = Math.Max(sign.Width, bottomEquation.Width);
                sign.MidX = value + maxLeft / 2;
                bottomEquation.MidX = sign.MidX;
                mainEquation.Left = Math.Max(bottomEquation.Right, sign.Right) + HGap;
            }
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
                return Math.Max(sign.RefY, mainEquation.RefY);
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
                    ActiveChild = bottomEquation;
                    return true;
                }
            }
            else if (key == Key.Up)
            {
                if (ActiveChild == bottomEquation)
                {
                    ActiveChild = mainEquation;
                    return true;
                }
            }
            return false;
        }
    }
}