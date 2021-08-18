using System.Xml.Linq;

namespace Editor.MathML3
{
    /// <summary>
    /// Superscript
    /// </summary>
    public sealed class msup : IMathMLElement
    {
        public XElement ToXElement()
        {
            return new XElement(Ns.MathML + "msup");
        }
    }
}
