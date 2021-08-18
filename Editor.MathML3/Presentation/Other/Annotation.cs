using System.Xml.Linq;

namespace Editor.MathML3
{
    /// <summary>
    /// Annotation
    /// </summary>
    public sealed class Annotation : IMathMLElement
    {
        public XElement ToXElement()
        {
            return new XElement(Ns.MathML + "annotation");
        }
    }
}
