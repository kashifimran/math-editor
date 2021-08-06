using System.Xml.Linq;

namespace Editor.MathML3
{
    /// <summary>
    /// Literal String
    /// </summary>
    /// <remarks>
    /// https://developer.mozilla.org/en-US/docs/Web/MathML/Element/ms
    /// </remarks>
    public sealed class LiteralString : TokenBase, IMathMLElement
    {
        public XElement ToXElement()
        {
            var element = new XElement(Ns.MathML + "ms", Content);

            AddTokenAttributes(element);

            element.AddMathMLAttribute("lquote", LeftQuote);
            element.AddMathMLAttribute("rquote", RightQuote);
            
            return element;
        }

        public string Content { get; set; } = string.Empty;

        public string LeftQuote { get; set; } = "&quot;";
        public string RightQuote { get; set; } = "&quot;";
    }
}
