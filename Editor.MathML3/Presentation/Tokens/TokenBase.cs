using System.Xml.Linq;

namespace Editor.MathML3
{
    public abstract class TokenBase : ElementBase
    {
        protected XElement AddTokenAttributes(XElement element)
        {
            AddElementAttributes(element);
            element.AddMathMLAttribute("dir", Dir);
            element.AddMathMLAttribute("displaystyle", DisplayStyle);
            element.AddMathMLAttribute("href", Href);
            element.AddMathMLAttribute("mathbackground", MathBackground);
            element.AddMathMLAttribute("mathcolor", MathColor);
            element.AddMathMLAttribute("mathsize", MathSize);
            element.AddMathMLAttribute("mathvariant", MathVariant);
            return element;
        }

        public string Dir { get; set; } = "ltr";
        public string DisplayStyle { get; set; } = string.Empty;
        public string Href { get; set; } = string.Empty;
        public string MathBackground { get; set; } = string.Empty;
        public string MathColor { get; set; } = string.Empty;
        public string MathSize { get; set; } = string.Empty;
        public string MathVariant { get; set; } = "normal";
    }
}
