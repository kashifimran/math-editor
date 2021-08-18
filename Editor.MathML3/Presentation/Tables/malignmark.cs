using System;
using System.Xml.Linq;

namespace Editor.MathML3
{
    [Obsolete("Not supported in Firefox")]
    public sealed class malignmark : IMathMLElement
    {
        public XElement ToXElement()
        {
            return new XElement(Ns.MathML + "malignmark");
        }
    }
}
