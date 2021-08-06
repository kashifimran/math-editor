using System.Xml.Linq;

namespace Editor.MathML3
{
    /// <summary>
    /// Fraction
    /// </summary>
    public sealed class Fraction : IMathMLElement
    {
        public XElement ToXElement()
        {
            return new XElement(Ns.MathML + "mfrac");
        }
    }
}
