using System.Xml.Linq;

namespace Editor.MathML3
{
    /// <summary>
    /// Long division notation
    /// </summary>
    public sealed class LongDiv : IMathMLElement
    {
        public XElement ToXElement()
        {
            return new XElement(Ns.MathML + "mlongdiv");
        }
    }
}
