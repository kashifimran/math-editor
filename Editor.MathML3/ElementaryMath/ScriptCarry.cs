using System;
using System.Xml.Linq;

namespace Editor.MathML3
{
    /// <summary>
    /// Carry
    /// </summary>
    [Obsolete("not supported in Firefox")]
    public sealed class ScriptCarry
    {
        public XElement ToXElement()
        {
            return new XElement(Ns.MathML + "mscarry");
        }
    }
}
