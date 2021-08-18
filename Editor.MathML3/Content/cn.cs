using System.Xml.Linq;

namespace Editor.MathML3.Content
{
    /// <summary>
    /// Numbers
    /// </summary>
    public sealed class cn : DefEncAtt
    {
        public override XElement ToXElement()
        {
            throw new System.NotImplementedException();
        }
        
        public string Type { get; set; } = string.Empty;
        public string Base { get; set; } = string.Empty;
    }
}
