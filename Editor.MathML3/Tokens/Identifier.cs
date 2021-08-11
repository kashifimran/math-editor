using System.Xml.Linq;

namespace Editor.MathML3
{
    /// <summary>
    /// Identifier
    /// </summary>
    public sealed class Identifier : TokenBase
    {
        public override XElement ToXElement()
        {
            var element = new XElement(Ns.MathML + "mi");
            AddTokenAttributes(element);
            return element;
        }
    }
}
