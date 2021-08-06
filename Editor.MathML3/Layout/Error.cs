using System.Xml.Linq;

namespace Editor.MathML3
{
    /// <summary>
    /// Enclosed syntax error messages
    /// </summary>
    public sealed class Error : IMathMLElement
    {
        public XElement ToXElement()
        {
            return new XElement(Ns.MathML + "merror");
        }
    }
}
