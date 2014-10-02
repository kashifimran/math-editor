using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Xml.Linq;
using System.Windows.Input;

namespace Editor
{
    public class nRoot : EquationContainer
    {
        protected RowContainer insideEquation = null;
        RowContainer nthRootEquation = null;
        protected RadicalSign radicalSign;
        protected double ExtraHeight { get { return FontSize * .15; }}

        double HGap { get { return FontSize * .5; } }
        double LeftPadding { get { return FontSize * .1; } }       

        public nRoot(EquationContainer parent)
            : base(parent)
        {
            radicalSign = new RadicalSign(this);
            ActiveChild = insideEquation = new RowContainer(this);
            nthRootEquation = new RowContainer(this);
            nthRootEquation.ApplySymbolGap = false;
            nthRootEquation.FontFactor = SubFontFactor;
            childEquations.AddRange(new EquationBase[] { insideEquation, radicalSign, nthRootEquation });
        }

        public override XElement Serialize()
        {
            XElement thisElement = new XElement(GetType().Name);
            thisElement.Add(insideEquation.Serialize());
            thisElement.Add(nthRootEquation.Serialize());
            return thisElement;
        }

        public override void DeSerialize(XElement xElement)
        {
            insideEquation.DeSerialize(xElement.Elements().First());
            nthRootEquation.DeSerialize(xElement.Elements().Last());
            CalculateSize();
        }

        public override bool ConsumeMouseClick(System.Windows.Point mousePoint)
        {
            if (nthRootEquation.Bounds.Contains(mousePoint))
            {
                ActiveChild = nthRootEquation;
            }
            else if (insideEquation.Bounds.Contains(mousePoint))
            {
                ActiveChild = insideEquation;
            }
            return ActiveChild.ConsumeMouseClick(mousePoint);
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
            radicalSign.Bottom = Bottom;
            nthRootEquation.Bottom = radicalSign.MidY - FontSize * .05;
        }

        protected override void CalculateWidth()
        {
            Width = Math.Max(nthRootEquation.Width + HGap, radicalSign.Width) + insideEquation.Width + LeftPadding;            
        }

        protected override void CalculateHeight()
        {
            Height = insideEquation.Height + Math.Max(0, nthRootEquation.Height - insideEquation.Height / 2 + FontSize * .05) + ExtraHeight;
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
                radicalSign.Height = insideEquation.Height + ExtraHeight;
                AdjustVertical();
            }
        }

        public override double RefY
        {
            get
            {
                return insideEquation.RefY + Math.Max(0, nthRootEquation.Height - insideEquation.Height / 2 + FontSize * .05) + ExtraHeight;
            }
        }

        public override double Left
        {
            get { return base.Left; }
            set
            {
                base.Left = value;                
                if (nthRootEquation.Width + HGap > radicalSign.Width)
                {
                    nthRootEquation.Left = Left + LeftPadding;
                    radicalSign.Right = nthRootEquation.Right + HGap;
                }
                else
                {
                    radicalSign.Left = Left + LeftPadding;
                    nthRootEquation.Right = radicalSign.Right - HGap;
                }
                insideEquation.Left = radicalSign.Right;
            }
        }
        public override bool ConsumeKey(Key key)
        {
            if (ActiveChild.ConsumeKey(key))
            {
                CalculateSize();
                return true;
            }
            if (key == Key.Left)
            {
                if (ActiveChild == insideEquation)
                {
                    ActiveChild = nthRootEquation;
                    return true;
                }
            }
            else if (key == Key.Right)
            {
                if (ActiveChild == nthRootEquation)
                {
                    ActiveChild = insideEquation;
                    return true;
                }
            }
            return false;
        }
    }
}
