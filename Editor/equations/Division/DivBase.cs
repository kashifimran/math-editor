using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

namespace Editor
{
    public abstract class DivBase : EquationContainer
    {
        protected RowContainer topEquation;
        protected RowContainer bottomEquation;

        protected DivBase(EquationContainer parent, bool isSmall = false)
            : base(parent)
        {
            if (isSmall) 
            {
                SubLevel++;
            }
            ActiveChild = topEquation = new RowContainer(this);
            bottomEquation = new RowContainer(this);
            if (isSmall)
            {
                topEquation.FontFactor = SubFontFactor;
                bottomEquation.FontFactor = SubFontFactor;
                topEquation.ApplySymbolGap = false;
                bottomEquation.ApplySymbolGap = false;
            }
            childEquations.AddRange(new EquationBase[] { topEquation, bottomEquation});
        }

        public override XElement Serialize()
        {
            XElement thisElement = new XElement(GetType().Name);
            thisElement.Add(topEquation.Serialize());
            thisElement.Add(bottomEquation.Serialize());
            return thisElement;
        }

        public override void DeSerialize(XElement xElement)
        {
            topEquation.DeSerialize(xElement.Elements().First());
            bottomEquation.DeSerialize(xElement.Elements().Last());
            CalculateSize();
        }       

        public override bool ConsumeMouseClick(Point mousePoint)
        {
            if (bottomEquation.Bounds.Contains(mousePoint))
            {
                ActiveChild = bottomEquation;
                ActiveChild.ConsumeMouseClick(mousePoint);
                return true;
            }
            else if (topEquation.Bounds.Contains(mousePoint))
            {
                ActiveChild = topEquation;
                ActiveChild.ConsumeMouseClick(mousePoint);
                return true;
            } 
            return false;
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
                if (ActiveChild == topEquation)
                {
                    Point point = ActiveChild.GetVerticalCaretLocation();
                    ActiveChild = bottomEquation;
                    point.Y = ActiveChild.Top + 1;
                    ActiveChild.SetCursorOnKeyUpDown(key, point);
                    return true;
                }
            }
            else if (key == Key.Up)
            {
                if (ActiveChild == bottomEquation)
                {
                    Point point = ActiveChild.GetVerticalCaretLocation();
                    ActiveChild = topEquation;
                    point.Y = ActiveChild.Bottom - 1;
                    ActiveChild.SetCursorOnKeyUpDown(key, point);
                    return true;
                }
            }
            return false;
        }
    }
}
