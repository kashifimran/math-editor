using System.Xml.Linq;

namespace Editor.MathML3
{
    /// <summary>
    /// Blank Space
    /// </summary>
    public sealed class mspace : IMathMLElement
    {
        public XElement ToXElement()
        {
            var element = new XElement(Ns.MathML + "mspace");

            element.AddMathMLAttribute("class", Class);
            element.AddMathMLAttribute("id", Id);
            element.AddMathMLAttribute("style", Style);
            element.AddMathMLAttribute("displaystyle", DisplayStyle);
            element.AddMathMLAttribute("mathbackground", MathBackground);

            element.AddMathMLAttribute("depth", Depth);
            element.AddMathMLAttribute("height", Height);
            element.AddMathMLAttribute("width", Width);

            return element;
        }

        public string Class { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;
        public string Style { get; set; } = string.Empty;
        public string DisplayStyle { get; set; } = string.Empty;
        public string MathBackground { get; set; } = string.Empty;
        
        public string Depth { get; set; } = string.Empty;
        public string Height { get; set; } = string.Empty;
        public string Width { get; set; } = string.Empty;
    }
}
