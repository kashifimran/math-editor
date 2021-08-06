using System.Xml.Linq;

namespace Editor.MathML3
{
    /// <summary>
    /// Number
    /// </summary>
    public sealed class Number : TokenBase, IMathMLElement
    {
        public XElement ToXElement()
        {
            var element = new XElement(Ns.MathML + "mn", Content);

            AddTokenAttributes(element);

            return element;
        }

        public string Content { get; set; } = string.Empty;
    }
}
