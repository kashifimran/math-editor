using System.Xml.Linq;

namespace Editor.MathML3
{
    public static class XLinkAttributesExtensions
    {
        public static XElement ApplyXLinkAttributes(this XElement element, IXLinkAttributes attributes)
        {
            element.AddXLinkAttribute("my", attributes.My);
            element.AddXLinkAttribute("type", attributes.Type);
            element.AddXLinkAttribute("href", attributes.Href);
            element.AddXLinkAttribute("role", attributes.Role);
            element.AddXLinkAttribute("title", attributes.Title);
            element.AddXLinkAttribute("show", attributes.Show);
            element.AddXLinkAttribute("actuate", attributes.Actuate);
            element.AddXLinkAttribute("label", attributes.Label);
            element.AddXLinkAttribute("from", attributes.From);
            element.AddXLinkAttribute("to", attributes.To);
            return element;
        }

        public static T ApplyXLinkAttributes<T>(T target, XElement source)
            where T:IXLinkAttributes
        {
            target.My = source.Attribute(Ns.XLink + "my")?.Value ?? string.Empty;
            target.Type = source.Attribute(Ns.XLink + "type")?.Value ?? string.Empty;
            target.Href = source.Attribute(Ns.XLink + "href")?.Value ?? string.Empty;
            target.Role = source.Attribute(Ns.XLink + "role")?.Value ?? string.Empty;
            target.Title = source.Attribute(Ns.XLink + "title")?.Value ?? string.Empty;
            target.Show = source.Attribute(Ns.XLink + "show")?.Value ?? string.Empty;
            target.Actuate = source.Attribute(Ns.XLink + "actuate")?.Value ?? string.Empty;
            target.Label = source.Attribute(Ns.XLink + "label")?.Value ?? string.Empty;
            target.From = source.Attribute(Ns.XLink + "from")?.Value ?? string.Empty;
            target.To = source.Attribute(Ns.XLink + "to")?.Value ?? string.Empty;
            return target;
        }
    }
}
