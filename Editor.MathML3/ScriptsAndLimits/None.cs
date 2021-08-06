using System;
using System.Xml.Linq;

namespace Editor.MathML3
{
    /// <summary>
    /// No script formatting
    /// </summary>
    [Obsolete("not supported in Firefox")]
    public sealed class None : IMathMLElement
    {
        public XElement ToXElement()
        {
            return new XElement(Ns.MathML + "mnone");
        }
    }
}
