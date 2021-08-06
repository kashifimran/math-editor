using System.Xml.Linq;

namespace Editor.MathML3
{
    /// <summary>
    /// Space around content
    /// </summary>
    public sealed class Padded : IMathMLElement
    {
        public XElement ToXElement()
        {
            return new XElement(Ns.MathML + "mpadded");
        }
    }
}
