using System.Xml.Linq;

namespace Editor.MathML3
{
    /// <summary>
    /// Enclosed syntax error messages
    /// </summary>
    public sealed class Error : LayoutBase
    {
        public override XElement ToXElement()
        {
            var element = new XElement(Ns.MathML + "merror");
            AddLayoutBaseAttributes(element);
            element.AddMathMLAttribute("open", Open);
            element.AddMathMLAttribute("close", Close);
            element.AddMathMLAttribute("separators", Separators);
            return element;
        }

        public string Open { get; set; } = string.Empty;
        public string Close { get; set; } = string.Empty;
        public string Separators { get; set; } = string.Empty;
    }
}
