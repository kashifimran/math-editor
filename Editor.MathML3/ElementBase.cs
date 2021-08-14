using System.Xml.Linq;

namespace Editor.MathML3
{
    public abstract class ElementBase : IMathMLElement, IXLinkAttributes
    {
        protected XElement AddElementAttributes(XElement element)
        {
            element.ApplyXLinkAttributes(this);
            element.AddMathMLAttribute("class", Class);
            element.AddMathMLAttribute("id", Id);
            element.AddMathMLAttribute("style", Style);
            element.AddXMLAttribute("lang", LanguageCode);
            element.Value = Content;
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

        public string Content { get; set; } = string.Empty;

        #region XLink
        string IXLinkAttributes.My { get; set; } = string.Empty;
        string IXLinkAttributes.Type { get; set; } = string.Empty;
        string IXLinkAttributes.Href { get; set; } = string.Empty;
        string IXLinkAttributes.Role { get; set; } = string.Empty;
        string IXLinkAttributes.Title { get; set; } = string.Empty;
        string IXLinkAttributes.Show { get; set; } = string.Empty;
        string IXLinkAttributes.Actuate { get; set; } = string.Empty;
        string IXLinkAttributes.Label { get; set; } = string.Empty;
        string IXLinkAttributes.From { get; set; } = string.Empty;
        string IXLinkAttributes.To { get; set; } = string.Empty;
        #endregion
    }
}
