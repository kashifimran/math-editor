using System.Xml.Linq;

namespace Editor.MathML3
{
    /// <summary>
    /// Operator
    /// </summary>
    public sealed class Operator : IMathMLElement
    {
        public XElement ToXElement()
        {
            return new XElement(Ns.MathML + "mo");
        }
    }
}
