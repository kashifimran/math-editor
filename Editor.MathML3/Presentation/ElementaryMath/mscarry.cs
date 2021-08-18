using System;
using System.Xml.Linq;

namespace Editor.MathML3
{
    /// <summary>
    /// Carry
    /// </summary>
    [Obsolete("not supported in Firefox")]
    public sealed class mscarry
    {
        public XElement ToXElement()
        {
            return new XElement(Ns.MathML + "mscarry");
        }
    }
}
