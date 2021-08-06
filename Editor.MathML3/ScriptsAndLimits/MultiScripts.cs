using System.Xml.Linq;

namespace Editor.MathML3
{
    /// <summary>
    /// Prescripts and tensor indices
    /// </summary>
    public sealed class MultiScripts : IMathMLElement
    {
        public XElement ToXElement()
        {
            return new XElement(Ns.MathML + "mmultiscripts");
        }
    }
}
