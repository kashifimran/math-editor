using System.Xml.Linq;

namespace Editor.MathML3
{
    /// <summary>
    /// Operator
    /// </summary>
    public sealed class Operator : TokenBase
    {
        public override XElement ToXElement()
        {
            var element = new XElement(Ns.MathML + "mo");

            AddTokenAttributes(element);

            element.AddMathMLAttribute("accent", Accent);
            element.AddMathMLAttribute("fence", Fence);
            element.AddMathMLAttribute("lspace", LeftSpace);
            element.AddMathMLAttribute("mathsize", MathSize);
            element.AddMathMLAttribute("maxsize", MaxSize);
            element.AddMathMLAttribute("minsize", MinSize);
            element.AddMathMLAttribute("movablelimits", MovableLimits);
            element.AddMathMLAttribute("rspace", RightSpace);
            element.AddMathMLAttribute("separator", Separator);
            element.AddMathMLAttribute("stretchy", Stretchy);
            element.AddMathMLAttribute("symmetric", Symmetric);

            return element;
        }

        public string Accent { get; set; } = "false";
        public string Fence { get; set; } = "false";
        public string LeftSpace { get; set; } = string.Empty;
        public string MathSize { get; set; } = "normal";
        public string MaxSize { get; set; } = string.Empty;
        public string MinSize { get; set; } = string.Empty;
        public string MovableLimits { get; set; } = string.Empty;
        public string RightSpace { get; set; } = string.Empty;
        public string Separator { get; set; } = string.Empty;
        public string Stretchy { get; set; } = string.Empty;
        public string Symmetric { get; set; } = string.Empty;
    }
}
