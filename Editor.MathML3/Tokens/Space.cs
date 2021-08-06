using System.Xml.Linq;

namespace Editor.MathML3
{
    /// <summary>
    /// Blank Space
    /// </summary>
    public sealed class Space : IMathMLElement
    {
        public XElement ToXElement()
        {
            return new XElement(Ns.MathML + "mspace");
        }
    }
}
