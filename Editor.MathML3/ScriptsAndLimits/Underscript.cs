using System.Xml.Linq;

namespace Editor.MathML3
{
    /// <summary>
    /// Underscript
    /// </summary>
    public sealed class Underscript : IMathMLElement
    {
        public XElement ToXElement()
        {
            return new XElement(Ns.MathML + "munder");
        }
    }
}
