using System.Xml.Linq;

namespace Editor.MathML3.Tokens
{
    /// <summary>
    /// Arbitrary text without notational meaning
    /// </summary>
    public sealed class Text : IMathMLElement
    {
        public XElement ToXElement()
        {
            return new XElement(Ns.MathML + "mtext");
        }
    }
}
