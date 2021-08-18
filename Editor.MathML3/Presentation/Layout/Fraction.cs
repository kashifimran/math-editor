using System;
using System.Xml.Linq;

namespace Editor.MathML3
{
    /// <summary>
    /// Fraction
    /// </summary>
    public sealed class Fraction : LayoutBase
    {
        public override XElement ToXElement()
        {
            var element = new XElement(Ns.MathML + "mfrac");
            AddLayoutBaseAttributes(element);
            element.AddMathMLAttribute("bevelled", Bevelled);
            element.AddMathMLAttribute("denomalign", Denomalign);
            element.AddMathMLAttribute("linethickness", LineThickness);
            element.AddMathMLAttribute("numalign", Numalign);
            return element;
        }

        public string Bevelled { get; set; } = "false";

        [Obsolete("")]
        public string Denomalign { get; set; } = "center";

        public string LineThickness { get; set; } = "1px";

        [Obsolete("")]
        public string Numalign { get; set; } = "center";
    }
}
