using System.Xml.Linq;

namespace Editor.MathML3.Tokens
{
    /// <summary>
    /// Arbitrary text without notational meaning
    /// </summary>
    public sealed class Text : TokenBase
    {
        public override XElement ToXElement()
        {
            var element = new XElement(Ns.MathML + "mtext", Content);

            AddTokenAttributes(element);

            return element;
        }

        public string Content { get; set; } = string.Empty;
    }
}
