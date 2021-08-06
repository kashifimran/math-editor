using System.Xml.Linq;

namespace Editor.MathML3
{
    public abstract class TokenBase
    {
        protected XElement AddTokenAttributes(XElement element)
        {
            element.AddMathMLAttribute("class", Class);
            element.AddMathMLAttribute("id", Id);
            element.AddMathMLAttribute("style", Style);
            element.AddMathMLAttribute("dir", Dir);
            element.AddMathMLAttribute("displaystyle", DisplayStyle);
            element.AddMathMLAttribute("href", Href);
            element.AddMathMLAttribute("mathbackground", MathBackground);
            element.AddMathMLAttribute("mathcolor", MathColor);
            element.AddMathMLAttribute("mathsize", MathSize);
            element.AddMathMLAttribute("mathvariant", MathVariant);
            return element;
        }

        public string Class { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;
        public string Style { get; set; } = string.Empty;
        public string Dir { get; set; } = "ltr";
        public string DisplayStyle { get; set; } = string.Empty;
        public string Href { get; set; } = string.Empty;
        public string MathBackground { get; set; } = string.Empty;
        public string MathColor { get; set; } = string.Empty;
        public string MathSize { get; set; } = string.Empty;
        public string MathVariant { get; set; } = "normal";
    }
}
