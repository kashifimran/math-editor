using System;
using System.Xml.Linq;

namespace Editor.MathML3
{
    /// <summary>
    /// Parentheses
    /// </summary>
    [Obsolete("deprecated in MathML3")]
    public sealed class Fenced : IMathMLElement
    {
        public XElement ToXElement()
        {
            return new XElement(Ns.MathML + "mfenced");
        }
    }
}
