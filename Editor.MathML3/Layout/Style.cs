using System.Xml.Linq;

namespace Editor.MathML3
{
    public class Style : IMathMLElement
    {
        public XElement ToXElement()
        {
            return new XElement(Ns.MathML + "mstyle");
        }
    }
}
