using System;
using System.Xml.Linq;

namespace Editor.MathML3
{
    /// <summary>
    /// 
    /// </summary>
    [Obsolete("not supported in Firefox")]
    public sealed class Prescripts : IMathMLElement
    {
        public XElement ToXElement()
        {
            return new XElement(Ns.MathML + "mprescripts");
        }
    }
}
