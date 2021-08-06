using System.Xml.Linq;

namespace Editor.MathML3
{
    public class Sqrt : IMathMLElement
    {
        public XElement ToXElement()
        {
            return new XElement(Ns.MathML + "msqrt");
        }
    }
}
