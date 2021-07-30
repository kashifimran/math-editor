using System.Windows.Media;
using System.Xml.Linq;

namespace Editor
{
    public class Decorated : EquationContainer
    {
        RowContainer rowContainer;
        DecorationDrawing decoration;
        DecorationType decorationType;
        Position decorationPosition;   

        public Decorated(EquationContainer parent, DecorationType decorationType, Position decorationPosition)
            : base(parent)
        {            
            ActiveChild = rowContainer = new RowContainer(this);
            this.decorationType = decorationType;
            this.decorationPosition = decorationPosition;
            decoration = new DecorationDrawing(this, decorationType);
            this.childEquations.Add(rowContainer);
            this.childEquations.Add(decoration);
        }

        public override void DrawEquation(DrawingContext dc)
        {
            rowContainer.DrawEquation(dc);
            decoration.DrawEquation(dc);
        }

        public override XElement Serialize()
        {
            XElement thisElement = new XElement(GetType().Name);
            XElement parameters = new XElement("parameters");
            parameters.Add(new XElement(decorationType.GetType().Name, decorationType));
            parameters.Add(new XElement(decorationPosition.GetType().Name, decorationPosition));
            thisElement.Add(parameters);
            thisElement.Add(rowContainer.Serialize());
            return thisElement;
        }

        public override void DeSerialize(XElement xElement)
        {
            rowContainer.DeSerialize(xElement.Element(rowContainer.GetType().Name));
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
                rowContainer.Left = value;
                decoration.Left = value;
            }
        }

        private void AdjustVertical()
        {
            if (decorationPosition == Position.Top)
            {
                rowContainer.Bottom = Bottom;
                decoration.Top = Top;
            }
            else
            {
                rowContainer.Top = Top;
                decoration.Bottom = Bottom;                
            }
        }

        public override double RefY
        {
            get
            {
                if (decorationPosition == Position.Top)
                {
                    return rowContainer.RefY + decoration.Height;
                }
                else
                {
                    return rowContainer.RefY;
                }
            }
        }
        
        protected override void CalculateHeight()
        {
            if (decorationPosition == Position.Bottom)
            {
                Height = rowContainer.Height + decoration.Height + FontSize * .1;
            }
            else
            {
                Height = rowContainer.Height + decoration.Height;
            }
            AdjustVertical();
        }

        protected override void CalculateWidth()
        {
            Width = rowContainer.Width;
            decoration.Width = Width;
        }
    }
}
