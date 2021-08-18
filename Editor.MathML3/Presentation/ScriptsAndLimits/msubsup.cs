using System.Xml.Linq;

namespace Editor.MathML3
{
    /// <summary>
    /// Subscript and Superscript
    /// </summary>
    public sealed class msubsup : IMathMLElement
    {
        public XElement ToXElement()
        {
            return new XElement(Ns.MathML + "msubsup");
        }
    }
}
