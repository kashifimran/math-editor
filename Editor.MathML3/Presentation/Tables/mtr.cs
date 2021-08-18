using System.Xml.Linq;

namespace Editor.MathML3
{
    /// <summary>
    /// Table row
    /// </summary>
    public sealed class mtr : IMathMLElement
    {
        public XElement ToXElement()
        {
            return new XElement(Ns.MathML + "mtr");
        }
    }
}
