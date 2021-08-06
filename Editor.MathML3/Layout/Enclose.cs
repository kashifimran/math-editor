using System.Xml.Linq;

namespace Editor.MathML3
{
    /// <summary>
    /// Enclosed contents
    /// </summary>
    public sealed class Enclose : IMathMLElement
    {
        public XElement ToXElement()
        {
            return new XElement(Ns.MathML + "menclose");
        }
    }
}
