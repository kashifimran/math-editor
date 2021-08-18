using System.Xml.Linq;

namespace Editor.MathML3
{
    /// <summary>
    /// Number
    /// </summary>
    public sealed class mn : TokenBase
    {
        public override XElement ToXElement()
        {
            var element = new XElement(Ns.MathML + "mn", Content);
            AddTokenAttributes(element);
            return element;
        }
    }
}
