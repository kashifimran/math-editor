using System.Xml.Linq;

namespace Editor.MathML3
{
    /// <summary>
    /// Number
    /// </summary>
    public sealed class Number : IMathMLElement
    {
        public XElement ToXElement()
        {
            return new XElement(Ns.MathML + "mn");
        }
    }
}
