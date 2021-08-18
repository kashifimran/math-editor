using System.Xml.Linq;

namespace Editor.MathML3
{
    /// <summary>
    /// Undercsript and Overscript
    /// </summary>
    public sealed class UnderOver
    {
        public XElement ToXElement()
        {
            return new XElement(Ns.MathML + "munderover");
        }
    }
}
