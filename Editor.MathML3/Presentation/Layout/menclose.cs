using System.Xml.Linq;

namespace Editor.MathML3
{
    /// <summary>
    /// Enclosed contents
    /// </summary>
    public sealed class menclose : LayoutBase
    {
        public override XElement ToXElement()
        {
            var element = new XElement(Ns.MathML + "menclose");
            AddElementAttributes(element);
            element.AddMathMLAttribute("notation", Notation);
            return element;
        }

        public string Notation { get; set; } = string.Empty;
    }
}
