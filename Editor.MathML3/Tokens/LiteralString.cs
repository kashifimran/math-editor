using System.Xml.Linq;

namespace Editor.MathML3
{
    /// <summary>
    /// Literal String
    /// </summary>
    /// <remarks>
    /// https://developer.mozilla.org/en-US/docs/Web/MathML/Element/ms
    /// </remarks>
    public sealed class LiteralString : IMathMLElement
    {
        public XElement ToXElement()
        {
            var element = new XElement(Ns.MathML + "ms", Content);
            
            element.AddMathMLAttribute("class", Class);
            element.AddMathMLAttribute("id", Id);
            element.AddMathMLAttribute("style", Style);
            element.AddMathMLAttribute("dir", Dir);
            element.AddMathMLAttribute("displaystyle", DisplayStyle);
            element.AddMathMLAttribute("lquote", LeftQuote);
            element.AddMathMLAttribute("href", Href);
            element.AddMathMLAttribute("mathbackground", MathBackground);
            element.AddMathMLAttribute("mathcolor", MathColor);
            element.AddMathMLAttribute("mathsize", MathSize);
            element.AddMathMLAttribute("mathvariant", MathVariant);
            
            return element;
        }

        public string Content { get; set; } = string.Empty;

        public string Class { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;
        public string Style { get; set; } = string.Empty;
        public string Dir { get; set; } = "ltr";
        public string DisplayStyle { get; set; } = string.Empty;
        public string LeftQuote { get; set; } = "&quot;";
        public string RightQuote { get; set; } = "&quot;";
        public string Href { get; set; } = string.Empty;
        public string MathBackground { get; set; } = string.Empty;
        public string MathColor { get; set; } = string.Empty;
        public string MathSize { get; set; } = string.Empty;
        public string MathVariant { get; set; } = "normal";
    }
}
