using System.Xml.Linq;

namespace Editor.MathML3
{
    public abstract class ElementBase : IMathMLElement
    {
        protected XElement AddElementAttributes(XElement element)
        {
            element.AddMathMLAttribute("class", Class);
            element.AddMathMLAttribute("id", Id);
            element.AddMathMLAttribute("style", Style);
            element.AddXMLAttribute("lang", LanguageCode);
            return element;
        }

        public abstract XElement ToXElement();

        public string Class { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;
        public string Style { get; set; } = string.Empty;

        /// <summary>
        /// The ISO 639-1 language code for the language of the content.
        /// </summary>
        public string LanguageCode { get; set; } = string.Empty;
    }
}
