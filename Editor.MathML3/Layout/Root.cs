using System.Xml.Linq;

namespace Editor.MathML3
{
    public sealed class Root : IMathMLElement
    {
        public XElement ToXElement()
        {
            return new XElement(Ns.MathML + "mroot");
        }
    }
}
