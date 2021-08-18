using System.Xml.Linq;

namespace Editor.MathML3
{
    /// <summary>
    /// Long division notation
    /// </summary>
    public sealed class mlongdiv : IMathMLElement
    {
        public XElement ToXElement()
        {
            return new XElement(Ns.MathML + "mlongdiv");
        }
    }
}
