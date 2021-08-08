using System.Xml.Linq;

namespace Editor.MathML3
{
    internal static class XElementExtensions
    {
        public static XElement AddMathMLAttribute(this XElement element, string name, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                element.Add(new XAttribute(Ns.MathML + name, value));
            }
            return element;
        }

        public static XElement AddXMLAttribute(this XElement element, string name, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                element.Add(new XAttribute(Ns.XML + name, value));
            }
            return element;
        }
    }
}
