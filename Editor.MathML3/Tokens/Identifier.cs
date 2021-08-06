using System.Xml.Linq;

namespace Editor.MathML3
{
    /// <summary>
    /// Identifier
    /// </summary>
    public sealed class Identifier : IMathMLElement
    {
        public XElement ToXElement()
        {
            return new XElement(Ns.MathML + "mi");
        }
    }
}
