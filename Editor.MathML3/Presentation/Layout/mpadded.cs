using System.Xml.Linq;

namespace Editor.MathML3
{
    /// <summary>
    /// Space around content
    /// </summary>
    public sealed class mpadded : LayoutBase
    {
        public override XElement ToXElement()
        {
            var element = new XElement(Ns.MathML + "mpadded");
            AddLayoutBaseAttributes(element);
            element.AddMathMLAttribute("depth", Depth);
            element.AddMathMLAttribute("height", Heigth);
            element.AddMathMLAttribute("lspace", LeftSpace);
            element.AddMathMLAttribute("voffset", VerticalOffset);
            element.AddMathMLAttribute("width", Width);
            return element;
        }

        public string Depth { get; set; } = string.Empty;
        public string Heigth { get; set; } = string.Empty;
        public string LeftSpace { get; set; } = string.Empty;
        public string VerticalOffset { get; set; } = string.Empty;
        public string Width { get; set; } = string.Empty;
    }
}
