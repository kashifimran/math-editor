using System;
using System.Text;
using System.Windows.Media;
using System.Xml.Linq;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Editor
{
    public class CompositeSuper : CompositeBase
    {        
        RowContainer topRowContainer;       

        public CompositeSuper(EquationContainer parent)
            : base(parent)
        {
            SubLevel++;
            topRowContainer = new RowContainer(this);
            topRowContainer.FontFactor = SubFontFactor;
            topRowContainer.ApplySymbolGap = false;
            childEquations.AddRange(new EquationBase[] { mainRowContainer, topRowContainer });
        }

        public override XElement Serialize()
        {
            XElement thisElement = new XElement(GetType().Name);
            thisElement.Add(mainRowContainer.Serialize());
            thisElement.Add(topRowContainer.Serialize());
            return thisElement;
        }

        public override void DeSerialize(XElement xElement)
        {
            mainRowContainer.DeSerialize(xElement.Elements().First());
            topRowContainer.DeSerialize(xElement.Elements().Last());
            CalculateSize();
        }
        
        public override double Left
        {
            get { return base.Left; }
            set
            {
                base.Left = value;
                mainRowContainer.Left = Left;
                topRowContainer.Left = mainRowContainer.Right;
                
            }
        }

        protected override void CalculateWidth()
        {
            Width = mainRowContainer.Width + topRowContainer.Width;
        }

        protected override void CalculateHeight()
        {
            Height = mainRowContainer.Height + topRowContainer.Height - SuperOverlap;            
        }

        public override double RefY
        {
            get
            {
                return mainRowContainer.RefY + topRowContainer.Height - SuperOverlap;
            }
        }

        public override double Top
        {
            get { return base.Top; }
            set
            {
                base.Top = value;
                topRowContainer.Top = value;
                mainRowContainer.Bottom = Bottom;
            }
        }

        public override bool ConsumeMouseClick(Point mousePoint)
        {
            if (mainRowContainer.Bounds.Contains(mousePoint))
            {
                ActiveChild = mainRowContainer;
                ActiveChild.ConsumeMouseClick(mousePoint);
                return true;
            }
            else if (topRowContainer.Bounds.Contains(mousePoint))
            {
                ActiveChild = topRowContainer;
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
                if (ActiveChild == topRowContainer)
                {
                    ActiveChild = mainRowContainer;
                    return true;
                }
            }
            else if (key == Key.Up)
            {
                if (ActiveChild == mainRowContainer)
                {
                    ActiveChild = topRowContainer;
                    return true;
                }
            }
            return false;
        }        
    }
}
