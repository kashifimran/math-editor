using System.Xml.Linq;

namespace Editor.MathML3
{
    /// <summary>
    /// Subscript
    /// </summary>
    public sealed class Subscript : IMathMLElement
    {
        public XElement ToXElement()
        {
            return new XElement(Ns.MathML + "msub");
        }
    }
}
