using System.Xml.Linq;

namespace Editor.MathML3
{
    public sealed class Row : LayoutBase
    {
        public override XElement ToXElement()
        {
            var element = new XElement(Ns.MathML + "mrow");
            AddLayoutBaseAttributes(element);
            element.AddMathMLAttribute("dir", Dir);
            return element;
        }

        public string Dir { get; set; } = "ltr";
    }
}
