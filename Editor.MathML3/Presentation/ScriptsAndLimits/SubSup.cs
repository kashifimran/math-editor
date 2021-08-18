using System.Xml.Linq;

namespace Editor.MathML3
{
    /// <summary>
    /// Subscript and Superscript
    /// </summary>
    public class SubSup : IMathMLElement
    {
        public XElement ToXElement()
        {
            return new XElement(Ns.MathML + "msubsup");
        }
    }
}
