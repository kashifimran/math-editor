using System;
using System.Text;
using System.Windows.Media;
using System.Windows;
using System.Xml.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Input;

namespace Editor
{
    public class CompositeSubSuper: CompositeBase
    {
        RowContainer superRow;        
        RowContainer subRow;

        public CompositeSubSuper(EquationContainer parent)
            : base(parent)
        {
            SubLevel++;
            subRow = new RowContainer(this);
            superRow = new RowContainer(this);
            superRow.FontFactor     = subRow.FontFactor     = SubFontFactor;
            superRow.ApplySymbolGap = subRow.ApplySymbolGap = false;
            childEquations.AddRange(new EquationBase[] { mainRowContainer, subRow, superRow });
        }

        public override XElement Serialize()
        {
            XElement thisElement = new XElement(GetType().Name);
            thisElement.Add(mainRowContainer.Serialize());
            thisElement.Add(subRow.Serialize());
            thisElement.Add(superRow.Serialize());
            return thisElement;
        }

        public override void DeSerialize(XElement xElement)
        {
            XElement[] elementArray = xElement.Elements().ToArray();
            mainRowContainer.DeSerialize(elementArray[0]);
            subRow.DeSerialize(elementArray[1]);
            superRow.DeSerialize(elementArray[2]);
            CalculateSize();
        }         
        
        public override double Left
        {
            get { return base.Left; }
            set
            {
                base.Left = value;
                mainRowContainer.Left = value;
                subRow.Left = mainRowContainer.Right;
                superRow.Left = mainRowContainer.Right;
            }
        }

        protected override void CalculateWidth()
        {
            Width = mainRowContainer.Width + Math.Max(subRow.Width, superRow.Width);
        }

        protected override void CalculateHeight()
        {
            Height = mainRowContainer.Height + subRow.Height - SubOverlap + superRow.Height - SuperOverlap;
        }

        public override double RefY
        {
            get
            {
                return superRow.Height - SuperOverlap + mainRowContainer.RefY;
            }
        }

        public override double Top
        {
            get { return base.Top; }
            set
            {
                base.Top = value;
                superRow.Top = value;
                mainRowContainer.Top = superRow.Bottom - SuperOverlap;
                subRow.Bottom = Bottom;
            }
        }

        public override bool ConsumeMouseClick(Point mousePoint)
        {
            bool returnValue = false;
            if (mainRowContainer.Bounds.Contains(mousePoint))
            {
                ActiveChild = mainRowContainer;
                returnValue = true;
            }
            else if (subRow.Bounds.Contains(mousePoint))
            {
                ActiveChild = subRow;
                returnValue = true;
            }
            else if (superRow.Bounds.Contains(mousePoint))
            {
                ActiveChild = superRow;
                returnValue = true;
            }
            ActiveChild.ConsumeMouseClick(mousePoint);
            return returnValue;
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
                if (ActiveChild == mainRowContainer)
                {
                    ActiveChild = subRow;
                    return true;
                }
                else if (ActiveChild == superRow)
                {
                    ActiveChild = mainRowContainer;
                    return true;
                }
            }
            else if (key == Key.Up)
            {
                if (ActiveChild == subRow)
                {
                    ActiveChild = mainRowContainer;
                    return true;
                }
                else if (ActiveChild == mainRowContainer)
                {
                    ActiveChild = superRow;
                    return true;
                }
            }
            return false;
        }
    }
}
