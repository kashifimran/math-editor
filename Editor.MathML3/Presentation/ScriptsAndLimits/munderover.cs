using System.Xml.Linq;

namespace Editor.MathML3
{
    /// <summary>
    /// Undercsript and Overscript
    /// </summary>
    public sealed class munderover
    {
        public XElement ToXElement()
        {
            return new XElement(Ns.MathML + "munderover");
        }
    }
}
