using System.Xml.Linq;

namespace Editor.MathML3
{
    /// <summary>
    /// Identifier
    /// </summary>
    public sealed class Identifier : TokenBase, IMathMLElement
    {
        public XElement ToXElement()
        {
            var element = new XElement(Ns.MathML + "mi");

            element.AddMathMLAttribute("class", Class);
            element.AddMathMLAttribute("id", Id);
            element.AddMathMLAttribute("style", Style);
            element.AddMathMLAttribute("dir", Dir);
            element.AddMathMLAttribute("displaystyle", DisplayStyle);
            element.AddMathMLAttribute("href", Href);
            element.AddMathMLAttribute("mathbackground", MathBackground);
            element.AddMathMLAttribute("mathcolor", MathColor);
            element.AddMathMLAttribute("mathsize", MathSize);
            element.AddMathMLAttribute("mathvariant", MathVariant);

            return element;
        }
    }
}
