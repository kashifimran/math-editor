using System.Xml.Linq;

namespace Editor.MathML3
{
    /// <summary>
    /// Table
    /// </summary>
    public sealed class Table : IMathMLElement
    {
        public XElement ToXElement()
        {
            return new XElement(Ns.MathML + "mtable");
        }
    }
}
