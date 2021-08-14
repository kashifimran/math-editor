using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Editor.MathML3
{
    /// <summary>
    /// MathML root element
    /// </summary>
    public sealed class Math : ElementBase
    {
        public XDocument ToXDocument()
        {
            var element = this.ToXElement();
            var doc = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XComment($"Created on {DateTime.UtcNow:yyyy-mm-ddTHH:mm:ssZ}"));
            doc.Add(element);
            return doc;
        }

        public override XElement ToXElement()
        {
            var element = new XElement(Ns.MathML + "math",
                new XAttribute(XNamespace.Xmlns + "m", Ns.MathML),
                new XAttribute(XNamespace.Xmlns + "xml", Ns.XML),
                new XAttribute(XNamespace.Xmlns + "rdf", Ns.RDF),
                new XAttribute(XNamespace.Xmlns + "html", Ns.HTML5),
                new XAttribute(XNamespace.Xmlns + "dc", Ns.DC),
                new XAttribute(XNamespace.Xmlns + "svg", Ns.SVG),
                new XAttribute(XNamespace.Xmlns + "xlink", Ns.XLink));

            AddElementAttributes(element);
            element.AddMathMLAttribute("class", Class);
            element.AddMathMLAttribute("id", Id);
            element.AddMathMLAttribute("style", Style);
            element.AddMathMLAttribute("dir", Dir);
            element.AddMathMLAttribute("mathbackground", MathBackground);
            element.AddMathMLAttribute("mathcolor", MathColor);
            element.AddMathMLAttribute("display", Display);
            element.AddMathMLAttribute("mode", Mode);
            element.AddMathMLAttribute("macros", Macros);
            element.AddMathMLAttribute("bevelled", Bevelled);
            element.AddMathMLAttribute("mathsize", Mathsize);
            element.AddXMLAttribute("lang", LanguageCode);

            foreach (var child in Children)
            {
                element.Add(child.ToXElement());
            }

            return element;
        }

        public IList<IMathMLElement> Children { get; } = new List<IMathMLElement>();

        public string Dir { get; set; } = "ltr";
        public string MathBackground { get; set; } = string.Empty;
        public string MathColor { get; set; } = string.Empty;
        public string Display { get; set; } = "inline";

        [Obsolete("deprecated")]
        public string Mode { get; set; } = "display";

        public string Macros { get; set; } = string.Empty;

        public string Bevelled { get; set; } = string.Empty;

        public string Mathsize { get; set; } = string.Empty;

        /// <summary>
        /// The ISO 639-1 language code for the language of the content.
        /// </summary>
        public string LanguageCode { get; set; } = string.Empty;
    }
}
