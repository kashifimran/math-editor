using System.Xml.Linq;

namespace Editor.MathML3
{
    public sealed class msqrt : ElementBase
    {
        public override XElement ToXElement()
        {
            var element = new XElement(Ns.MathML + "msqrt");
            AddElementAttributes(element);
            element.AddMathMLAttribute("displaystyle", DisplayStyle);
            element.AddMathMLAttribute("href", Href);
            element.AddMathMLAttribute("mathbackground", MathBackground);
            element.AddMathMLAttribute("mathcolor", MathColor);
            if (Base != null) element.Add(Base.ToXElement());
            return element;
        }

        public string DisplayStyle { get; set; } = "false";
        public string Href { get; set; } = string.Empty;
        public string MathBackground { get; set; } = string.Empty;
        public string MathColor { get; set; } = string.Empty;

        public IMathMLElement? Base { get; set; }
    }
}
