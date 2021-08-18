using System.Collections.Generic;
using System.Xml.Linq;

namespace Editor.MathML3
{
    /// <summary>
    /// Applies attributes to all child elements.
    /// </summary>
    /// <remarks>
    /// https://developer.mozilla.org/en-US/docs/Web/MathML/Element/mstyle
    /// </remarks>
    public sealed class Style : IMathMLElement
    {
        public XElement ToXElement()
        {
            var element = new XElement(Ns.MathML + "mstyle");
            element.AddMathMLAttribute("dir", Dir);
            element.AddMathMLAttribute("displaystyle", DisplayStyle);
            element.AddMathMLAttribute("infixlinebreakstyle", InfixLineBreakstyle);
            element.AddMathMLAttribute("scriptlevel", ScriptLevel);
            element.AddMathMLAttribute("scriptminsize", ScriptMinSize);
            element.AddMathMLAttribute("scriptsizemultiplier", ScriptSizeMultiplier);
            foreach (var content in Content)
            {
                element.Add(content.ToXElement());
            }
            return element;
        }

        public string Dir { get; set; } = "ltr";
        public string DisplayStyle { get; set; } = "false";
        public string InfixLineBreakstyle { get; set; } = string.Empty;
        public string ScriptLevel { get; set; } = string.Empty;
        public string ScriptMinSize { get; set; } = string.Empty;
        public string ScriptSizeMultiplier { get; set; } = string.Empty;

        // TODO: element accepts almost all presentation attributes
        // https://developer.mozilla.org/en-US/docs/Web/MathML/Attribute

        public IList<IMathMLElement> Content { get; set; } = new List<IMathMLElement>();
    }
}
