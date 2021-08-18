using System.Xml.Linq;

namespace Editor.MathML3
{
    /// <summary>
    /// Semantic annotation
    /// </summary>
    public sealed class Semantics : IMathMLElement
    {
        public XElement ToXElement()
        {
            return new XElement(Ns.MathML + "semantics");
        }
    }
}
