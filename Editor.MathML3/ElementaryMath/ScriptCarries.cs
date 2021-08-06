using System;
using System.Xml.Linq;

namespace Editor.MathML3
{
    /// <summary>
    /// Carries, borrows, and crossouts
    /// </summary>
    [Obsolete("not supported in Firefox")]
    public sealed class ScriptCarries
    {
        public XElement ToXElement()
        {
            return new XElement(Ns.MathML + "mscarries");
        }
    }
}
