using System.Xml.Linq;

namespace Editor
{
    public class LeftRightBracket : Bracket
    {
        BracketSign bracketSign2;

        public LeftRightBracket(EquationContainer parent, BracketSignType leftBracketType, BracketSignType rightBracketType)
            : base(parent)
        {
            bracketSign = new BracketSign(this, leftBracketType);
            bracketSign2 = new BracketSign(this, rightBracketType);
            childEquations.AddRange(new EquationBase[] { insideEq, bracketSign, bracketSign2 });
        }     

        public override XElement Serialize()
        {
            XElement thisElement = new XElement(GetType().Name);
            XElement parameters = new XElement("parameters");
            parameters.Add(new XElement(bracketSign.SignType.GetType().Name, bracketSign.SignType));
            parameters.Add(new XElement(bracketSign2.SignType.GetType().Name, bracketSign2.SignType));
            thisElement.Add(parameters);
            thisElement.Add(insideEq.Serialize());
            return thisElement;
        }

        public override void DeSerialize(XElement xElement)
        {
            insideEq.DeSerialize(xElement.Element(insideEq.GetType().Name));
            CalculateSize();
        }


        protected override void CalculateWidth()
        {
            Width = insideEq.Width + bracketSign.Width + bracketSign2.Width;
        }

        protected override void CalculateHeight()
        {
            base.CalculateHeight();
            bracketSign2.Height = bracketSign.Height;
        }

        public override double Left
        {
            get { return base.Left; }
            set
            {
                base.Left = value;
                bracketSign.Left = value;
                insideEq.Left = bracketSign.Right;
                bracketSign2.Left = insideEq.Right;
            }
        }

        public override double Top
        {
            get { return base.Top; }
            set
            {
                base.Top = value;
                bracketSign2.Top = bracketSign.Top;
            }
        }
    }
}
