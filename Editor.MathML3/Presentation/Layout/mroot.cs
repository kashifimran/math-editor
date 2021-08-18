using System.Xml.Linq;

namespace Editor.MathML3
{
    public sealed class mroot : ElementBase
    {
        public override XElement ToXElement()
        {
            var element = new XElement(Ns.MathML + "mroot");
            AddElementAttributes(element);
            element.AddMathMLAttribute("displaystyle", DisplayStyle);
            element.AddMathMLAttribute("href", Href);
            element.AddMathMLAttribute("mathbackground", MathBackground);
            element.AddMathMLAttribute("mathcolor", MathColor);
            if (Base != null) element.Add(Base.ToXElement());
            if (Exponent != null) element.Add(Exponent.ToXElement());
            return element;
        }

        public string DisplayStyle { get; set; } = "false";
        public string Href { get; set; } = string.Empty;
        public string MathBackground { get; set; } = string.Empty;
        public string MathColor { get; set; } = string.Empty;

        public IMathMLElement? Base { get; set; }
        public IMathMLElement? Exponent { get; set; }
    }
}
