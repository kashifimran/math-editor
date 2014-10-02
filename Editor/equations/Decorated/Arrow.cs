using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows;
using System.Xml.Linq;
using System.Windows.Input;

namespace Editor
{
    public class Arrow : EquationContainer
    {
        RowContainer rowContainer1;
        RowContainer rowContainer2;
        DecorationDrawing arrow1;
        DecorationDrawing arrow2;
        ArrowType arrowType;
        Position equationPosition;

        double ArrowGap
        {
            get
            {
                if (arrowType == ArrowType.SmallRightLeftHarpoon || arrowType == ArrowType.RightSmallLeftHarpoon || 
                    arrowType == ArrowType.RightLeftHarpoon)
                {
                    return FontSize * .2;
                }
                else
                {
                    return 0; //FontSize * .02;
                }
            }
        }

        public Arrow(EquationContainer parent, ArrowType arrowType, Position equationPosition)
            : base(parent)
        {
            this.arrowType = arrowType;
            this.equationPosition = equationPosition;
            SubLevel++;
            ApplySymbolGap = false;
            ActiveChild = rowContainer1 = new RowContainer(this);
            rowContainer1.FontFactor = SubFontFactor;
            this.childEquations.Add(rowContainer1);
            CreateDecorations();
            if (equationPosition == Position.BottomAndTop)
            {
                rowContainer2 = new RowContainer(this);
                rowContainer2.FontFactor = SubFontFactor;
                childEquations.Add(rowContainer2);
            }
        }

        void CreateDecorations()
        {
            switch (arrowType)
            {
                case ArrowType.LeftArrow:
                    arrow1 = new DecorationDrawing(this, DecorationType.LeftArrow);
                    childEquations.Add(arrow1);
                    break;
                case ArrowType.RightArrow:
                    arrow1 = new DecorationDrawing(this, DecorationType.RightArrow);
                    childEquations.Add(arrow1);
                    break;
                case ArrowType.DoubleArrow:
                    arrow1 = new DecorationDrawing(this, DecorationType.DoubleArrow);
                    childEquations.Add(arrow1);
                    break;
                case ArrowType.RightLeftArrow:
                case ArrowType.RightSmallLeftArrow:
                case ArrowType.SmallRightLeftArrow:
                    arrow1 = new DecorationDrawing(this, DecorationType.RightArrow);
                    arrow2 = new DecorationDrawing(this, DecorationType.LeftArrow);
                    childEquations.Add(arrow1);
                    childEquations.Add(arrow2);
                    break;
                case ArrowType.RightLeftHarpoon:
                case ArrowType.RightSmallLeftHarpoon:
                case ArrowType.SmallRightLeftHarpoon:
                    arrow1 = new DecorationDrawing(this, DecorationType.RightHarpoonUpBarb);
                    arrow2 = new DecorationDrawing(this, DecorationType.LeftHarpoonDownBarb);
                    childEquations.Add(arrow1);
                    childEquations.Add(arrow2);
                    break;
            }
        }

        public override XElement Serialize()
        {
            XElement thisElement = new XElement(GetType().Name);
            XElement parameters = new XElement("parameters");
            parameters.Add(new XElement(arrowType.GetType().Name, arrowType));
            parameters.Add(new XElement(equationPosition.GetType().Name, equationPosition));
            thisElement.Add(parameters);
            thisElement.Add(rowContainer1.Serialize());
            if (rowContainer2 != null)
            {
                thisElement.Add(rowContainer2.Serialize());
            }
            return thisElement;
        }

        public override void DeSerialize(XElement xElement)
        {
            XElement[] elements = xElement.Elements(rowContainer1.GetType().Name).ToArray();
            rowContainer1.DeSerialize(elements[0]);
            if (rowContainer2 != null)
            {
                rowContainer2.DeSerialize(elements[1]);
            }
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

        public override double Left
        {
            get { return base.Left; }
            set
            {
                base.Left = value;
                foreach (EquationBase eb in childEquations)
                {
                    eb.MidX = MidX;
                }
            }
        }

        private void AdjustVertical()
        {
            if (equationPosition == Position.Top)
            {
                rowContainer1.Top = Top;
                if (arrow2 != null)
                {
                    arrow1.Top = rowContainer1.Bottom;
                    arrow2.Bottom = Bottom;
                }
                else
                {
                    arrow1.Bottom = Bottom;
                }
            }
            else if (equationPosition == Position.Bottom)
            {
                arrow1.Top = Top;
                rowContainer1.Bottom = Bottom;
                if (arrow2 != null)
                {
                    arrow2.Top = arrow1.Bottom + ArrowGap;
                }
            }
            else if (equationPosition == Position.BottomAndTop)
            {
                rowContainer1.Top = Top;
                arrow1.Top = rowContainer1.Bottom;
                if (arrow2 != null)
                {
                    arrow2.Top = arrow1.Bottom + ArrowGap;
                }
                rowContainer2.Bottom = Bottom;
            }
        }

        public override double RefY
        {
            get
            {
                if (equationPosition == Position.Top) //only top container
                {
                    if (arrow2 == null)
                    {
                        return Height - LineThickness;// -arrow1.Height / 2;
                    }
                    else
                    {
                        return Height - LineThickness * 4;
                    }
                }
                else if (equationPosition == Position.Bottom) //only bottom container
                {
                    if (arrow2 == null)
                    {
                        return arrow1.Height / 2;
                    }
                    else
                    {
                        return arrow1.Height + ArrowGap / 2;
                    }
                }
                else //both top and bottom containers
                {
                    if (arrow2 == null)
                    {
                        return rowContainer1.Height + arrow1.Height / 2;
                    }
                    else
                    {
                        return rowContainer1.Height + arrow1.Height + ArrowGap / 2;
                    }
                }
            }
        }

        protected override void CalculateHeight()
        {
            Height = childEquations.Sum(x => x.Height) + ArrowGap;
        }

        protected override void CalculateWidth()
        {
            if (arrowType.ToString().ToLower().Contains("small"))
            {
                Width = Math.Max(rowContainer1.Width, (rowContainer2 != null ? rowContainer2.Width : 0)) + FontSize * 3;
            }
            else
            {
                Width = Math.Max(rowContainer1.Width, (rowContainer2 != null ? rowContainer2.Width : 0)) + FontSize * 2;
            }
            switch (arrowType)
            {
                case ArrowType.LeftArrow:
                case ArrowType.RightArrow:
                case ArrowType.DoubleArrow:
                    arrow1.Width = Width - FontSize * .3;
                    break;

                case ArrowType.RightLeftArrow:
                case ArrowType.RightLeftHarpoon:
                    arrow1.Width = Width - FontSize * .3;
                    arrow2.Width = Width - FontSize * .3;
                    break;

                case ArrowType.RightSmallLeftArrow:
                case ArrowType.RightSmallLeftHarpoon:
                    arrow1.Width = Width - FontSize * .3;
                    arrow2.Width = Width - FontSize * 1.5;
                    break;

                case ArrowType.SmallRightLeftArrow:
                case ArrowType.SmallRightLeftHarpoon:
                    arrow1.Width = Width - FontSize * 1.5;
                    arrow2.Width = Width - FontSize * .3;
                    break;
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
                if (ActiveChild == rowContainer1)
                {
                    Point point = ActiveChild.GetVerticalCaretLocation();
                    ActiveChild = rowContainer2;
                    point.Y = ActiveChild.Top + 1;
                    ActiveChild.SetCursorOnKeyUpDown(key, point);
                    return true;
                }
            }
            else if (key == Key.Up)
            {
                if (ActiveChild == rowContainer2)
                {
                    Point point = ActiveChild.GetVerticalCaretLocation();
                    ActiveChild = rowContainer1;
                    point.Y = ActiveChild.Bottom - 1;
                    ActiveChild.SetCursorOnKeyUpDown(key, point);
                    return true;
                }
            }
            return false;
        }
    }
}
