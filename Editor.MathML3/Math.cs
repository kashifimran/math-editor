using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Editor.MathML3
{
    /// <summary>
    /// MathML root element
    /// </summary>
    public sealed class Math
    {
        public XDocument ToXDocument()
        {
            var element = new XElement(Ns.MathML + "math");

            foreach (var child in Children)
            {
                element.Add(child.ToXElement());
            }

            element.AddMathMLAttribute("class", Class);
            element.AddMathMLAttribute("id", Id);
            element.AddMathMLAttribute("style", Style);
            element.AddMathMLAttribute("dir", Dir);
            element.AddMathMLAttribute("mathbackground", MathBackground);
            element.AddMathMLAttribute("mathcolor", MathColor);
            element.AddMathMLAttribute("display", Display);
            element.AddMathMLAttribute("mode", Mode);

            var doc = new XDocument();
            doc.Add(new XDeclaration("1.0", "utf-8", "yes"));
            doc.Add(element);
            return doc;
        }

        public IList<IMathMLElement> Children { get; } = new List<IMathMLElement>();

        public string Class { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;
        public string Style { get; set; } = string.Empty;
        public string Dir { get; set; } = "ltr";
        public string MathBackground { get; set; } = string.Empty;
        public string MathColor { get; set; } = string.Empty;
        public string Display { get; set; } = "inline";

        [Obsolete("deprecated")]
        public string Mode { get; set; } = "display";
    }
}
