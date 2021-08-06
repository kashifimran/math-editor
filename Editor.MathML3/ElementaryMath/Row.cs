using System;
using System.Xml.Linq;

namespace Editor.MathML3
{
    /// <summary>
    /// Row
    /// </summary>
    [Obsolete("not supported in Firefox")]
    public sealed class Row
    {
        public XElement ToXElement()
        {
            return new XElement(Ns.MathML + "mrow");
        }
    }
}
