using System.Collections.Generic;
using System.Xml.Linq;

namespace Editor.MathML3
{
    public abstract class LayoutBase : ElementBase
    {
        protected XElement AddLayoutBaseAttributes(XElement element)
        {
            AddElementAttributes(element);
            element.AddMathMLAttribute("displaystyle", DisplayStyle);
            element.AddMathMLAttribute("href", Href);
            element.AddMathMLAttribute("mathbackground", MathBackground);
            element.AddMathMLAttribute("mathcolor", MathColor);
            foreach (var content in Content)
            {
                element.Add(content.ToXElement());
            }
            return element;
        }

        public string DisplayStyle { get; set; } = "false";
        public string Href { get; set; } = string.Empty;
        public string MathBackground { get; set; } = string.Empty;
        public string MathColor { get; set; } = string.Empty;
        public IList<IMathMLElement> Content { get; set; } = new List<IMathMLElement>();
    }
}
