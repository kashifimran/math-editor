using System.Xml.Linq;

namespace Editor.MathML3
{
    /// <summary>
    /// Overscript
    /// </summary>
    public sealed class Overscript : IMathMLElement
    {
        public XElement ToXElement()
        {
            return new XElement(Ns.MathML + "mover");
        }
    }
}
