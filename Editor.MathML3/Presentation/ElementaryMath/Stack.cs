using System;
using System.Xml.Linq;

namespace Editor.MathML3
{
    /// <summary>
    /// Stack
    /// </summary>
    [Obsolete("not supported in Firefox")]
    public sealed class Stack
    {
        public XElement ToXElement()
        {
            return new XElement(Ns.MathML + "mstack");
        }
    }
}
