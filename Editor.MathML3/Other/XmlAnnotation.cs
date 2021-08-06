using System.Xml.Linq;

namespace Editor.MathML3
{
    /// <summary>
    /// XML annotation
    /// </summary>
    public sealed class XmlAnnotation : IMathMLElement
    {
        public XElement ToXElement()
        {
            return new XElement(Ns.MathML + "annotation-xml");
        }
    }
}
