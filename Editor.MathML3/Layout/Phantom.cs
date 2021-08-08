using System.Collections.Generic;
using System.Xml.Linq;

namespace Editor.MathML3
{
    /// <summary>
    /// Invisible content with reserved space
    /// </summary>
    public sealed class Phantom : ElementBase
    {
        public override XElement ToXElement()
        {
            var element  = new XElement(Ns.MathML + "mphantom");
            AddElementAttributes(element);
            element.AddMathMLAttribute("displaystyle", DisplayStyle);
            element.AddMathMLAttribute("mathbackground", MathBackground);
            foreach (var content in Content)
            {
                element.Add(content.ToXElement());
            }
            return element;
        }

        public string DisplayStyle { get; set; } = "false";
        public string MathBackground { get; set; } = string.Empty;
        public IList<IMathMLElement> Content { get; set; } = new List<IMathMLElement>();
    }
}
