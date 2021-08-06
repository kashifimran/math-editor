using System.Xml.Linq;

namespace Editor.MathML3
{
    public interface IMathMLElement
    {
        XElement ToXElement();
    }
}
