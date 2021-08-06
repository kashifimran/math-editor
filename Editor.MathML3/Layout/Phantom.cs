using System.Xml.Linq;

namespace Editor.MathML3
{
    /// <summary>
    /// Invisible content with reserved space
    /// </summary>
    public sealed class Phantom : IMathMLElement
    {
        public XElement ToXElement()
        {
            return new XElement(Ns.MathML + "mphantom");
        }
    }
}
