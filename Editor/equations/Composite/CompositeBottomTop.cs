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
    public class CompositeBottomTop : CompositeBase
    {
        RowContainer topRow;        
        RowContainer bottomRow;

        public CompositeBottomTop(EquationContainer parent)
            : base(parent)
        {
            SubLevel++;
            bottomRow = new RowContainer(this);
            topRow = new RowContainer(this);
            topRow.FontFactor = bottomRow.FontFactor = SubFontFactor;
            topRow.ApplySymbolGap = bottomRow.ApplySymbolGap = false;
            childEquations.AddRange(new EquationBase[] { mainRowContainer, bottomRow, topRow });
        }

        public override XElement Serialize()
        {
            XElement thisElement = new XElement(GetType().Name);
            thisElement.Add(mainRowContainer.Serialize());
            thisElement.Add(bottomRow.Serialize());
            thisElement.Add(topRow.Serialize());
            return thisElement;
        }

        public override void DeSerialize(XElement xElement)
        {
            XElement[] elementArray = xElement.Elements().ToArray();
            mainRowContainer.DeSerialize(elementArray[0]);
            bottomRow.DeSerialize(elementArray[1]);
            topRow.DeSerialize(elementArray[2]);
            CalculateSize();
        }         
        
        public override double Left
        {
            get { return base.Left; }
            set
            {
                base.Left = value;
                bottomRow.MidX = MidX;
                mainRowContainer.MidX = MidX;
                topRow.MidX = MidX;
            }
        }

        protected override void CalculateWidth()
        {
            Width = Math.Max(Math.Max(mainRowContainer.Width, bottomRow.Width), topRow.Width);
        }

        protected override void CalculateHeight()
        {
            Height = mainRowContainer.Height + bottomRow.Height + topRow.Height + bottomGap;            
        }

        public override double RefY
        {
            get
            {
                return topRow.Height + mainRowContainer.RefY;
            }
        }

        public override double Top
        {
            get { return base.Top; }
            set
            {
                base.Top = value;
                topRow.Top = value;
                mainRowContainer.Top = topRow.Bottom;
                bottomRow.Top = mainRowContainer.Bottom + bottomGap;
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
            else if (bottomRow.Bounds.Contains(mousePoint))
            {
                ActiveChild = bottomRow;
                returnValue = true;
            }
            else if (topRow.Bounds.Contains(mousePoint))
            {
                ActiveChild = topRow;
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
                    Point point = ActiveChild.GetVerticalCaretLocation();
                    ActiveChild = bottomRow;
                    point.Y = ActiveChild.Top + 1;
                    ActiveChild.SetCursorOnKeyUpDown(key, point);
                    return true;
                }
                else if (ActiveChild == topRow)
                {
                    Point point = ActiveChild.GetVerticalCaretLocation();
                    ActiveChild = mainRowContainer;
                    point.Y = ActiveChild.Top + 1;
                    ActiveChild.SetCursorOnKeyUpDown(key, point);
                    return true;
                }
            }
            else if (key == Key.Up)
            {
                if (ActiveChild == bottomRow)
                {
                    Point point = ActiveChild.GetVerticalCaretLocation();
                    ActiveChild = mainRowContainer;
                    point.Y = ActiveChild.Bottom - 1;
                    ActiveChild.SetCursorOnKeyUpDown(key, point);
                    return true;
                }
                else if (ActiveChild == mainRowContainer)
                {
                    Point point = ActiveChild.GetVerticalCaretLocation();
                    ActiveChild = topRow;
                    point.Y = ActiveChild.Bottom - 1;
                    ActiveChild.SetCursorOnKeyUpDown(key, point);
                    return true;
                }
            }
            return false;
        }
    }
}
