using System.Xml.Linq;

namespace Editor.MathML3
{
    /// <summary>
    /// Bound actions to sub-expressions
    /// </summary>
    public sealed class maction : IMathMLElement
    {
        public XElement ToXElement()
        {
            return new XElement(Ns.MathML + "maction");
        }
    }
}
