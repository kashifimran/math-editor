using System.Xml.Linq;

namespace Editor.MathML3
{
    public sealed class Row : IMathMLElement
    {
        public XElement ToXElement()
        {
            return new XElement(Ns.MathML + "mrow");
        }
    }
}
