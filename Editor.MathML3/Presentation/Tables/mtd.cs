using System.Xml.Linq;

namespace Editor.MathML3
{
    /// <summary>
    /// Table data cell
    /// </summary>
    public sealed class mtd : IMathMLElement
    {
        public XElement ToXElement()
        {
            return new XElement(Ns.MathML + "mtd");
        }
    }
}
